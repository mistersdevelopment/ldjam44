using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    public GameObject projectile;
    public GameObject drop;
    public float dropChance; // 0 to 1
    public Stats stats;

	public float maxHealth;
	public float currentHealth;
    public float invincibilityTime;
    private bool canTakeDamage;
	private float nextFire = float.MinValue;
    public List<Effect> currentStatusEffects;

    private CharacterSounds sounds;

	void Start()
	{
		stats = GetComponent<Stats>();
		currentHealth = maxHealth;
        sounds = GetComponent<CharacterSounds>();
        canTakeDamage = true;
        if (!sounds)
        {
            sounds = gameObject.AddComponent<CharacterSounds>();
        }
    }

    void Update()
    {
        for (int i = 0; i < currentStatusEffects.Count; ++i)
        {
            currentStatusEffects[i].ProcessEffect(this);
        }
    }

    public void ModifyHealth(float modification)
    {
        if (canTakeDamage)
        {
            canTakeDamage = false;
            StartCoroutine("InvincibilityPeriod");
            currentHealth += modification;
            if (currentHealth <= 0)
            {
                Die();
            }
            else if (currentHealth > maxHealth)
            {
                currentHealth = maxHealth;
            }
            else if (modification < 0)
            {
                sounds.Damage();
            } 
            Player playerComponent = GetComponent<Player>();
            if (playerComponent)
            {
                playerComponent.SetHealth( currentHealth );
            }
        }
    }

    private IEnumerator InvincibilityPeriod()
    {
        yield return new WaitForSeconds(invincibilityTime);
        canTakeDamage = true;
    }

    public void Fire(CardinalDirection dir)
	{
		if (projectile && Time.time > nextFire)
		{
            sounds.Shoot();
			nextFire = Time.time + stats.rateOfFire;
			FireOne(dir);
		}
	}

	void FireOne(CardinalDirection dir)
	{
		if (projectile)
		{
			Vector3 fireVec = DirectionUtils.CardinalDirectionToVec(dir);
			Vector3 startPos = transform.position + fireVec * 0.75f;
			GameObject proj = Instantiate(projectile, startPos, Quaternion.identity);
			Rigidbody2D projRigidbody = proj.GetComponent<Rigidbody2D>();
			proj.transform.localScale = new Vector3(stats.shotSize, stats.shotSize, 1.0f);
			projRigidbody.angularVelocity = GetComponent<Rigidbody2D>().angularVelocity;
			projRigidbody.AddForce(fireVec * stats.shotSpeed);
			Projectile projScript = proj.GetComponent<Projectile>();
			projScript.SetLifetime(stats.shotLifetime);
			CollisionEffector collision = proj.GetComponent<CollisionEffector>();
			stats.baseEffect.damage = stats.damage;
			collision.SetBaseEffect(stats.baseEffect);
            collision.SetEffects(stats.additionalEffects);
		}
	}

	public void PowerUp(Stats modifyPlayerstats)
	{
		stats.movementSpeed += modifyPlayerstats.movementSpeed;
		stats.shotSpeed += modifyPlayerstats.shotSpeed;
		stats.rateOfFire += modifyPlayerstats.rateOfFire;
		stats.damage += modifyPlayerstats.damage;
		stats.shotSize += modifyPlayerstats.shotSize;
		for (int i = 0; i < modifyPlayerstats.additionalEffects.Count; ++i)
		{
            Debug.Log("Adding Burn Effect to playershot");
			stats.additionalEffects.Add(modifyPlayerstats.additionalEffects[i]);
		}
	}

    class EffectTime
    {
        public Effect effect;
        public float time;
    }

    public void RemoveEffectAfterTime(Effect effectToRemove, float time)
    {
        EffectTime newEffectTime = new EffectTime();
        newEffectTime.effect = effectToRemove;
        newEffectTime.time = time;
        StartCoroutine("RemoveEffect", newEffectTime);
    }

    private IEnumerator RemoveEffect(EffectTime effectTime)
    {
        yield return new WaitForSeconds(effectTime.time);
        currentStatusEffects.Remove(effectTime.effect);
    }

    public void Die()
    {
        if (drop && Random.Range(0.0f, 1.0f) < dropChance)
        {
            var room = GameObject.FindGameObjectsWithTag("GameManager")[0].GetComponent<GameManager>().GetActiveRoom();
            var go = Instantiate(drop, transform.position, transform.rotation);
            go.transform.parent = room.transform;
        }
        Player playerComponent = GetComponent<Player>();
        if (playerComponent)
        {
            playerComponent.Die();
        }
        Destroy(this.gameObject);
    }
}