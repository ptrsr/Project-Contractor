using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour {


    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            WinText winText = FindObjectOfType<WinText>();
            winText.GetComponent<Canvas>().enabled = true;
        }
    }
}
