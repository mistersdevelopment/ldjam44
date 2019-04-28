using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSounds : MonoBehaviour
{
    private static AudioSource globalTaunt = null;

    public AudioClip[] footstep = new AudioClip[0];
    private AudioSource footstepSource;

    public AudioClip[] shoot = new AudioClip[0];
    private AudioSource shootSource;

    public AudioClip[] damage = new AudioClip[0];
    private AudioSource damageSource;

    public AudioClip[] taunt = new AudioClip[0];
    private AudioSource tauntSource;


    // Use this for initialization
    void Start() {
        footstepSource = gameObject.AddComponent<AudioSource>();
        footstepSource.volume = 0.7f;
        shootSource = gameObject.AddComponent<AudioSource>();
        shootSource.volume = 0.8f;
        damageSource = gameObject.AddComponent<AudioSource>();
        damageSource.volume = 0.6f;
        tauntSource = gameObject.AddComponent<AudioSource>();
        tauntSource.volume = 0.4f;
        AudioHighPassFilter filter = gameObject.AddComponent<AudioHighPassFilter>();
        filter.cutoffFrequency = 100;
        filter.highpassResonanceQ = 1;
    }

    // Update is called once per frame
    void Update() {
        if (Random.Range(0, 1000) == 0)
        {
            Taunt();
        }
    }

    private void PlayRandomSound(AudioSource source, AudioClip[] clips)
    {
        if (clips.Length > 0)
        {
            source.clip = clips[Random.Range(0, clips.Length)];
            source.Play();
        }
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
        if (globalTaunt && globalTaunt.isPlaying) return;
        if (IsTalking()) return;
        PlayRandomSound(tauntSource, taunt);
        globalTaunt = tauntSource;
    }
}
