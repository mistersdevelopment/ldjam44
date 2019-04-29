using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneShotAudio : MonoBehaviour
{
	private AudioSource source;
	private bool played = false;
	public void Play(AudioClip clip, float volume = 1f, float pitch = 1f)
	{
		if (!played)
		{
			played = true;
			source = gameObject.AddComponent<AudioSource>();
			source.playOnAwake = false;
			source.volume = volume;
			source.pitch = pitch;
			source.clip = clip;
			source.Play();
			Destroy(gameObject, clip.length);
		}
	}
}
