using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseCoinEffect : AdditionalCoinEffect
{
    public float damage = 1.0f;

    public override void ApplyEffect(Enemy enemy)
    {
        enemy.ModifyHealth(-damage);
    }
}
