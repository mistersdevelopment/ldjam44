using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : Effect
{
    public float damage = 1.0f;

    public override void ApplyEffect(Character character)
    {
        character.ModifyHealth(-damage);
    }
}
