using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Singleton;

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
    private bool _frozen = false;

    public bool hasCharged { get { return _charged; } }


    //---------------------------------Charging data--------------------------------------------
    private float _lastTap;
    [SerializeField]
    private float _tapIntervalsForCharge = 0.6f;
    [SerializeField]
    private int _cooldown = 90;
    [SerializeField]
    private int _stunSlowTime = 60;
    [SerializeField]
    private int _oxygenLossOnHit = 10;
    private int _counter = 0;
    private int _stunSlowCounter = 0;

    private ParticleSystem _particles;
    private HighScoreManager _manager;

    private int _lossOfOxygen = 1;

    public int LossOfOxygen { set { _lossOfOxygen = value; } }


    void Awake () {
        _rigidBody = GetComponent<Rigidbody>();
        _oxygen = FindObjectOfType<Oxygen>();
        _camera = Camera.main;
        _startPosition = transform.position;
        _particles = GetComponentInChildren<ParticleSystem>();
        _manager = FindObjectOfType<HighScoreManager>();
        //TutorialImage tutorial = FindObjectOfType<TutorialImage>();
        //if (tutorial != null) tutorial.SetChaseTarget(this.transform);
        _lastTap = 0;
       
    }

	void FixedUpdate ()
    {
        if (Input.GetKeyDown(KeyCode.R))
        {
            SceneManager.LoadScene(0);
            Volumes.Reset();
            Pings.Reset();
            DarkZones.Reset();
        }
        if (_frozen) return;
        _oxygen.Remove(_lossOfOxygen);
        //keeps the player on the correct plane
        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        //return only if stunned
        if(_stunned)
        {
            _stunSlowCounter = StunSlowDelay(_stunSlowTime, _stunSlowCounter);
            return;
        }
        //Gets correct direction of mouse and rotates depending on that
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
        if (Input.GetMouseButtonDown(0) || Input.GetMouseButtonDown(1))
        {
            float clickTime = Time.timeSinceLevelLoad - _lastTap;

            if (clickTime > 0.08f && clickTime < _tapIntervalsForCharge)
            { 
                _charged = true;
            }
            _lastTap = Time.timeSinceLevelLoad;
        }
        //Movement through dragging
        if (Input.GetMouseButton(0) || Input.GetMouseButton(1))
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
           OxygenCrack value = other.gameObject.GetComponent<OxygenCrack>();
            _oxygen.Refill();
            value.PickUp();

        }
        if(other.gameObject.tag == "SpawnPoint")
        {
            Debug.Log("new Spawn point");
            Vector3 newPos = new Vector3(other.gameObject.transform.position.x, other.gameObject.transform.position.y + 20, 0);
            _startPosition = newPos;
        }
        if(other.gameObject.tag == "End")
        {
            if(_manager != null)
            {
                _manager.StartEndPlacement(this);
            }
        }
       
    }
    private void OnCollisionEnter(Collision other)
    {
        if (other.gameObject.tag == "Wall")
        {
            _oxygen.Remove(_oxygenLossOnHit);
            _particles.Play();
        }
    }

    public void PlayParticles()
    {
        _particles.Play();
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
    public void StunPlayer()
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
            _stunned = false;
        }
        else
        {
            counter++;
        }
        return counter;
    }

    public void Freeze(bool value)
    {
        _frozen = value;
    }


    public bool Charged { get { return _charged;  } }
    public bool Frozen { get { return _frozen; } }

    public int DragSpeed { get { return _dragSpeed; } set { _dragSpeed = value; } }
    public int MaxSpeed { get { return _maxSpeed; } set { _maxSpeed = value; } }
    public int DistaceForMax { get { return _distanceForMaxSpeed; } set { _distanceForMaxSpeed = value; } }
    public int ChargeSpeed { get { return _chargeSpeed; } set { _chargeSpeed = value; } }
    public float TapChargeIntrvl { get { return _tapIntervalsForCharge; } set { _tapIntervalsForCharge = value; } }
    public int ChargeCooldwn { get { return _cooldown; } set { _cooldown = value; } }
    public int StunSlowCooldown { get { return _stunSlowTime; } set { _stunSlowTime = value; } }

}
