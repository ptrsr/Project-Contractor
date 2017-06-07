using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oxygen : MonoBehaviour
{
    [SerializeField]
    private float 
        _amount,
        _size;

    public float Amount
    {
        get { return _amount; }
    }
}
