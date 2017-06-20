using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SubRotation : MonoBehaviour {

    [SerializeField]
    private float rotationSpeed = 20;
    [SerializeField]
    private float minMaxRotationX = 60;
    [SerializeField]
    private float minMaxRotationY = 60;
    private float _rotationY = 0;
    private float _rotationX = 0;
    private SubMovement _sub;
	void Start () {
        _sub = GetComponent<SubMovement>();
	}
	

	void Update () {
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
        {
            if (_sub.Frozen) return;
            Vector3 mouse_pos = Input.mousePosition;
            Vector3 object_pos = Camera.main.WorldToScreenPoint(transform.position);
            Vector3 dir = mouse_pos - object_pos;
            dir.x = Mathf.Clamp(dir.x, -minMaxRotationX, minMaxRotationX);
            dir.y = Mathf.Clamp(dir.y, -minMaxRotationY, minMaxRotationY);
            dir = dir.normalized;
            Quaternion newRot = new Quaternion();
            newRot.eulerAngles = (new Vector3(dir.y * rotationSpeed, -dir.x * rotationSpeed, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot, 5 * Time.deltaTime);
        }
        else
        {
            Quaternion newRot2 = new Quaternion();
            newRot2.eulerAngles = (new Vector3(0, 0, 0));
            transform.rotation = Quaternion.Slerp(transform.rotation, newRot2, 5 * Time.deltaTime);
        }
    }


    private void RotateDependingOnDistance(Vector3 vec3)
    {
        Quaternion quat = GetQuaternionFromVector(vec3);
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

