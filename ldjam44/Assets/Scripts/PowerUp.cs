using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pickup {

    public PlayerStats powerup;

    public override void ApplyPowerup(Player player)
    {
        player.PowerUp(powerup);
    }
}
