using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;



public class TimeManager : MonoBehaviour {

    private int _delayBeforeExit = 30;
    private float _lastInput = 0;

    private int _timeFromMuseum = 360;

    public int TimeFromMuseum { set { _timeFromMuseum = value; } }

    private OxygenCrack[] _oxygenCracks;

    private bool _disabledOxygen = false;

    public bool DisabledOxygen { get { return _disabledOxygen; } }

	void Start () {
        _oxygenCracks = FindObjectsOfType<OxygenCrack>();
	}
	
	// Update is called once per frame
	void Update () {
        CheckInput();
        if ((_lastInput + _delayBeforeExit) - Time.timeSinceLevelLoad <= 0)
        {
            EditorApplication.isPlaying = false;
            //Application.Quit();
        }
        if((_timeFromMuseum - 15) - Time.timeSinceLevelLoad <= 0)
        {
            _disabledOxygen = true;
            DisableOxygenCracks();
        }


	}


    private void DisableOxygenCracks()
    {
        foreach(OxygenCrack oc in _oxygenCracks)
        {
            oc.DisableCreation();
        }
    }
    

    private void CheckInput()
    {
        if(Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            _lastInput = Time.timeSinceLevelLoad;
        }
        else if(Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            _lastInput = Time.timeSinceLevelLoad;
        }
    }
}
