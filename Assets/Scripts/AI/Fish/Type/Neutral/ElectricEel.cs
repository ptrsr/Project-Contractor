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

    private Collider _collider;
    public Collider Collider { get { return _collider; } }

    public override void Start()
    {
        origPos = transform.position;
        origRot = transform.rotation;
        body = GetComponent<Rigidbody>();

        _anchorOrigPos = _anchor.position;
        _anchorBody = _anchor.GetComponent<Rigidbody>();

        _collider = GetComponent<Collider>();

        _target = FindObjectOfType<SubMovement>().transform;

        _holeExit = _hole.GetComponentsInChildren<Transform>()[1];

        Direction = (Vector3.right * WallDetectionRange).normalized;

        stateCache[typeof(FishStateHide)] = new FishStateHide(this);
        stateCache[typeof(FishStateCharge)] = new FishStateCharge(this);
        stateCache[typeof(FishStateReturnToHole)] = new FishStateReturnToHole(this);

        SetState<FishStateHide>();
    }

    public override void Update()
    {
        BindZ();

        base.Update();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.rigidbody == null)
            return;

        //Stun player

        c.rigidbody.AddForce(Direction * _knockbackStrength, ForceMode.Impulse);
        SetState<FishStateReturnToHole>();
    }

    public override Quaternion GetLookRotation(Vector3 direction)
    {
        Quaternion lookRot = base.GetLookRotation(direction);
        return lookRot;
    }
}