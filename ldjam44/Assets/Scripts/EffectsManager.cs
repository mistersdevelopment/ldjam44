using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectsManager : MonoBehaviour
{
    public static EffectsManager Instance;
    public Effect burnEffect;

    void Start()
    {
        Instance = this;
    }
}
