using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullDoorAnimator : MonoBehaviour {

    // Use this for initialization
    private Animator[] _animator;
	private SubMovement _subMov;
    private bool _key1InPlace = false;
    private bool _key2InPlace = false;
    private bool _played = false;
	private bool _opened = false;
    private bool _finished = false;
    [SerializeField]
    private int _lenghtRotateEyes = 0;
    [SerializeField]
    private int _lenghtLockedDoor = 0;
    [SerializeField]
    private Transform _playerPos;
    [SerializeField]
    private Transform _camPos;
    [SerializeField]
    private RuntimeAnimatorController _controller;

    private float _startedAt = 0;
    private float _startedAt2 = 0;
    private int _counter = 0;
    private int _delay = 460;

    private int _finalPosition = 0;

    private Camera _cam;
    private Animator _camAnimator;
    private camMove _camMove;




    private AdditiveSceneManager _sceneManager;

	void Start () {
        _animator = GetComponentsInChildren<Animator>();
		_subMov = FindObjectOfType<SubMovement>();
        _sceneManager = FindObjectOfType<AdditiveSceneManager>();
        if(_playerPos == null)
        {
            _playerPos = _subMov.transform;
        }
        _cam = Camera.main;
        _camAnimator = _cam.GetComponent<Animator>();
        _camMove = _cam.GetComponent<camMove>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (_finished) return;
        if (_opened)
        {
            if (_startedAt2 + _lenghtLockedDoor <= Time.time)
            {
                _subMov.Freeze(false);
                _counter = 0;
                _finished = true;
                _camAnimator.enabled = false;
                _camMove.FollowAnimation(false);
            }
            return;
        }
            
        if (_key1InPlace && _key2InPlace)
        {
            if (!_played)
            {
                _subMov.Freeze(true);
                _camMove.FollowAnimation(true);
                if (!MovePlayer()) { }
                if (!MoveCamera()) return;
                _camAnimator.runtimeAnimatorController = _controller;
                _camAnimator.enabled = true;
                _animator[1].SetBool("Rotate", true);
                _startedAt = Time.time;
                _played = true;
            }
            if ((_startedAt + _lenghtRotateEyes) - Time.time < 0.0f)
            {
                _opened = true;
                _animator[0].SetBool("Open", true);
                _startedAt2 = Time.time;
            }
            if (Time.time < 150)
            {
                _sceneManager.LoadScene(2);
            }
            else if (Time.time < 200 && Time.time > 150)
            {
                _sceneManager.LoadScene(1);
            }
            else
            {
                _sceneManager.LoadScene(0);
            }

        }
        
	}

    private bool MovePlayer()
    {
        _subMov.transform.position = Vector3.Lerp(_subMov.transform.position, _playerPos.position, 0.05f);
        if (Vector3.Distance(_subMov.transform.position, _playerPos.position) < 1)
        {
            return true;
        }
        else { return false; }
    }
    private bool MoveCamera()
    {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camPos.position, 0.05f);
        if (Vector3.Distance(_cam.transform.position, _camPos.position) < 1)
        {
            return true;
        }
        else { return false; }
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