using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEel : FishNeutral
{
    [Header("Electric Eel Variables")]
    [SerializeField]
    private int _chargeSpeed = 400;
    public int ChargeSpeed { get { return _chargeSpeed; } }

    [SerializeField]
    private Transform _hole = null;
    public Transform Hole { get { return _hole; } }

    private Transform _holeExit;
    public Transform HoleExit { get { return _holeExit; } }

    private Vector3 _anchorOrigPos;
    public Vector3 AnchorOrigPos { get { return _anchorOrigPos; } }

    private Rigidbody _anchorBody;
    public Rigidbody AnchorBody { get { return _anchorBody; } }
    
    [SerializeField]
    private Transform _anchor = null;
    public Transform Anchor { get { return _anchor; } }

    [SerializeField]
    private float _knockbackStrength = 100f;

    private Vector3 _holeDir;
    public Vector3 HoleDir { get { return _holeDir; } }

    private Collider _collider;
    public Collider Collider { get { return _collider; } }

    private RigidbodyConstraints _xCons = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    public RigidbodyConstraints XCons { get { return _xCons; } }

    private RigidbodyConstraints _yCons = RigidbodyConstraints.FreezePositionX | RigidbodyConstraints.FreezePositionZ | RigidbodyConstraints.FreezeRotation;
    public RigidbodyConstraints YCons { get { return _yCons; } }

    public override void Start()
    {
        origPos = transform.position;
        origRot = transform.rotation;
        body = GetComponent<Rigidbody>();

        _anchorOrigPos = _anchor.position;
        _anchorBody = _anchor.GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        target = FindObjectOfType<SubMovement>().transform;
        targetBody = target.GetComponent<Rigidbody>();
        _holeExit = _hole.GetComponentsInChildren<Transform>()[1];
        _holeDir = _holeExit.position - AnchorOrigPos;

        _holeDir = new Vector3(_holeDir.x < 0 ? -_holeDir.x : _holeDir.x, _holeDir.y < 0 ? -_holeDir.y : _holeDir.y, 0);

        if (_holeDir.x > _holeDir.y)
            AnchorBody.constraints = _xCons;
        else if (_holeDir.y > _holeDir.x)
            AnchorBody.constraints = _yCons;

        Direction = (Vector3.right * WallDetectionRange).normalized;

        stateCache[typeof(EelHide)] = new EelHide(this);
        stateCache[typeof(EelCharge)] = new EelCharge(this);
        stateCache[typeof(EelReturnToHole)] = new EelReturnToHole(this);

        SetState<EelHide>();
    }

    public override void FixedUpdate()
    {
        BindZ();

        base.FixedUpdate();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.rigidbody == null)
            return;

        //Stun player
        Target.GetComponent<SubMovement>().StunPlayer();
        
        c.rigidbody.AddForce(Direction * _knockbackStrength, ForceMode.Impulse);
        SetState<EelReturnToHole>();
    }

    public bool DetectTarget()
    {
        float targetDis = Vector3.Distance(OriginPos, Target.position);
        float origDis = Vector3.Distance(transform.position, OriginPos);

        /* Check if:
         * target is not obstructed by a wall
         * target is in range of the enemy
         * target is in range of the origin
         * enemy is in range of the origin */
        return (!Physics.Linecast(transform.position, Target.position, ~IgnoreDetection) && targetDis < Range && origDis < Range);
    }

    public override Quaternion GetLookRotation(Vector3 direction)
    {
        //Proper rotation for the model
        Quaternion lookRot = base.GetLookRotation(direction);
        return lookRot;
    }
}