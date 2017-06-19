using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReefAnimation : MonoBehaviour {

	// Use this for initialization
	private Animator _animator;
    private SubMovement _sub;
	private bool _key3InPlace = false;
	private bool _played = false;
    private float _startedAt = 0;
    [SerializeField]
    private int _lenghtAnimation = 0;
	void Start () {

		_animator = GetComponentInChildren<Animator>();
        _sub = FindObjectOfType<SubMovement>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_played)
        {

        }
		if (_key3InPlace) {
			if (!_played) {
				_animator.SetBool ("Open", true);
				_played = true;
                _sub.Freeze(true);
                _startedAt = Time.time;

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
