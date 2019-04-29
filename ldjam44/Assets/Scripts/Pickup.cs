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
            Player playerScript = other.GetComponent<Player>();
            if (playerScript)
            {
                ApplyPowerup(playerScript);
                Destroy(this.gameObject);
            }
        }
	}

	public virtual void ApplyPowerup(Player player)
	{
	}
}
