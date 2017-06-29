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
    private Vector3 _playerPos;
    [SerializeField]
    private Transform _camPos;

    private Camera _cam;
    private Animator _camAnimator;
    private bool _finished = false;


    void Start () {

		_animator = GetComponentInChildren<Animator>();
        _sub = FindObjectOfType<SubMovement>();
        _cam = Camera.main;
        _camAnimator = _cam.GetComponent<Animator>();
        _camAnimator.enabled = false;
        _playerPos = new Vector3(_camPos.position.x, _camPos.position.y, 0);
    }
	
	// Update is called once per frame
	void FixedUpdate () {
        if (_finished) return;
        if (_played)
        {
            if (_startedAt + _lenghtAnimation <= Time.timeSinceLevelLoad)
            {
                _sub.Freeze(false);
                _cam.GetComponent<camMove>().FollowAnimation(false);
                _camAnimator.enabled = false;
                _finished = true;
            }
        }
		if (_key3InPlace) {
			if (!_played)
            {
                _sub.Freeze(true);
                if (!MovePlayer()) { }
                if (!MoveCamera()) { return; }
                _camAnimator.enabled = true;
                _animator.SetBool("Open", true);
                _played = true;
                _startedAt = Time.timeSinceLevelLoad;

			}
		}
	}

    private bool MovePlayer()
    {
        _sub.transform.position = Vector3.Lerp(_sub.transform.position, _playerPos, 0.1f);
        if (Vector3.Distance(_sub.transform.position, _playerPos) < 1f)
        {
            return true;
        }
        else { return false; }
    }
    private bool MoveCamera()
    {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camPos.position, 0.1f);
        Quaternion newRot = new Quaternion();
        newRot.eulerAngles = (new Vector3(0,0,0));
        transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 5 * Time.deltaTime);
        if (Vector3.Distance(_cam.transform.position, _camPos.position) < 1f)
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
