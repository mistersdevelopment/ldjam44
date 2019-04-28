using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffector : MonoBehaviour
{
	public Effect baseEffect;
	public List<Effect> additionalEffects;

	public void SetBaseEffect(Effect newBaseEffect)
	{
		baseEffect = newBaseEffect;
	}

	public void SetEffects(List<Effect> newAdditionaleffects)
	{
		additionalEffects = newAdditionaleffects;
	}

	void OnCollisionEnter2D(Collision2D collision)
	{
		GameObject other = collision.gameObject;
		Character enemyScript = other.GetComponent<Character>();
		if (enemyScript)
		{
			if (baseEffect)
			{
				baseEffect.ApplyEffect(enemyScript);
			}
			for (int i = 0; i < additionalEffects.Count; ++i)
			{
				additionalEffects[i].ApplyEffect(enemyScript);
			}
		}
		Destroy(this.gameObject);
	}
}
