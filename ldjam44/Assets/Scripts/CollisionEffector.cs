using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollisionEffector : MonoBehaviour
{
	public bool destroySelfOnCollison = false;
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

	void OnCollisionStay2D(Collision2D collision)
	{
		GameObject other = collision.gameObject;
		if (other.layer != gameObject.layer)
		{
			Character character = other.GetComponent<Character>();
			if (character)
			{
				if (baseEffect)
				{
					baseEffect.ApplyEffect(character);
				}
				for (int i = 0; i < additionalEffects.Count; ++i)
				{
					additionalEffects[i].ApplyEffect(character);
				}
			}
			if (destroySelfOnCollison)
			{
				Destroy(this.gameObject);
			}
		}
	}
}
