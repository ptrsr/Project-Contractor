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
        RotateDependingOnDistance(new Vector3(_rotationX, -_rotationY , 0));
    }
    private void RotateDependingOnDistance(Vector3 vec3)
    {
        Quaternion quat = GetQuaternionFromVector(vec3);
        //Vector3 pos = GetMousePosition();
        // distance = Vector3.Distance(pos, transform.position);
        transform.rotation = Quaternion.Slerp(transform.rotation, quat, 0.1f);
    }
    private Vector3 GetMousePosition()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = -Camera.main.transform.position.z;
        pos = Camera.main.ScreenToWorldPoint(pos);
        return pos;
    }
    private Quaternion GetQuaternionFromVector(Vector3 vec3)
    {
        Quaternion quat = new Quaternion();
        quat.eulerAngles = vec3;
        return quat;
    }
}

