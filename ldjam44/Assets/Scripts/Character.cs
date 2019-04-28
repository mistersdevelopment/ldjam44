using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	public GameObject projectile;
	public Stats stats;

	public float maxHealth;
	public float currentHealth;

	private float nextFire = float.MinValue;

	void Start()
	{
		stats = GetComponent<Stats>();
		currentHealth = maxHealth;
	}

	public void ModifyHealth(float modification)
	{
		currentHealth += modification;
		if (currentHealth <= 0)
		{
			Destroy(this.gameObject);
		}
	}

	public void Fire(CardinalDirection dir)
	{
		if (projectile && Time.time > nextFire)
		{
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
			stats.additionalEffects.Add(modifyPlayerstats.additionalEffects[i]);
		}
	}
}