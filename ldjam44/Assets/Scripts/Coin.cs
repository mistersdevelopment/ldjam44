using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
	public float lifetime;

	// Use this for initialization
	void Start()
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

	public void SetLifetime(float newLifetime)
	{
		lifetime = newLifetime;
	}
}
