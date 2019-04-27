using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStats : MonoBehaviour {

    public float movementSpeed = 2.0f;
    public float shotSpeed = 100f;
    public float rateOfFire = 0.3f;
    public float damage = 1.0f;
    public float shotSize = 0.5f;
    public float shotLifetime = 1.0f;
    public BaseCoinEffect baseEffect;
    public List<AdditionalCoinEffect> additionalEffects;
}
