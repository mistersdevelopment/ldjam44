using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pickup {

    public Stats powerup;

    public override void ApplyPowerup(Player player)
    {
        player.PowerUp(powerup);
    }
}
