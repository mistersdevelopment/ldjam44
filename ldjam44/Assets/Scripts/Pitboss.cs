using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pitboss : MonoBehaviour
{
    public float waitTime = 3;
    public float waitTimeVariance = 2;
    public float chargeUpTime = 1;
    public float chargeTime = 1;
    public AudioClip chargeClip;

    public Stats stats;
    Rigidbody2D body;
    Animator spritesAnimator;

    bool moved = false;
    Vector2 movement = Vector2.zero;

    float waitTimer;
    float chargeUpTimer;
    float chargeTimer;
    Vector3 direction;

    AudioSource chargeupAudioSource;

    bool active = false;

    void Start()
    {
        waitTimer = waitTime + Random.Range(0, waitTimeVariance);
        chargeUpTimer = chargeUpTime;
        chargeTimer = chargeTime;
        body = GetComponent<Rigidbody2D>();
        stats = GetComponent<Stats>();
        spritesAnimator = transform.Find("Sprites").GetComponent<Animator>();
        chargeupAudioSource = gameObject.AddComponent<AudioSource>();
        chargeupAudioSource.loop = true;
        chargeupAudioSource.volume = 1.0f;
    }

    void Update()
    {
        var player = GameObject.Find("Player");
        movement = transform.position;
        if (player && active)
        {
            if (waitTimer > 0)
            {
                waitTimer -= Time.deltaTime;
                spritesAnimator.SetBool("IsCharging", false);
                spritesAnimator.SetBool("IsChargingUp", false);
                if (waitTimer < 0 && chargeClip)
                {
                    chargeupAudioSource.clip = chargeClip;
                    chargeupAudioSource.Play();
                }
            }
            else if (chargeUpTimer > 0)
            {
                chargeUpTimer -= Time.deltaTime;
                if (chargeUpTimer <= 0)
                {
                    direction = Vector3.Normalize(player.transform.position - transform.position);
                    chargeTimer = chargeTime;
                    if (chargeClip)
                    {
                        chargeupAudioSource.Stop();
                    }
                }
                spritesAnimator.SetBool("IsChargingUp", true);
            }
            else
            {
                spritesAnimator.SetBool("IsCharging", true);
                chargeTimer -= Time.deltaTime;
                if (chargeTimer < 0)
                {
                    waitTimer = waitTime + Random.Range(0, waitTimeVariance);
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
        else
        {
            spritesAnimator.SetBool("IsCharging", false);
            spritesAnimator.SetBool("IsChargingUp", false);
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

    void OnRoomActivate()
    {
        active = true;
    }
}
