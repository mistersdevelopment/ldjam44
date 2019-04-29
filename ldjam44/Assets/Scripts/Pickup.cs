using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pickup : MonoBehaviour
{
	protected bool allowPickup = true;
    public AudioClip[] pickupSfxs = new AudioClip[0];

    void OnCollisionEnter2D(Collision2D collision)
	{
		if (allowPickup)
		{
			GameObject other = collision.gameObject;
			Player playerScript = other.GetComponent<Player>();
			if (playerScript)
			{
				if (ApplyPowerup(playerScript))
                {
                    PlayRandomSound(playerScript.GetComponent<AudioSource>(), pickupSfxs);
                    Destroy(this.gameObject);
				}
			}
		}
	}

	public virtual bool ApplyPowerup(Player player)
	{
        return true;
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
}
