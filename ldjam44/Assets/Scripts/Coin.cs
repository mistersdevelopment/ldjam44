using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public float damage;
    Rigidbody2D body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	public void SetDamage( float newDamage )
    {
        damage = newDamage;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        Enemy enemyScript = other.GetComponent< Enemy >();
        if (enemyScript)
        {
            enemyScript.ModifyHealth(-damage);
        }
        Destroy(this.gameObject);
    }
}
