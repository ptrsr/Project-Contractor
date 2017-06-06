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
    private float _maxOxygen;
	private void Start () {
        _mainCamera = Camera.main;
        _vignette = FindObjectOfType<vignette>();
        _maxOxygen = _oxygen;
        _sub = FindObjectOfType<SubMovement>();
    }

    private void Update()
    {
    }
    public void Add(int value)
    {
        _oxygen += value;
        ChangeValues();
        
    }
    public void Remove(int value)
    {
        _oxygen -= value;
        ChangeValues();
        Surface();
    }
    private void ChangeValues()
    {
        if(_oxygen > 0)
        {
            _vignette.range = _oxygen / _maxOxygen;
        }
    }
    private void Surface()
    {
        if(_oxygen < 0)
        {
            _sub.Surface();
        }
    }
}
