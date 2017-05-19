using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullDoorAnimator : MonoBehaviour {

    // Use this for initialization
    private Animator _animator;
    private bool _key1InPlace = false;
    private bool _key2InPlace = false;
	void Start () {
        _animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_key1InPlace && _key2InPlace)
        {
            _animator.Play("RotateEyes");
            
        }
	}
    public void Key1InPlace()
    {
        _key1InPlace = true;
    }
    public void Key2InPlace()
    {
        _key2InPlace = true;
    }
}
