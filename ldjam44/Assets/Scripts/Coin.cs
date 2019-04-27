using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public int damage;
    Rigidbody2D body;
	// Use this for initialization
	void Start () {
        body = GetComponent<Rigidbody2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
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
