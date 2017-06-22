using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    float i = -1;
    float add = 0.1f;
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        i += add;
        if (i >= 1 || i <= -1)
            add = -add;
        Rigidbody body = GetComponent<Rigidbody>();
        body.AddTorque(0, i, 0, ForceMode.VelocityChange);
    }
}