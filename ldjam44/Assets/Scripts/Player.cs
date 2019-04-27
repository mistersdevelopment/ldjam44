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

	bool firing = false;

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
		horizontalMovement = Input.GetAxis("HorizontalMove");
		verticalMovement = Input.GetAxis("VerticalMove");
	}

	void ProcessAttacks()
	{
		var horizontalLook = Input.GetAxis("HorizontalLook");
		var verticalLook = Input.GetAxis("VerticalLook");
		Vector2 input = new Vector2(horizontalLook, verticalLook);
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

		if (Input.GetKeyDown(KeyCode.UpArrow) || Input.GetKeyDown(KeyCode.RightArrow) || Input.GetKeyDown(KeyCode.DownArrow) || Input.GetKeyDown(KeyCode.LeftArrow))
		{
            InvokeRepeating("Fire", 0.0f, playerStats.rateOfFire);
			firing = true;
		}
		if (!Input.GetKey(KeyCode.UpArrow) && !Input.GetKey(KeyCode.RightArrow) && !Input.GetKey(KeyCode.DownArrow) && !Input.GetKey(KeyCode.LeftArrow) && firing)
		{
			firing = false;
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
        coinScript.SetLifetime(playerStats.shotLifetime);
	}

    void PowerUp(PlayerStats modifyPlayerstats)
    {
        playerStats.movementSpeed += modifyPlayerstats.movementSpeed;
        playerStats.shotSpeed += modifyPlayerstats.shotSpeed;
        playerStats.rateOfFire += modifyPlayerstats.rateOfFire;
        playerStats.damage += modifyPlayerstats.damage;
        playerStats.shotSize += modifyPlayerstats.shotSize;
        for (int i = 0; i < modifyPlayerstats.additionalEffects.Count; ++i)
        {
            playerStats.additionalEffects.Add(modifyPlayerstats.additionalEffects[i]);
        }
    }
}
