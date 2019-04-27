using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	public Rigidbody2D coinPrefab;

	float movementSpeed = 2.0f;
	float shotSpeed = 100f;

	Rigidbody2D body;
	private float horizontalMovement = 0;
	private float verticalMovement = 0;
	private CardinalDirection facing = CardinalDirection.SOUTH;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
	}

	void FixedUpdate()
	{
		body.velocity = new Vector2(horizontalMovement * movementSpeed, verticalMovement * movementSpeed);
	}

	void Update()
	{
		ProcessMovement();
		ProcessAttacks();
	}

	void ProcessMovement()
	{
		horizontalMovement = Input.GetAxis("Horizontal");
		verticalMovement = Input.GetAxis("Vertical");

		Vector2 input = new Vector2(horizontalMovement, verticalMovement);
		input.x = (float)(Math.Sign(input.x) * Math.Ceiling(Math.Abs(input.x)));
		input.y = (float)(Math.Sign(input.y) * Math.Ceiling(Math.Abs(input.y)));
		input = input.normalized;

		if (input != Vector2.zero)
		{
			CardinalDirection dir = DirectionUtils.CoordinatesToCardinalDirection(input);

			if (dir != facing)
			{
				facing = dir;
			}
		}
	}

	void ProcessAttacks()
	{
		if (Input.GetButtonDown("Fire1"))
		{
			InvokeRepeating("Fire", 0.0f, 0.3f);
		}
		if (Input.GetButtonUp("Fire1"))
		{
			CancelInvoke("Fire");
		}
	}

	void Fire()
	{
		Vector3 facingVec = DirectionUtils.CardinalDirectionToVec(facing);
		Vector3 coinStart = transform.position + facingVec * 0.75f;
		Rigidbody2D coin = Instantiate(coinPrefab, coinStart, Quaternion.identity) as Rigidbody2D;
		coin.velocity = GetComponent<Rigidbody2D>().velocity;
		coin.AddForce(facingVec * shotSpeed);
	}
}
