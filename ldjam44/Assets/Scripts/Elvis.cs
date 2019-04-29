using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Elvis : MonoBehaviour
{
    public Stats stats;
    Rigidbody2D body;
    Animator spritesAnimator;
    private Character character;

    bool moved = false;
    Vector2 movement = Vector2.zero;

    bool shooting = true;
    Vector3 randomAdjustment;

    float attackRange;

    bool active = false;

    void Start()
    {
        body = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        spritesAnimator = transform.Find("Sprites").GetComponent<Animator>();
        attackRange = Random.Range(5, 8);
        character = GetComponent<Character>();
    }

    void FixedUpdate()
    {
        var player = GameObject.Find("Player");
        moved = false;
        if (player && active)
        {
            var playerPos = player.transform.position;

            float shootingDistance = shooting ? attackRange + 0.3f : attackRange;
            shooting = Vector3.Distance(player.transform.position, transform.position) < shootingDistance;
            if (shooting)
            {
                CardinalDirection cd = GetDirectionToPlayer();
                Vector3 axis = new Vector3(1, 0, 0);
                Vector3 offset = new Vector3(0, 0, 0);
                if (cd == CardinalDirection.EAST || cd == CardinalDirection.WEST)
                {
                    axis = new Vector3(0, 1, 0);
                    offset = new Vector3(0, 0.3f, 0);
                }
                Vector3 tangent = new Vector3(-axis.y, axis.x, 0);
                float moveDir = Vector3.Dot(playerPos - (transform.position + offset), axis);
                bool linedUp = Mathf.Abs(moveDir) < 0.3f;
                if (!linedUp)
                {
                    axis *= moveDir;
                    axis = (axis * 0.7f) + (Vector3.Normalize(playerPos - transform.position) * 0.3f);
                    movement = Vector2.MoveTowards(transform.position, transform.position + axis, Time.deltaTime * stats.movementSpeed);
                    moved = true;
                    spritesAnimator.SetBool("IsWalking", true);
                    spritesAnimator.SetBool("IsShooting", false);
                }
                else
                {
                    spritesAnimator.SetBool("IsWalking", false);
                    spritesAnimator.SetBool("IsShooting", true);
                    randomAdjustment = new Vector3(Random.Range(-1.0f, 1.0f), Random.Range(-1.0f, 1.0f), 0) * attackRange / 2;
                }
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

        if (moved)
        {
            body.MovePosition(movement);
        }
    }

    public void Shoot()
    {
        character.Fire(GetDirectionToPlayer());
    }

    private CardinalDirection GetDirectionToPlayer()
    {
        var player = GameObject.Find("Player");
        if (player)
        {
            var playerDir = Vector3.Normalize(player.transform.position - transform.position);
            var northVal = Vector3.Dot(playerDir, DirectionUtils.CardinalDirectionToVec(CardinalDirection.NORTH));
            var southVal = Vector3.Dot(playerDir, DirectionUtils.CardinalDirectionToVec(CardinalDirection.SOUTH));
            var eastVal = Vector3.Dot(playerDir, DirectionUtils.CardinalDirectionToVec(CardinalDirection.EAST));
            var westVal = Vector3.Dot(playerDir, DirectionUtils.CardinalDirectionToVec(CardinalDirection.WEST));
            CardinalDirection cd = CardinalDirection.NORTH;
            if (northVal > southVal && northVal > eastVal && northVal > westVal)
            {
                cd = CardinalDirection.NORTH;
            }
            else if (southVal > eastVal && southVal > westVal)
            {
                cd = CardinalDirection.SOUTH;
            }
            else if (eastVal > westVal)
            {
                cd = CardinalDirection.EAST;
            }
            else
            {
                cd = CardinalDirection.WEST;
            }
            return cd;
        }
        return CardinalDirection.NORTH;
    }

    void OnRoomActivate()
    {
        active = true;
        var sounds = GetComponent<CharacterSounds>();
        if (sounds) sounds.EnableTaunt();
    }

}
