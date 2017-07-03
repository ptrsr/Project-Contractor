using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAnimation : MonoBehaviour {

    // Use this for initialization
    private Animator _animator;
	void Start () {
        _animator = GetComponentInParent<Animator>();
        _animator.enabled = false;
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Player") {
            Debug.Log("Animator activate");
            _animator.enabled = true;
        }
    }
}
