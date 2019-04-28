using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject other = collision.gameObject;
		Character playerScript = other.GetComponent<Character>();
		if (playerScript)
		{
			ApplyPowerup(playerScript);
		}
		Destroy(this.gameObject);
	}

	public virtual void ApplyPowerup(Character player)
	{
	}
}
