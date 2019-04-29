using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Granny : MonoBehaviour
{
	public Stats stats;
	Rigidbody2D body;
	Animator spritesAnimator;

	bool moved = false;
	Vector2 movement = Vector2.zero;

    bool tired = false;
    float energy = 6;
    float maxEnergy = 6;
    float weakness;

    bool active = false;

	void Start()
	{
		body = GetComponent<Rigidbody2D>();
		stats = GetComponent<Stats>();
		spritesAnimator = transform.Find("Sprites").GetComponent<Animator>();
        weakness = Random.Range(0, 2);
	}

    void Update()
    {
        var player = GameObject.Find("Player");
        if (player && !tired && active)
        {
            var playerPos = player.transform.position;
            movement = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime * stats.movementSpeed * (energy / maxEnergy));
            var grannyPos = transform.position;
            grannyPos.z = 0;
            var playerDir = Vector3.Normalize(playerPos - grannyPos);

            var eastVal = Vector3.Dot(playerDir, DirectionUtils.CardinalDirectionToVec(CardinalDirection.EAST));
            var westVal = Vector3.Dot(playerDir, DirectionUtils.CardinalDirectionToVec(CardinalDirection.WEST));
            if (eastVal > westVal)
            {
                transform.localScale = new Vector3(1, 1, 1);
            }
            else
            {
                transform.localScale = new Vector3(-1, 1, 1);
            }

            moved = true;
            spritesAnimator.SetBool("IsWalking", moved);

            energy -= Time.deltaTime + weakness * Time.deltaTime;

            if (energy <= 0)
            {
                tired = true;
            }
        }
        else
        {
            energy += Time.deltaTime * 2;
            if (energy >= maxEnergy)
            {
                tired = false;
                maxEnergy = energy;
            }
            movement = transform.position;
            spritesAnimator.SetBool("IsWalking", false);
        }
    }

	void FixedUpdate()
	{
		if (moved)
		{
			body.MovePosition(movement);
		}
	}

    void OnRoomActivate()
    {
        active = true;
    }
}
