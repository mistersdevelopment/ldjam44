using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour 
{
    public int maxHealth;
    public int currentHealth;

	// Use this for initialization
	void Start () 
    {
        currentHealth = maxHealth;
	}
	
	// Update is called once per frame
	void Update () 
    {
		
	}

    public void ModifyHealth(int modification)
    {
        currentHealth += modification;
        if (currentHealth <= 0)
        {
            Destroy(this.gameObject);
        }
    }
}
