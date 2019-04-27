using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pickup {

    public PlayerStats powerup;

    public override void ApplyPowerup(Player player)
    {
        PlayerStats playerStats = player.GetComponent<PlayerStats>();
        playerStats.movementSpeed += powerup.movementSpeed;
        playerStats.shotSpeed += powerup.shotSpeed;
        playerStats.rateOfFire += powerup.rateOfFire;
        playerStats.damage += powerup.damage;
        playerStats.shotSize += powerup.shotSize;
        playerStats.shotLifetime += powerup.shotLifetime;
        playerStats.baseEffect.damage += powerup.damage;
        if (powerup.baseEffect)
        {
            playerStats.baseEffect = powerup.baseEffect;
        }

        for (int i = 0; i < powerup.additionalEffects.Count; ++i)
        {
            playerStats.additionalEffects.Add(powerup.additionalEffects[i]);
        }
    }
}
