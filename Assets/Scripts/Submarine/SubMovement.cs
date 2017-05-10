using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SubMovement : MonoBehaviour {

    private Rigidbody _rigidBody;
    private Camera _camera;

    [SerializeField]
    private Transform light;

    private bool _tapping;
    private float _lastTap;
    private float _tapTime = 1;
    private bool _charged = false;
    private float _cooldown = 30;
    private float _counter = 0;

	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
        _camera = Camera.main;
	}

	void Update ()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = -Camera.main.transform.position.z;
        pos = Camera.main.ScreenToWorldPoint(pos);
        Vector3 dir = pos - transform.position;
        dir = dir.normalized;

        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        light.eulerAngles = new Vector3(angle - 90, 90, 0);

        if (Input.GetMouseButton(0))
        {
            if (Quaternion.AngleAxis(angle, Vector3.up).eulerAngles.y > 180)
            {
                Vector3 right = new Vector3(0, 90, 0);
                Quaternion quat = new Quaternion();
                quat.eulerAngles = right;
                transform.rotation = quat;
            }
            if (Quaternion.AngleAxis(angle, Vector3.up).eulerAngles.y < 180)
            {

                Vector3 right = new Vector3(0, -90, 0);
                Quaternion quat = new Quaternion();
                quat.eulerAngles = right;
                transform.rotation = quat;
            }
            float distance = Vector3.Distance(pos, transform.position);
            _rigidBody.AddForce(dir * (distance / 10), ForceMode.VelocityChange);
        }

        if (_charged){
            if (_counter >= _cooldown){
                _charged = false;
                _counter = 0;
            }
            else{ _counter++; }
            return;
        }
        if (Input.GetMouseButtonDown(0))
        {
            if (!_tapping)
            {
                _tapping = true;
                SingleTap();
            }
            if ((Time.time - _lastTap) < _tapTime)
            {
                Debug.Log("DoubleTap");
                pos = Input.mousePosition;
                pos.z = -Camera.main.transform.position.z;
                pos = Camera.main.ScreenToWorldPoint(pos);
                dir = pos - transform.position;
                float distance = Vector3.Distance(pos, transform.position);
                _rigidBody.AddForce(dir * (distance / 15), ForceMode.Impulse);
                _tapping = false;
                _charged = true;

            }
            _lastTap = Time.time;
        }
    }
    IEnumerator SingleTap()
    {
        yield return new WaitForSeconds(_tapTime);
        if (_tapping)
        {
            Debug.Log("SingleTap");
            _tapping = false;
        }
    }

}
