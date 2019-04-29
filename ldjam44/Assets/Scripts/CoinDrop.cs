using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoinDrop : Pickup
{
	private Rigidbody2D rb;

	// Use this for initialization
	void Start()
	{
		rb = this.GetComponent<Rigidbody2D>();
		float dir = Random.Range(0, Mathf.PI * 2);
		Vector2 force = new Vector3(Mathf.Cos(dir), Mathf.Sin(dir), 0);
		rb.AddForce(force * Random.Range(100, 200));
	}

	// Update is called once per frame
	void Update()
	{
	}

	public override bool ApplyPowerup(Player player)
	{
		var character = player.GetComponent<Character>();
		if (character.currentHealth < character.maxHealth)
		{
			player.GetComponent<Character>().ModifyHealth(1);
			return true;
		}
		return false;
	}
}
