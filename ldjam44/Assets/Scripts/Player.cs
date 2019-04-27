using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : MonoBehaviour
{
	public GameObject coinPrefab;
    public PlayerStats playerStats;

	Rigidbody2D body;
	private float horizontalMovement = 0;
	private float verticalMovement = 0;
	private CardinalDirection facing = CardinalDirection.SOUTH;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
        playerStats = GetComponent<PlayerStats>();
	}

	void FixedUpdate()
	{
		body.velocity = new Vector2(horizontalMovement * playerStats.movementSpeed, verticalMovement * playerStats.movementSpeed);
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
            InvokeRepeating("Fire", 0.0f, playerStats.rateOfFire);
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
		GameObject coin = Instantiate(coinPrefab, coinStart, Quaternion.identity);
        Rigidbody2D coinRigidbody = coin.GetComponent<Rigidbody2D>();
        Coin coinScript = coin.GetComponent<Coin>();
        coin.transform.localScale = new Vector3(playerStats.shotSize, playerStats.shotSize, 1.0f);
        coinRigidbody.velocity = GetComponent<Rigidbody2D>().velocity;
        coinRigidbody.AddForce(facingVec * playerStats.shotSpeed);
        coinScript.SetDamage(playerStats.damage);
	}

    void PowerUp(PlayerStats modifyPlayerstats)
    {
        playerStats.movementSpeed += modifyPlayerstats.movementSpeed;
        playerStats.shotSpeed += modifyPlayerstats.shotSpeed;
        playerStats.rateOfFire += modifyPlayerstats.rateOfFire;
        playerStats.damage += modifyPlayerstats.damage;
        playerStats.shotSize += modifyPlayerstats.shotSize;
    }
}
