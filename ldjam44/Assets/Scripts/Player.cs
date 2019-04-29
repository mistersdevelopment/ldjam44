using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System;

public class Player : MonoBehaviour
{
	private Rigidbody2D body;
	private Character character;
	private Stats stats;
	private float horizontalMovement = 0;
	private float verticalMovement = 0;
    private int[] powerupsCount = new int[4];
    private CardinalDirection facing = CardinalDirection.SOUTH;

	private GameObject back;
	private GameObject front;
	private GameObject side;
	private GameObject[] eyesClosed;
    private Animator animator;
	public GameObject[] healthUI;
    public Text[] powerupCountUI;


	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		character = GetComponent<Character>();
		stats = GetComponent<Stats>();

		var playa = transform.Find("Sprites");
		animator = playa.GetComponent<Animator>();
		back = playa.transform.Find("Player_Back").gameObject;
		front = playa.transform.Find("Player_Front").gameObject;
		side = playa.transform.Find("Player_Side").gameObject;
		eyesClosed = new GameObject[2];
		eyesClosed[0] = side.transform.Find("Player_Side_Eyes_Closed").gameObject;
		eyesClosed[1] = front.transform.Find("Player_Front_Eyes_Closed").gameObject;
		UpdateSprites(CardinalDirection.SOUTH);
		StartCoroutine(Blinking());
	}

	void FixedUpdate()
	{
		body.velocity = new Vector2(horizontalMovement * stats.movementSpeed, verticalMovement * stats.movementSpeed);
	}

	void Update()
	{
		if (!GameManager.Instance.IsUpgradeMachineOpen())
		{
			// Always call before attacks. Attacks takes precedent on the direction we are facing.
			ProcessMovement();
			ProcessAttacks();
		}
        else
        {
            horizontalMovement = 0f;
            verticalMovement = 0f;
        }
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

		if (ShouldFire())
		{
			character.Fire(facing);
		}
	}

	bool ShouldFire()
	{
		return Input.GetKey(KeyCode.UpArrow) || Input.GetKey(KeyCode.RightArrow) || Input.GetKey(KeyCode.DownArrow) ||
			Input.GetKey(KeyCode.LeftArrow) || (Input.GetMouseButton(0) && !EventSystem.current.IsPointerOverGameObject());
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

	public Stats GetStats()
	{
		return stats;
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

	public void SetHealth(float health)
	{
		for (int i = 0; i < health; ++i)
		{
			healthUI[i].SetActive(true);
		}
		for (int i = (int)health; i < 5; ++i)
		{
			healthUI[i].SetActive(false);
		}
	}

	public void Die()
	{
        GameManager.Instance.ShowDeathScreen();
        Destroy(this);
    }

    public void AddPowerUp(PowerUpType pupType)
    {
        switch (pupType)
        {
            case PowerUpType.Speed: powerupsCount[0]++; break;
            case PowerUpType.RateOfFire: powerupsCount[1]++; break;
            case PowerUpType.ShotSize: powerupsCount[2]++; break;
            case PowerUpType.Damage: powerupsCount[3]++; break;
        }

        for (int i = 0; i < powerupCountUI.Length; i++)
        {
            Text countUI = powerupCountUI[i];
            countUI.text = powerupsCount[i].ToString();
        }
	}
}
