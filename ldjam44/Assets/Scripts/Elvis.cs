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

    bool shooting = true;
    Vector3 randomAdjustment;

    float attackRange;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        spritesAnimator = transform.Find("Sprites").GetComponent<Animator>();
        attackRange = Random.Range(3, 8);
    }

    void Update()
    {
        var player = GameObject.Find("Player");
        moved = false;
        if (player)
        {
            var playerPos = player.transform.position;

            float shootingDistance = shooting ? attackRange + 1.0f : attackRange;
            shooting = Vector3.Distance(player.transform.position, transform.position) < shootingDistance;
            if (shooting)
            {
                spritesAnimator.SetBool("IsWalking", false);
                spritesAnimator.SetBool("IsShooting", true);
                randomAdjustment = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0) * attackRange / 2;
            }
            else
            {
                movement = Vector2.MoveTowards(transform.position, playerPos + randomAdjustment, Time.deltaTime * stats.movementSpeed);

                moved = true;
                spritesAnimator.SetBool("IsWalking", moved);
                spritesAnimator.SetBool("IsShooting", false);

                moved = true;
            }
            var playerDir = Vector3.Normalize(playerPos - transform.position);

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
        }
        else
        {
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
}
