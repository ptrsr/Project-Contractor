﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZBound : MonoBehaviour
{
    public bool tap = false;

    public void Update()
    {
        if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += Vector3.up * 0.1f;
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position += Vector3.down * 0.1f;
        }
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position += Vector3.left * 0.1f;
        }
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += Vector3.right * 0.1f;
        }
        if (Input.GetKey(KeyCode.Space))
        {
            tap = true;
        }

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }
}