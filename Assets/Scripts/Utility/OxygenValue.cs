using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenValue : MonoBehaviour {

    [SerializeField]
    private int _value;

    private Oxygen _oxygen;

    private void Start()
    {
        _oxygen = FindObjectOfType<Oxygen>();
    }

    public int OxygenVal()
    {
        return _value;
    }

}
