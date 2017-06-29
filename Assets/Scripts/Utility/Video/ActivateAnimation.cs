using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimation : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private Animator _animator;
	void Start () {
        _animator.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) {
            _animator.enabled = true;
        }
    }
}
