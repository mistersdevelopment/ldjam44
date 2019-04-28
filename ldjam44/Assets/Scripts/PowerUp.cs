using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : Pickup {

    public Stats powerup;

    public override void ApplyPowerup(Character player)
    {
        player.PowerUp(powerup);
    }
}
