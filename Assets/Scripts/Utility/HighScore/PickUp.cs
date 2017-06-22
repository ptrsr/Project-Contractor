using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PickUp : MonoBehaviour {

    // Use this for initialization
    private Camera _cam;
    private GameObject _rotateAround;
    private bool _rotate;
    private bool _rotateBack;
    private float _startAt = 0;
    private bool _done = false;
    private bool _finished = false;
    private int _finalRotation = 180;
    private int _rotationSpeed = 6;
    private int _counter = 0;
    private Vector3 _subPos;
    private SubMovement _sub;
    public bool Finished { get { return _finished; } }
	void Start () {
        _cam = Camera.main;
        _sub = FindObjectOfType<SubMovement>();
        _subPos = _sub.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        if (_finished) return;
        _subPos = _sub.transform.position;
        if (_rotate)
        {
            RotateToCamera(_rotateAround,gameObject,new Vector3(0,180,0));
        }
        else if (_rotateBack)
        {
            RotateBack(_rotateAround, gameObject, new Vector3(0, 0, 0));
        }
	}

    public void Pick()
    {
       
        //Vector3 spawnPos = (camPos - trinketPos) / 2;
        _rotateAround = GameObject.CreatePrimitive(PrimitiveType.Cube);
        _rotateAround.name = "Cubisimo";
        Vector3 spawnPos = (_subPos - _cam.transform.position).normalized * 25;
        transform.position = _sub.transform.position;
        _rotateAround.transform.position = spawnPos + _cam.transform.position;
        _rotateAround.transform.parent = _cam.transform;
        _rotateAround.transform.LookAt(_cam.transform);
        _rotateAround.GetComponent<BoxCollider>().enabled = false;
        _rotateAround.GetComponent<MeshRenderer>().enabled = false;
        _rotate = true;
        transform.parent = _rotateAround.transform;
    }
    public void RotateBack(GameObject objectToRotateAround, GameObject objectToRotate, Vector3 angles)
    {
        Debug.Log("back");
        if (_counter >= _finalRotation)
        {
            _finished = true;
        }
        else
        {
            _rotateAround.transform.Rotate(new Vector3(0, _rotationSpeed, 0));
            _counter += _rotationSpeed;
            Vector3 scale = transform.localScale;
            Vector3 finalScale = new Vector3(0.1f, 0.1f, 0.1f);
            transform.localScale = Vector3.Lerp(scale, finalScale, 0.05f);
        }
    }
    public void RotateToCamera(GameObject objectToRotateAround, GameObject objectToRotate, Vector3 angles)
    {
        if (_done)
        {
            Quaternion rotation = transform.rotation;
            Quaternion straightRot = new Quaternion();
            straightRot.eulerAngles = new Vector3(0, 0, 0);
            transform.rotation = Quaternion.Slerp(rotation, straightRot, 0.2f);
            if (Delay(2))
            {
                _rotateBack = true;
                _rotate = false;
                _counter = 0;
            }
        }
        else if (_counter >= _finalRotation)
        {
            _startAt = Time.timeSinceLevelLoad;
            _done = true;
        }
        else
        {
            _rotateAround.transform.Rotate(new Vector3(0, _rotationSpeed, 0));
            _counter += _rotationSpeed;
            Quaternion rotation = transform.rotation;
            Quaternion straightRot = new Quaternion();
            straightRot.eulerAngles = new Vector3(0, transform.rotation.eulerAngles.y, 0);
            transform.rotation = Quaternion.Slerp(rotation, straightRot, 0.05f);
        }
    }
    private bool Delay(int seconds)
    {
        if (_startAt + seconds <= Time.timeSinceLevelLoad)
        {
            Debug.Log("delay passed");
            return true;
        }
        else
            return false;
    }
}
