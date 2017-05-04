using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraUtility : MonoBehaviour
{
    public void Update()
    {
        if (Input.GetKey(KeyCode.W))
        {
            transform.position += Vector3.forward;
        }
        if (Input.GetKey(KeyCode.S))
        {
            transform.position += Vector3.back;
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position += Vector3.left;
        }
        if (Input.GetKey(KeyCode.D))
        {
            transform.position += Vector3.right;
        }
        if (Input.GetKey(KeyCode.Q))
        {
            transform.position += Vector3.down;
        }
        if (Input.GetKey(KeyCode.E))
        {
            transform.position += Vector3.up;
        }
    }
}