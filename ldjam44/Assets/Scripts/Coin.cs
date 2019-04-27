using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public float damage;
    public List<AdditionalCoinEffect> additionalEffects;
    Rigidbody2D body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	public void SetDamage( float newDamage )
    {
        damage = newDamage;
    }

    public void SetEffects(List<AdditionalCoinEffect> newAdditionaleffects)
    {
        additionalEffects = newAdditionaleffects;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        Enemy enemyScript = other.GetComponent< Enemy >();
        if (enemyScript)
        {
            enemyScript.ModifyHealth(-damage);
            for (int i = 0; i < additionalEffects.Count; ++i)
            {
                additionalEffects[i].ApplyEffect(enemyScript);
            }
        }
        Destroy(this.gameObject);
    }
}
