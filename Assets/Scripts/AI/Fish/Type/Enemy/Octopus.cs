using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : FishEnemy
{
    [Header("Octopus Variables")]
    [SerializeField]
    private int _attackCooldown = 500;
    public int AttackCooldown { get { return _attackCooldown; } }

    private int _attackCounter;
    public int AttackCounter { get { return _attackCounter; } set { _attackCounter = value; } }

    [SerializeField]
    [Tooltip("Used to decrease idle intervals between bursts when chasing the player")]
    private float _idleIntervalChange = 2f;
    public float IdleIntervalChange { get { return _idleIntervalChange; } }

    [SerializeField]
    private float _latchOnRange = 10f;
    public float LatchOnRange { get { return _latchOnRange; } }

    [SerializeField]
    private float _latchOnOffset = 5f;
    public float LatchOnOffset { get { return _latchOnOffset; } }

    [SerializeField]
    private int _latchOffDelay = 25;
    public int LatchOffDelay { get { return _latchOffDelay; } }

    [SerializeField]
    [Tooltip("The time the Octopus stays on a rock before finding a new one")]
    private int _restTime = 500;
    public int RestTime { get { return _restTime; } }

    [SerializeField]
    private int _awareDuration = 200;
    public int AwareDuration { get { return _awareDuration; } }

    private int _awakeCounter = 0;
    public int AwakeCounter { get { return _awakeCounter; } set { _awakeCounter = value; } }

    private Collider _collider;
    public Collider Collider { get { return _collider; } }

    //Resting pos
    private Vector3 _rockPos;
    public Vector3 RockPos { get { return _rockPos; } set { _rockPos = value; } }

    //Resting upside
    private Vector3 _rockNormal;
    public Vector3 RockNormal { get { return _rockNormal; } set { _rockNormal = value; } }

    private Vector3 _targetNormal;
    public Vector3 TargetNormal { get { return _targetNormal; } set { _targetNormal = value; } }

    private bool _isChasing = false;
    public bool IsChasing { get { return _isChasing; } set { _isChasing = value; } }

    private Tentacles _tentacleControl;
    public Tentacles TentacleControl { get { return _tentacleControl; } }

    public override void Start()
    {
        base.Start();
        
        _collider = GetComponent<Collider>();
        _rockPos = origPos;
        _attackCounter = _attackCooldown;
        _tentacleControl = GetComponent<Tentacles>();
        _tentacleControl.Start();

        stateCache[typeof(OctopusFindRock)] = new OctopusFindRock(this);
        stateCache[typeof(OctopusBurstIdle)] = new OctopusBurstIdle(this);
        stateCache[typeof(OctopusBurstMove)] = new OctopusBurstMove(this);
        stateCache[typeof(OctopusBurstChase)] = new OctopusBurstChase(this);
        stateCache[typeof(OctopusLatchOnRock)] = new OctopusLatchOnRock(this);
        stateCache[typeof(OctopusLatchOnPlayer)] = new OctopusLatchOnPlayer(this);
        stateCache[typeof(OctopusLatchOffRock)] = new OctopusLatchOffRock(this);
        stateCache[typeof(OctopusLatchOffPlayer)] = new OctopusLatchOffPlayer(this);
        stateCache[typeof(OctopusSleep)] = new OctopusSleep(this);

        SetState<OctopusFindRock>();
    }

    public override void FixedUpdate()
    {
        BindZ();

        base.FixedUpdate();
    }

    public void OnTriggerEnter(Collider c)
    {
        //Slowly count up to make it aware
        if (c.tag == "Pulse")
        {
            Debug.DrawLine(transform.position, Target.position, Color.red, 1f);
            if (!Physics.Linecast(transform.position, Target.position, ~IgnoreDetection) && Vector3.Distance(OriginPos, Target.position) < Range)
                _awakeCounter++;
        }
    }

    public void OnCollisionEnter(Collision c)
    {
        //Instantly wake when the player collides
        if (c.transform == Target)
        {
            IsChasing = true;
            SetState<OctopusBurstChase>();
        }
    }

    public override bool DetectTarget()
    {
        return false;

        //Wait for cooldown
        if (_attackCounter != _attackCooldown)
        {
            _attackCounter++;
            return false;
        }

        bool detected = base.DetectTarget();

        if (detected)
            _isChasing = true;
        else
            _isChasing = false;

        return detected;
    }

    public override Quaternion GetLookRotation(Vector3 direction)
    {
        //Proper rotation for the model
        Quaternion lookRot = base.GetLookRotation(direction);
        lookRot.eulerAngles -= new Vector3(180f, 0f, 180f);
        return lookRot;
    }

    public Quaternion GetLatchOnRot(Vector3 direction)
    {
        //Proper rotation for the model while latching on
        Quaternion lookRot = Quaternion.LookRotation(direction);
        lookRot.eulerAngles -= new Vector3(90f, 0f, 180f);
        return lookRot;
    }

    public bool CheckLatchOnRange()
    {
        return (Vector3.Distance(transform.position, IsChasing ? Target.position : _rockPos) < _latchOnRange);
    }
}