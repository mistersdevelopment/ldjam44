using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour
{
	private static AudioSource globalTaunt = null;

	public AudioClip[] footstep = new AudioClip[0];
	public float footstepVolume = 1;
	private AudioSource footstepSource;

	public AudioClip[] shoot = new AudioClip[0];
	public float shootVolume = 1;
	private AudioSource shootSource;

	public AudioClip[] damage = new AudioClip[0];
	public float damageVolume = 1;
	private AudioSource damageSource;

	public AudioClip[] taunt = new AudioClip[0];
	public float tauntVolume = 1;
	private AudioSource tauntSource;

	public AudioClip[] death = new AudioClip[0];
	public float deathVolume = 1;
	private AudioSource deathSource;

	private bool canTaunt = false;


	// Use this for initialization
	void Start()
	{
		footstepSource = gameObject.AddComponent<AudioSource>();
		footstepSource.volume = footstepVolume * 0.7f;
		shootSource = gameObject.AddComponent<AudioSource>();
		shootSource.volume = shootVolume * 0.5f;
		damageSource = gameObject.AddComponent<AudioSource>();
		damageSource.volume = damageVolume * 0.6f;
		tauntSource = gameObject.AddComponent<AudioSource>();
		tauntSource.volume = tauntVolume * 0.7f;
		deathSource = gameObject.AddComponent<AudioSource>();
		deathSource.volume = deathVolume * 0.6f;
		AudioHighPassFilter filter = gameObject.AddComponent<AudioHighPassFilter>();
		filter.cutoffFrequency = 100;
		filter.highpassResonanceQ = 1;
	}

	// Update is called once per frame
	void Update()
	{
		if (Random.Range(0, 4000) == 0)
		{
			Taunt();
		}
	}

	private AudioClip PlayRandomSound(AudioSource source, AudioClip[] clips)
	{
		if (clips.Length > 0)
		{
			source.clip = clips[Random.Range(0, clips.Length)];
			source.Play();
			return source.clip;
		}
		return null;
	}

	public bool IsTalking()
	{
		if (tauntSource.isPlaying) return true;
		if (damageSource.isPlaying) return true;
		return false;
	}

	public void StopTalking()
	{
		if (tauntSource.isPlaying) tauntSource.Stop();
		if (damageSource.isPlaying) damageSource.Stop();
	}

	public void Footstep()
	{
		PlayRandomSound(footstepSource, footstep);
	}
	public void Shoot()
	{
		PlayRandomSound(shootSource, shoot);
	}
	public void Damage()
	{
		if (damageSource.isPlaying) return;
		StopTalking();
		PlayRandomSound(damageSource, damage);
	}
	public void Taunt()
	{
		if (!canTaunt) return;
		if (globalTaunt && globalTaunt.isPlaying) return;
		if (IsTalking()) return;
		PlayRandomSound(tauntSource, taunt);
		globalTaunt = tauntSource;
	}

	public void Die()
	{
		var clip = PlayRandomSound(deathSource, death);
		if (clip)
		{
			var go = new GameObject();
			var oneShot = go.AddComponent<OneShotAudio>();
			oneShot.Play(clip);
		}
	}

	public void EnableTaunt()
	{
		canTaunt = true;
	}
}
