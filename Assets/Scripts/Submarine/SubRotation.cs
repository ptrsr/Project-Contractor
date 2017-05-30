using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubRotation : MonoBehaviour {

    [SerializeField]
    private float rotationSpeedY = 5;
    [SerializeField]
    private float rotationSpeedX = 5;
    [SerializeField]
    private float minMaxRotationX = 60;
    [SerializeField]
    private float minMaxRotationY = 60;
    private Rigidbody _rigidbody;
    private float _rotationY = 0;
    private float _rotationX = 0;
	void Start () {
        _rigidbody = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
        _rotationY += Input.GetAxis("Mouse X") * rotationSpeedY;
        _rotationY = Mathf.Clamp(_rotationY, -minMaxRotationX, minMaxRotationX);
        _rotationX += Input.GetAxis("Mouse Y") * rotationSpeedX;
        _rotationX = Mathf.Clamp(_rotationX, -minMaxRotationY, minMaxRotationY);
        transform.localEulerAngles = new Vector3(transform.localEulerAngles.x, -_rotationY , transform.localEulerAngles.z);
        transform.localEulerAngles = new Vector3(_rotationX, transform.localEulerAngles.y, transform.localEulerAngles.z);
    }
}
