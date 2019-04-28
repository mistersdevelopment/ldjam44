using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour
{
	public EnemyStats enemyStats;
	Rigidbody2D body;

	bool move = false;
	Vector2 movement = Vector2.zero;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		enemyStats = GetComponent<EnemyStats>();
	}

	void Update()
	{
		// TODO Make granny randomly stop. Because tired.
		var player = GameObject.Find("Player");
		if (player)
		{
			move = true;
			movement = Vector2.MoveTowards(gameObject.transform.position, player.gameObject.transform.position, Time.deltaTime * enemyStats.movementSpeed);
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
