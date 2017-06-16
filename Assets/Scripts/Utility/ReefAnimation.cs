using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReefAnimation : MonoBehaviour {

	// Use this for initialization
	private Animator _animator;
	private bool _key3InPlace = false;
	private bool _played = false;
	void Start () {

		_animator = GetComponentInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if (_key3InPlace) {
			if (!_played) {
				_animator.SetBool ("Open", true);
				_played = true;
			}
		}
	}
	public void Key3InPlace()
	{
		_key3InPlace = true;
	}

	private void OnTriggerEnter(Collider other){
		if (other.gameObject.tag == "Player") {
			_animator.SetBool ("Close", true);		
		}
	}
}
