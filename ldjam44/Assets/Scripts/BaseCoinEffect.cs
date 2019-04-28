﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCoinEffect : Effect
{
    public float damage = 1.0f;

    public override void ApplyEffect(Health enemy)
    {
        enemy.ModifyHealth(-damage);
    }
}
