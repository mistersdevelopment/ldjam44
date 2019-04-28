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

	private GameObject back;
	private GameObject front;
	private GameObject side;
	private GameObject[] eyesClosed;
	private Animator animator;

	private float nextFire = float.MinValue;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		playerStats = GetComponent<PlayerStats>();

		var playa = gameObject.transform.Find("Player");
		animator = playa.GetComponent<Animator>();
		back = playa.gameObject.transform.Find("Player_Back").gameObject;
		front = playa.gameObject.transform.Find("Player_Front").gameObject;
		side = playa.gameObject.transform.Find("Player_Side").gameObject;
		eyesClosed = new GameObject[2];
		eyesClosed[0] = side.gameObject.transform.Find("Player_Side_Eyes_Closed").gameObject;
		eyesClosed[1] = front.gameObject.transform.Find("Player_Front_Eyes_Closed").gameObject;
		UpdateSprites(CardinalDirection.SOUTH);
		StartCoroutine(Blinking());
	}

	void FixedUpdate()
	{
		body.velocity = new Vector2(horizontalMovement * playerStats.movementSpeed, verticalMovement * playerStats.movementSpeed);
	}

	void Update()
	{
		// Always call before attacks. Attacks takes precedent on the direction we are facing.
		ProcessMovement();
		ProcessAttacks();
	}

	void ProcessMovement()
	{
		horizontalMovement = Input.GetAxis("HorizontalMove");
		verticalMovement = Input.GetAxis("VerticalMove");
		animator.SetBool("Walking", Math.Abs(horizontalMovement) > 0.01 || Math.Abs(verticalMovement) > 0.01);

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
				UpdateSprites(facing);
			}
		}
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
				UpdateSprites(facing);
			}
		}

		// Mouse overrides keys.
		if (Input.GetMouseButton(0))
		{
			var mouseWorld = Input.mousePosition;
			mouseWorld = Camera.main.ScreenToWorldPoint(mouseWorld);
			mouseWorld.z = 0;

			var pos = transform.position;
			pos.z = 0;
			var mouseDir = Vector3.Normalize(mouseWorld - pos);

			var maxDot = float.MinValue;
			var maxDotDir = CardinalDirection.COUNT;
			for (int i = 0; i < (int)CardinalDirection.COUNT; i++)
			{
				var dotVal = Vector3.Dot(mouseDir, DirectionUtils.CardinalDirectionToVec((CardinalDirection)i));
				if (dotVal > maxDot)
				{
					maxDot = dotVal;
					maxDotDir = (CardinalDirection)i;
				}
			}
			if (maxDotDir != facing)
			{
				facing = maxDotDir;
				UpdateSprites(facing);
			}
		}

		if (ShouldFire() && Time.time > nextFire)
		{
			nextFire = Time.time + playerStats.rateOfFire;
			Fire(facing);
		}
	}

	bool ShouldFire()
	{
		return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.LeftArrow) || Input.GetMouseButton(0);
	}

	void Fire(CardinalDirection dir)
	{
		Vector3 fireVec = DirectionUtils.CardinalDirectionToVec(dir);
		Vector3 coinStart = transform.position + fireVec * 0.75f;
		GameObject coin = Instantiate(coinPrefab, coinStart, Quaternion.identity);
		Rigidbody2D coinRigidbody = coin.GetComponent<Rigidbody2D>();
		coin.transform.localScale = new Vector3(playerStats.shotSize, playerStats.shotSize, 1.0f);
		coinRigidbody.angularVelocity = GetComponent<Rigidbody2D>().angularVelocity;
		coinRigidbody.AddForce(fireVec * playerStats.shotSpeed);
		Coin coinScript = coin.GetComponent<Coin>();
		coinScript.SetLifetime(playerStats.shotLifetime);
		CollisionEffector collision = coin.GetComponent<CollisionEffector>();
		playerStats.baseEffect.damage = playerStats.damage;
		collision.SetBaseEffect(playerStats.baseEffect);
	}

	public void PowerUp(PlayerStats modifyPlayerstats)
	{
		playerStats.movementSpeed += modifyPlayerstats.movementSpeed;
		playerStats.shotSpeed += modifyPlayerstats.shotSpeed;
		playerStats.rateOfFire += modifyPlayerstats.rateOfFire;
		playerStats.damage += modifyPlayerstats.damage;
        playerStats.shotSize += modifyPlayerstats.shotSize;
        playerStats.baseEffect.damage = playerStats.damage;
		for (int i = 0; i < modifyPlayerstats.additionalEffects.Count; ++i)
		{
			playerStats.additionalEffects.Add(modifyPlayerstats.additionalEffects[i]);
		}
	}

	void UpdateSprites(CardinalDirection dir)
	{
		back.SetActive(dir == CardinalDirection.NORTH);
		front.SetActive(dir == CardinalDirection.SOUTH);
		side.SetActive(dir == CardinalDirection.EAST || dir == CardinalDirection.WEST);
		side.transform.localScale = new Vector3(dir == CardinalDirection.EAST ? -1 : 1, 1, 1);
	}

	void SetEyesOpen(bool open)
	{
		foreach (GameObject eye in eyesClosed)
		{
			eye.SetActive(!open);
		}
	}

	IEnumerator Blinking()
	{
		SetEyesOpen(true);
		while (true)
		{
			SetEyesOpen(false);
			yield return new WaitForSeconds(0.1f);
			SetEyesOpen(true);
			yield return new WaitForSeconds(UnityEngine.Random.Range(2, 4));
		}
	}
}
