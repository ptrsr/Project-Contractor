using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class SubMovement : MonoBehaviour {

    private Rigidbody _rigidBody;
    private Oxygen _oxygen;
    private Camera _camera;
    private Vector3 _startPosition;


    //-----------------------------------Movement speed variables------------------------------
    [SerializeField]
    private int _dragSpeed = 10;
    [SerializeField]
    private int _maxSpeed = 4;
    [SerializeField]
    private int _distanceForMaxSpeed = 20;
    [SerializeField]
    private int _chargeSpeed = 50;


    //----------------------------------Checks for submarine states----------------------------
    private bool _tapping = false;
    private bool _charged = false;
    private bool _slowed = false;
    private bool _stunned = false;
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
    private int _cooldown = 90;
    [SerializeField]
    private int _stunSlowTime = 60;
    private int _counter = 0;
    private int _stunSlowCounter = 0;


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
        _oxygen = FindObjectOfType<Oxygen>();
        _camera = Camera.main;
        _startPosition = transform.position;
        //TutorialImage tutorial = FindObjectOfType<TutorialImage>();
        //if (tutorial != null) tutorial.SetChaseTarget(this.transform);
        left = GetQuaternionFromVector(_possibleLeftTurn);
        right = GetQuaternionFromVector(_possibleRightTurn);
        leftDown = GetQuaternionFromVector(_possibleLeftDownTurn);
        leftUp = GetQuaternionFromVector(_possibleLeftUpTurn);
        rightDown = GetQuaternionFromVector(_possibleRightDownTurn);
        rightUp = GetQuaternionFromVector(_possibleRightUpTurn);
        forward = GetQuaternionFromVector(new Vector3(0, 0, 0));
        _lastTap = 0;
    }

	void FixedUpdate () {
        _oxygen.Remove(1);
        //keeps the player on the correct plane
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //return only if stunned only count when slowed 
        if(_stunned)
        {
            _stunSlowCounter = StunSlowDelay(_stunSlowTime, _stunSlowCounter);
            return;
        }
        else if (_slowed)
        {
            _stunSlowCounter = StunSlowDelay(_stunSlowTime, _stunSlowCounter);
        }
        //Gets correct direction of mouse and rotates depending on that
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
            else
            {
                if (_counter < _cooldown / 5)
                {
                    Vector3 pos = GetMousePosition();
                    Vector3 dir = pos - transform.position;
                    _rigidBody.AddForce(dir.normalized * _chargeSpeed, ForceMode.VelocityChange);
                }
                _counter++; }
            return;
        }
        //check for double taps
        if (Input.GetMouseButtonDown(0))
        {
            float clickTime = Time.time - _lastTap;

            if (clickTime > 0.05f && clickTime < _tapIntervalsForCharge)
            {
                _charged = true;
            }
            _lastTap = Time.time;
        }
        //Movement through dragging
        if (Input.GetMouseButton(0))
        {

            Vector3 pos = GetMousePosition();
            float distance = Vector3.Distance(pos, transform.position);
            Vector3 dir = pos - transform.position;
            dir = dir.normalized;
            float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

            //adding force based on direction and distance from mouse
            float speed = 0;
            if (distance > _distanceForMaxSpeed)
            {
                speed = _maxSpeed;
            }
            else { speed = distance / _dragSpeed; }
            if (_slowed)
            {
                speed /= 2;
            }
            _rigidBody.AddForce(dir * speed, ForceMode.VelocityChange);
        }
        
    }


    //Gets correct direction of mouse
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

    //Rotates depending on the direction of the mouse
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

    //Rotates depending on the distance from the mouse(from small rotation to full) for smoothness
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
        _stunSlowCounter = 0;
    }

    //For eel or whatever is going to stun the player
    public void StunPlayer(bool value)
    {
        _stunned = true;
        _stunSlowCounter = 0;
    }

    //Delay that takes a counter and to count with and later returns(more convenient) 
    //After delay player is unslowed and unstunned
    private int StunSlowDelay(int frames, int counter = 0)
    {
        if(counter >= frames)
        {
            StunPlayer(false);
            SlowDownPlayer(false);
        }
        else
        {
            counter++;
        }
        return counter;
    }


    public int DragSpeed { get { return _dragSpeed; } set { _dragSpeed = value; } }
    public int MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
    public int DistaceForMax { get { return _distanceForMaxSpeed; } set { _distanceForMaxSpeed = value; } }
    public int ChargeSpeed { get { return _chargeSpeed; } set { _chargeSpeed = value; } }
    public float TapChargeIntrvl { get { return _tapIntervalsForCharge; } set { _tapIntervalsForCharge = value; } }
    public int ChargeCooldwn { get { return _cooldown; } set { _cooldown = value; } }
    public int StunSlowCooldown { get { return _stunSlowTime; } set { _stunSlowTime = value; } }

}
