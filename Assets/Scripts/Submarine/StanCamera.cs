using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StanCamera : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private GameObject _target;
    [SerializeField]
    private float smoothTimeX = 0.4f;
    [SerializeField]
    private float smoothTimeY = 0.7f;

    private Transform _targetTransf;
    private Transform _dummyTransform;

    private float yVelocity = 0.0f;
    private float xVelocity = 0.0f;
    void Start () {
        _targetTransf = _target.transform;
        _dummyTransform = GetComponentInChildren<Transform>();
	}
	
	// Update is called once per frame
	void Update () {

        _dummyTransform.LookAt(_targetTransf);
        float someAngle = _dummyTransform.rotation.eulerAngles.y;
        float MinClamp = 5;
        float MaxClamp = -5;
        someAngle = Mathf.Clamp(someAngle, MinClamp, MaxClamp);
        Quaternion quat = new Quaternion();
        quat.eulerAngles = new Vector3(0, someAngle, 0);
        Quaternion newRotation = Quaternion.Slerp(transform.rotation, quat, 0.1f);
        transform.rotation = newRotation;
        float newPositionY = Mathf.SmoothDamp(transform.position.y, _targetTransf.position.y, ref yVelocity, smoothTimeY);
        float newPositionX = Mathf.SmoothDamp(transform.position.x, _targetTransf.position.x, ref xVelocity, smoothTimeX);
        transform.position = new Vector3(newPositionX, newPositionY, transform.position.z);
    }
} 
