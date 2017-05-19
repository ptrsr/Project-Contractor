using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkullDoorAnimator : MonoBehaviour {

    // Use this for initialization
    private Animator[] _animator;
    private bool _key1InPlace = false;
    private bool _key2InPlace = false;
    private bool _played = false;
    private int _lenght = 0;
    private float _startedAt = 0;
	void Start () {
        _animator = GetComponentsInChildren<Animator>();
	}
	
	// Update is called once per frame
	void Update () {
		if(_key1InPlace && _key2InPlace)
        {
            if (!_played)
            {
                _animator[1].SetBool("Rotate", true);
                _lenght = _animator[1].GetCurrentAnimatorClipInfo(0).Length;
                _startedAt = Time.time;
                _played = true;
            }
            if ((_startedAt + _lenght) - Time.time < 0.0f)
            {
                _animator[0].SetBool("Open", true);
            }
            
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
