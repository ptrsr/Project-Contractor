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
    [SerializeField]
    private Transform _playerPos;
    [SerializeField]
    private Transform _camPos;

    private Camera _cam;
    private Animator _camAnimator;


    void Start () {

		_animator = GetComponentInChildren<Animator>();
        _sub = FindObjectOfType<SubMovement>();
        _cam = Camera.main;
        _camAnimator = _cam.GetComponent<Animator>();
    }
	
	// Update is called once per frame
	void Update () {
        if (_played)
        {

        }
		if (_key3InPlace) {
			if (!_played) {

                if (!MovePlayer() || !MoveCamera()) return;
                _camAnimator.SetBool("Play1", true);
				_played = true;
                _sub.Freeze(true);
                _startedAt = Time.time;

			}
		}
	}

    private bool MovePlayer()
    {
        _sub.transform.position = Vector3.Lerp(_sub.transform.position, _playerPos.position, 0.03f);
        if (Vector3.Distance(_sub.transform.position, _playerPos.position) < 0.6f)
        {
            return true;
        }
        else { return false; }
    }
    private bool MoveCamera()
    {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camPos.position, 0.03f);
        if (Vector3.Distance(_cam.transform.position, _camPos.position) < 0.6f)
        {
            return true;
        }
        else { return false; }
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
