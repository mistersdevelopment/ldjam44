using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OrderByY : MonoBehaviour
{
    public float offset = 0;
        
    // Use this for initialization
    void Start()
    {
    }

    // Update is called once per frame
    void LateUpdate()
    {
        Vector3 pos = gameObject.transform.position;
        pos.z = -5 + (pos.y + offset) / 100.0f;
        gameObject.transform.position = pos;
    }
}
