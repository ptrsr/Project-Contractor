using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SubMovement : MonoBehaviour {

    private Rigidbody _rigidBody;
    private Camera _camera;

	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
        _camera = Camera.main;
	}

	void Update () {
        //Vector3 cursorPosition = _camera.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, _camera.nearClipPlane));
        //Debug.Log(cursorPosition);
        //Vector3 gameObjectPos = _rigidBody.gameObject.transform.position;
        //Vector3 direction = transform.position - cursorPosition;
        if (Input.GetMouseButton(0))
        {
            Vector3 pos = Input.mousePosition;
            pos.z = -Camera.main.transform.position.z;
            pos = Camera.main.ScreenToWorldPoint(pos);
            Vector3 dir = pos - transform.position;
            float distance = Vector3.Distance(pos, transform.position);
            dir = dir.normalized;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.up); // Turns Right
            transform.rotation = Quaternion.AngleAxis(angle, Vector3.down); //Turns Left
            _rigidBody.AddForce(dir * (distance / 10), ForceMode.VelocityChange);
        }
        
    }
}
