using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour {
    private CharacterSounds sounds;

    // Use this for initialization
    void Start() {
        sounds = gameObject.GetComponent<CharacterSounds>();
        if (!sounds)
        {
            sounds = gameObject.GetComponentInParent<CharacterSounds>();
        }
    }

    // Update is called once per frame
    void Update() {

    }

    public void Footstep()
    {
        if (sounds)
        {
            sounds.Footstep();
        }
    }

    public void Shoot()
    {
        if (sounds)
        {
            sounds.Shoot();
        }
        Elvis elvis = GetComponentInParent<Elvis>();
        if (elvis)
        {
            elvis.Shoot();
        }
    }
}
