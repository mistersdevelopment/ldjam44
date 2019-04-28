using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitboss : MonoBehaviour
{
    public float waitTime = 3;
    public float chargeUpTime = 1;
    public float chargeTime = 1;

    public Stats stats;
    Rigidbody2D body;
    Animator spritesAnimator;

    bool moved = false;
    Vector2 movement = Vector2.zero;

    float waitTimer;
    float chargeUpTimer;
    float chargeTimer;
    Vector3 direction;

    void Start()
    {
        waitTimer = waitTime;
        chargeUpTimer = chargeUpTime;
        chargeTimer = chargeTime;
        body = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        spritesAnimator = transform.Find("Sprites").GetComponent<Animator>();
    }

    void Update()
    {
        var player = GameObject.Find("Player");
        movement = transform.position;
        if (player)
        {
            if (waitTimer > 0)
            {
                waitTimer -= Time.deltaTime;
                spritesAnimator.SetBool("IsCharging", false);
                spritesAnimator.SetBool("IsChargingUp", false);
            }
            else if (chargeUpTimer > 0)
            {
                chargeUpTimer -= Time.deltaTime;
                if (chargeUpTimer <= 0)
                {
                    direction = Vector3.Normalize(player.transform.position - transform.position);
                    chargeTimer = chargeTime;
                }
                spritesAnimator.SetBool("IsChargingUp", true);
            }
            else
            {
                spritesAnimator.SetBool("IsCharging", true);
                chargeTimer -= Time.deltaTime;
                if (chargeTimer < 0)
                {
                    waitTimer = waitTime;
                    chargeUpTimer = chargeUpTime;
                }

                movement = Vector2.MoveTowards(transform.position, transform.position + direction, Time.deltaTime * stats.movementSpeed);

                if (movement.x > 0)
                {
                    transform.localScale = new Vector3(1, 1, 1);
                }
                else
                {
                    transform.localScale = new Vector3(-1, 1, 1);
                }

                //spritesAnimator.SetBool("IsWalking", moved);
            }
        }
        moved = true;
    }

    void FixedUpdate()
    {
        if (moved)
        {
            body.MovePosition(movement);
        }
    }
}
