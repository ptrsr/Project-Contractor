using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkTrigger : MonoBehaviour
{
    [SerializeField]
    private GameObject _object;

    public void OnTriggerEnter(Collider c)
    {
        if (c.GetComponent<SubMovement>())
        {
            _object.SetActive(true);
        }
    }
}