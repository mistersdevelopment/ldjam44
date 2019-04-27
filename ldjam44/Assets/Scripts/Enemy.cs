using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public float maxHealth;
    public float currentHealth;

	// Use this for initialization
	void Start () 
    {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void ModifyHealth(float modification)
    {
        currentHealth += modification;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
