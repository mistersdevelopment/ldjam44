using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elvis : MonoBehaviour
{
    public Stats stats;
    Rigidbody2D body;
    Animator spritesAnimator;

    bool moved = false;
    Vector2 movement = Vector2.zero;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        spritesAnimator = transform.Find("Sprites").GetComponent<Animator>();
    }

    void Update()
    {
        // TODO Make granny randomly stop. Because tired.
        var player = GameObject.Find("Player");
        if (player)
        {
            var playerPos = player.transform.position;
            movement = Vector2.MoveTowards(transform.position, playerPos, Time.deltaTime * stats.movementSpeed);
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
        }
    }

    void FixedUpdate()
    {
        if (moved)
        {
            body.MovePosition(movement);
        }
    }
}
