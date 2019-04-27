using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        Player playerScript = other.GetComponent<Player>();
        if (playerScript)
        {
            ApplyPowerup(playerScript);
        }
        Destroy(this.gameObject);
    }

    public virtual void ApplyPowerup(Player player)
    {
    }
}
