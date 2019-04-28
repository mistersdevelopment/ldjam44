using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pickup {

    private Rigidbody2D rb;

    public Stats powerup;

    // Use this for initialization
    void Start()
    {
        allowPickup = false;

        rb = this.GetComponent<Rigidbody2D>();
        float dir = Random.Range(0, Mathf.PI * 2);
        Vector2 force = new Vector3(Mathf.Cos(dir), Mathf.Sin(dir), 0);
        rb.AddForce(force * Random.Range(100, 200));
        StartCoroutine(BecomeActiveAfterDelay());
    }

    public override void ApplyPowerup(Character player)
    {
        player.PowerUp(powerup);
    }

    IEnumerator BecomeActiveAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        allowPickup = true;
    }
}
