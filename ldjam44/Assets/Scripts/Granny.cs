using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour
{
	public Stats stats;
	Rigidbody2D body;

	bool move = false;
	Vector2 movement = Vector2.zero;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		stats = GetComponent<Stats>();
	}

	void Update()
	{
		// TODO Make granny randomly stop. Because tired.
		var player = GameObject.Find("Player");
		if (player)
		{
			move = true;
			movement = Vector2.MoveTowards(gameObject.transform.position, player.gameObject.transform.position, Time.deltaTime * stats.movementSpeed);
			// TODO If within range attack.
			// ProcessAttacks();
		}
	}

	void FixedUpdate()
	{
		if (move)
		{
			body.MovePosition(movement);
		}
	}
}
