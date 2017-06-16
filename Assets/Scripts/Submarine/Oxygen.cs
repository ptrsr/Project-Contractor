using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Oxygen : MonoBehaviour {

    private SubMovement _sub;
    private vignette _vignette;
    private Camera _mainCamera;
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

 
	private void Start () {
        _mainCamera = Camera.main;
        _vignette = FindObjectOfType<vignette>();
        _sub = FindObjectOfType<SubMovement>();
        _delay = (1 / _smoothness) + 50;
    }

    private void Update()
    {
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
            _oxygen -= _newValueRemove;
            ChangeValues();
        }
    }
    public void Refill()
    {
        _changeAdd = true;
        
    }
    public void Remove(float value)
    {
        _newValueRemove = value;
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
            _sub.Surface();
        }
    }
}
