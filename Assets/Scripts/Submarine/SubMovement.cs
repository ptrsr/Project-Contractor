using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SubMovement : MonoBehaviour {

    private Rigidbody _rigidBody;
    private Oxygen _oxygen;
    private Camera _camera;
    private Vector3 _startPosition;

    [SerializeField]
    private int dragSpeed = 10;
    [SerializeField]
    private int chanrgeSpeed = 50;

    private bool _tapping = false;
    private bool _charged = false;
    private bool _slowed = false;
    private float _lastTap;
    [SerializeField]
    private float _tapIntervalForCharge = 1;
    [SerializeField]
    private float _cooldown = 90;
    private float _counter = 0;


	void Start () {
        _rigidBody = GetComponent<Rigidbody>();
        _oxygen = FindObjectOfType<Oxygen>();
        _camera = Camera.main;
        _startPosition = transform.position;
	}

	void FixedUpdate () {
        _oxygen.Remove(1);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //Cooldown after you charge(double tap)
        if (_charged)
        {
            if (_counter >= _cooldown)
            {
                _charged = false;
                _counter = 0;
            }
            else { _counter++; }
            return;
        }
        //Movement through dragging
        if (Input.GetMouseButton(0))
        {

            Vector3 pos = GetMousePosition();
            Vector3 dir = pos - transform.position;
            float distance = Vector3.Distance(pos, transform.position);
            dir = dir.normalized;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            if (Quaternion.AngleAxis(angle, Vector3.up).eulerAngles.y > 180)
            {
                transform.rotation = GetQuaternionFromVector(new Vector3(0,90,0));
            }
            if (Quaternion.AngleAxis(angle, Vector3.up).eulerAngles.y < 180)
            {
                transform.rotation = GetQuaternionFromVector(new Vector3(0, -90, 0));
            }
            //adding force based on direction and distance from mouse
            float speed = distance / dragSpeed;
            if (_slowed)
            {
                speed /= 2;
            }
            _rigidBody.AddForce(dir * speed, ForceMode.VelocityChange);
        }
        //Check for double tapping for charge
        if (Input.GetMouseButtonDown(0))
        {
            //if no taps count one and do coroutine
            if (!_tapping)
            {
                _tapping = true;
                SingleTap();
            }
            //if you tap a second time before _taptime(interval time for second taps) charge
            if ((Time.time - _lastTap) < _tapIntervalForCharge)
            {

                Debug.Log("DoubleTap");
                Vector3 pos = Input.mousePosition;
                pos.z = -Camera.main.transform.position.z;
                pos = Camera.main.ScreenToWorldPoint(pos);
                Vector3 dir = pos - transform.position;

                float distance = Vector3.Distance(pos, transform.position);
                
                _rigidBody.AddForce(dir.normalized * chanrgeSpeed, ForceMode.Impulse);
                _tapping = false;
                _charged = true;

            }
            _lastTap = Time.time;
        }
    }

    //Get mouse position in world space
    private Vector3 GetMousePosition()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = -Camera.main.transform.position.z;
        pos = Camera.main.ScreenToWorldPoint(pos);
        return pos;
    }

    //Get a quaternion based on vec3 provided
    private Quaternion GetQuaternionFromVector(Vector3 vec3)
    {
        Quaternion quat = new Quaternion();
        quat.eulerAngles = vec3;
        return quat;
    }

    //Coroutine for waiting after first tap
    IEnumerator SingleTap()
    {
        yield return new WaitForSeconds(_tapIntervalForCharge);
        if (_tapping)
        {
            Debug.Log("SingleTap");
            _tapping = false;
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Oxygen")
        {
           OxygenValue value = other.gameObject.GetComponent<OxygenValue>();
            _oxygen.Add(value.OxygenVal());
            other.gameObject.SetActive(false);
        }
    }

    //Using a position provided at start to move the submarine to surface
    public void Surface()
    {
        transform.position = _startPosition;
    }

    //For octupus or whatever is gonna stick to the player and slow him down
    public void SlowDownPlayer(bool value)
    {
        _slowed = value;
    }
}
