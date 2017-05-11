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
    private int chargeSpeed = 50;


    //----------------------------------Checks for submarine states----------------------------
    private bool _tapping = false;
    private bool _charged = false;
    private bool _slowed = false;
    private bool _goingLeft = false;
    private bool _goingRight = false;
    private bool _goingDownLeft = false;
    private bool _goingDownRight = false;
    private bool _goingUpLeft = false;
    private bool _goingUpRight = false;


    //---------------------------------Charging date--------------------------------------------
    private float _lastTap;
    [SerializeField]
    private float _tapIntervalsForCharge = 1;
    [SerializeField]
    private float _cooldown = 90;
    private float _counter = 0;


    //------------------------------Rotation of sumbarine---------------------------------------
    private Quaternion left = new Quaternion();
    private Quaternion right = new Quaternion();
    private Quaternion forward = new Quaternion();
    private Quaternion leftUp = new Quaternion();
    private Quaternion leftDown = new Quaternion();
    private Quaternion rightUp = new Quaternion();
    private Quaternion rightDown = new Quaternion();

    [SerializeField]
    private Vector3 _possibleLeftTurn = new Vector3(0, 25, 0);
    [SerializeField]
    private Vector3 _possibleLeftDownTurn = new Vector3(-50, 50, 0);
    [SerializeField]
    private Vector3 _possibleLeftUpTurn = new Vector3(50, 50, 0);
    [SerializeField]
    private Vector3 _possibleRightTurn = new Vector3(0, -25, 0);
    [SerializeField]
    private Vector3 _possibleRightDownTurn = new Vector3(-50, -50, 0);
    [SerializeField]
    private Vector3 _possibleRightUpTurn = new Vector3(50, -50, 0);
    [SerializeField]
    private float _smoothnessOfTurning = 0.1f;


    void Awake () {
        _rigidBody = GetComponent<Rigidbody>();
        //_oxygen = FindObjectOfType<Oxygen>();
        _camera = Camera.main;
        _startPosition = transform.position;
        TutorialImage tutorial = FindObjectOfType<TutorialImage>();
        if (tutorial != null) tutorial.SetChaseTarget(this.transform);
        left = GetQuaternionFromVector(_possibleLeftTurn);
        right = GetQuaternionFromVector(_possibleRightTurn);
        leftDown = GetQuaternionFromVector(_possibleLeftDownTurn);
        leftUp = GetQuaternionFromVector(_possibleLeftUpTurn);
        rightDown = GetQuaternionFromVector(_possibleRightDownTurn);
        rightUp = GetQuaternionFromVector(_possibleRightUpTurn);
        forward = GetQuaternionFromVector(new Vector3(0, 0, 0));
    }

	void FixedUpdate () {
        // _oxygen.Remove(1);
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        GetCorrectDirection();
        RotateDependingOnDirection();
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
            float distance = Vector3.Distance(pos, transform.position);
            Vector3 dir = pos - transform.position;
            dir = dir.normalized;
            Debug.Log(dir);
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;
           
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
            if ((Time.time - _lastTap) < _tapIntervalsForCharge)
            {
                Vector3 pos = GetMousePosition();
                float distance = Vector3.Distance(pos, transform.position);
                Debug.Log("DoubleTap");
                pos.z = -Camera.main.transform.position.z;
                pos = Camera.main.ScreenToWorldPoint(pos);
                Vector3 dir = pos - transform.position;

                _rigidBody.AddForce(dir.normalized * chargeSpeed, ForceMode.Impulse);
                _tapping = false;
                _charged = true;

            }
            _lastTap = Time.time;
        }
    }

    private void GetCorrectDirection() {
        _goingLeft = false;
        _goingRight = false;
        _goingUpLeft = false;
        _goingUpRight = false;
        _goingDownRight = false;
        _goingDownLeft = false;
        Vector3 pos = GetMousePosition();
        float distance = Vector3.Distance(pos, transform.position);
        Vector3 dir = pos - transform.position;
        dir = dir.normalized;
        if (dir.x >= 0.5f && dir.y < 0.5f)
        {
            _goingRight = true;
        }
        else if (dir.x <= -0.5f && dir.y <= 0.5f  )
        {
            _goingLeft = true;
        }
        else if (dir.x <= -0.3f && dir.y <= -0.1f)
        {
            _goingDownLeft = true;
        }
        else if (dir.x <= -0.3f && dir.y >= 0.3f)
        {
            _goingUpLeft = true;
        }
        else if (dir.x <= 0.3f && dir.y <= -0.1f)
        {
            _goingDownRight = true;
        }
        else if (dir.x <= 0.3f && dir.y >= 0.3f)
        {
            _goingUpRight = true;
        }


        if (!Input.GetMouseButton(0))
        {
            _goingLeft = false;
            _goingRight = false;
            transform.rotation = Quaternion.Slerp(transform.rotation, forward, _smoothnessOfTurning);
        }
    }

    private void RotateDependingOnDirection()
    {
        if (_goingLeft)
        {
            RotateDependingOnDistance(left);
        }
        else if (_goingDownLeft)
        {
            RotateDependingOnDistance(leftDown);
        }
        else if (_goingUpLeft)
        {
            RotateDependingOnDistance(leftUp);
        }
        else if (_goingRight)
        {
            RotateDependingOnDistance(right);
        }
        else if (_goingUpRight)
        {
            RotateDependingOnDistance(rightUp);
        }
        else if (_goingDownRight)
        {
            RotateDependingOnDistance(rightDown);
        }
    }

    private void RotateDependingOnDistance(Quaternion quat)
    {
        Vector3 pos = GetMousePosition();
        float distance = Vector3.Distance(pos, transform.position);
        if (distance < 1)
        {
            Quaternion tempQuat = new Quaternion();
            tempQuat.eulerAngles = quat.eulerAngles * -(distance / 10);
            transform.rotation = Quaternion.Slerp(transform.rotation, tempQuat, _smoothnessOfTurning);
        }
        else
            transform.rotation = Quaternion.Slerp(transform.rotation, quat, _smoothnessOfTurning);
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
        yield return new WaitForSeconds(_tapIntervalsForCharge);
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
