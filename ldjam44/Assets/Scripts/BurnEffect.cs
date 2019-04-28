using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BurnEffect : Effect {

    public float damage;
    public float time;
    
    public override void ApplyEffect(Character enemy)
    {
        enemy.RemoveEffectAfterTime(EffectsManager.Instance.burnEffect, time);
        enemy.currentStatusEffects.Add(EffectsManager.Instance.burnEffect);
    }

    public override void ProcessEffect(Character enemy)
    {
        enemy.ModifyHealth(-damage * Time.deltaTime );
    }

    public IEnumerator RemoveEffect(Character enemy)
    {
        yield return new WaitForSeconds(time);
        enemy.currentStatusEffects.Remove(EffectsManager.Instance.burnEffect);
    }
}
