using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour {

    public float lifetime;
    public AdditionalCoinEffect baseEffect;
    public List<AdditionalCoinEffect> additionalEffects;
	// Use this for initialization
	void Start () 
    {
	}

    void Update()
    {
        lifetime -= Time.deltaTime;
        if (lifetime <= 0f)
        {
            Destroy(this.gameObject);
        }

    }

    public void SetBaseEffect(AdditionalCoinEffect newBaseEffect)
    {
        baseEffect = newBaseEffect;
    }

    public void SetLifetime(float newLifetime)
    {
        lifetime = newLifetime;
    }

    public void SetEffects(List<AdditionalCoinEffect> newAdditionaleffects)
    {
        additionalEffects = newAdditionaleffects;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        GameObject other = collision.gameObject;
        Enemy enemyScript = other.GetComponent< Enemy >();
        if (enemyScript)
        {
            baseEffect.ApplyEffect(enemyScript);
            for (int i = 0; i < additionalEffects.Count; ++i)
            {
                additionalEffects[i].ApplyEffect(enemyScript);
            }
        }
        Destroy(this.gameObject);
    }
}
