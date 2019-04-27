using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
	public float movementSpeed = 1.0f;

	Rigidbody2D body;
	private float horizontalMovement = 0;
	private float verticalMovement = 0;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
	}

	void Update()
	{
		PerformMovement();
	}

	void PerformMovement()
	{
		horizontalMovement = Input.GetAxis("Horizontal");
		verticalMovement = Input.GetAxis("Vertical");

	}
	void FixedUpdate()
	{
		body.velocity = new Vector2(horizontalMovement * movementSpeed, verticalMovement * movementSpeed);
	}
}
