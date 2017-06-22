using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour {

    private SubMovement _sub;
    private vignette _vignette;
    private Camera _mainCamera;
    private TimeManager _timeManager;
    private HighScoreManager _highScoreManager;
    [SerializeField]
    private float _oxygen = 10000;
	[SerializeField]
    private float _maxOxygen;
    private float _newValueRemove = 0;
    private bool _changeAdd = false;
    private bool _changeRemove = false;
    private float _counter = 0;
    private float _delay = 0;
    private float _smoothness = 0.05f;
    private float _currentOxygen = 0;

    private bool _done = false;

 
	private void Start () {
        _mainCamera = Camera.main;
        _vignette = FindObjectOfType<vignette>();
        _sub = FindObjectOfType<SubMovement>();
        _timeManager = FindObjectOfType<TimeManager>();
        _highScoreManager = FindObjectOfType<HighScoreManager>();
        
        _delay = (1 / _smoothness) + 50;
    }

    private void Update()
    {
        if (_done) return;
        if (_changeAdd)
        {
            _oxygen = Mathf.Lerp(_oxygen, _maxOxygen, _smoothness);
            ChangeValues();
            if(_counter >= _delay)
            {
                _counter = 0;
                _changeAdd = false;
            }
            else { _counter++; }
        }
        else if (_changeRemove)
        {
            ChangeValues();
        }
    }
    public void Refill()
    {
        _changeAdd = true;
        
    }
    public void Remove(float value)
    {
        _oxygen -= value;
        _changeRemove = true;
    }
    private void ChangeValues()
    {
        if (_oxygen > 0)
        {
            _vignette.range = _oxygen / _maxOxygen;
        }
        else if (_oxygen < 0)
        {
            if (_timeManager.DisabledOxygen)
            {
                _highScoreManager.ShowEndHUD();
                _done = true;
            }
            else {
                _sub.Surface();
                Refill();
            }
        }
    }
}
