using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinTrigger : MonoBehaviour {
    
    void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Player")
        {
            Canvas canvas = FindObjectOfType<Canvas>();
            canvas.enabled = true;
            
        }
    }
}
