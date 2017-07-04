using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAnimator : MonoBehaviour {

    [SerializeField]
    private RuntimeAnimatorController _camController;
    [SerializeField]
    private Transform _camPos;
    [SerializeField]
    private Animator _animator1;
    [SerializeField]
    private int _lenghtOfAnimation = 0;

    private Camera _cam;
    private camMove _camMove;
    private Animator _camAnimator;

    private Vector3 _playerPos;

    private SubMovement _subMov;

    private bool _update = false;
    private bool _cameraAnimatorActive = false;
    private bool _animatorActive = false;
    private bool _finished = false;

    private float _startedAt = 0;
    private float _startedAt2 = 0;
	// Use this for initialization
	void Start () {
        _cam = Camera.main;
        _camMove = _cam.GetComponent<camMove>();
        _camAnimator = _cam.GetComponent<Animator>();
        _subMov = FindObjectOfType<SubMovement>();
        _playerPos = new Vector3(_camPos.position.x, _camPos.position.y, 0);
	}
	
	// Update is called once per frame
	void Update () {
        if (_finished) return;
        if (_cameraAnimatorActive)
        {
            MovePlayer();
            if (_startedAt + _lenghtOfAnimation <= Time.timeSinceLevelLoad)
            {
                if (MovePlayer())
                {
                    _subMov.Freeze(false);
                }
                _camAnimator.enabled = false;
                _camMove.FollowAnimation(false);
                _animator1.enabled = false;
                _finished = true;
            }
            return;
        }
        else if (_animatorActive)
        {
            if (_startedAt2 + _lenghtOfAnimation <= Time.timeSinceLevelLoad)
            {
                _subMov.Freeze(false);
                _finished = true;
                _animator1.enabled = false;
            }
            return;
        }
        else if (_update)
        {
            if(_camAnimator != null)
            {
                _camMove.FollowAnimation(true);
                _subMov.Freeze(true);
                if (MoveCamera())
                {
                    _camAnimator.runtimeAnimatorController = _camController;
                    _camAnimator.enabled = true;
                    _animator1.enabled = true;
                    _cameraAnimatorActive = true;
                    _startedAt = Time.timeSinceLevelLoad;
                }
            }
            else
            {
                _subMov.Freeze(true);
                _animator1.enabled = true;
                _startedAt2 = Time.timeSinceLevelLoad;
                _animatorActive = true;
            }
        }
		
	}
    private bool MovePlayer()
    {
        _subMov.transform.position = Vector3.Lerp(_subMov.transform.position, _playerPos, Time.deltaTime * 2);
        if (Vector3.Distance(_subMov.transform.position, _playerPos) < 1)
        {
            return true;
        }
        else { return false; }
    }

    private bool MoveCamera()
    {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camPos.position, Time.deltaTime * 2);
        if (Vector3.Distance(_cam.transform.position, _camPos.position) < 1)
        {
            return true;
        }
        else { return false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _update = true;
        }
    }
}
