using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    protected bool allowPickup = true;

	void OnCollisionEnter2D(Collision2D collision)
    {
        if (allowPickup)
        {
            GameObject other = collision.gameObject;
            Character playerScript = other.GetComponent<Character>();
            if (playerScript)
            {
                ApplyPowerup(playerScript);
                Destroy(this.gameObject);
            }
        }
	}

	public virtual void ApplyPowerup(Character player)
	{
	}
}
