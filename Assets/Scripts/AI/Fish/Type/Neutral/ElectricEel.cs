using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ElectricEel : FishNeutral
{
    [Header("Electric Eel Variables")]
    [SerializeField]
    private int _chargeSpeed = 400;
    public int ChargeSpeed { get { return _chargeSpeed; } }

    [SerializeField]
    private int _attackDuration = 100;
    public int AttackDuration { get { return _attackDuration; } }

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

    private List<Rigidbody> _bodies = new List<Rigidbody>();
    public List<Rigidbody> Bodies { get { return _bodies; } }

    public override void Start()
    {
        base.Start();

        _bodies = transform.parent.GetComponentsInChildren<Rigidbody>().ToList();
        _anchorOrigPos = _anchor.position;
        _anchorBody = _anchor.GetComponent<Rigidbody>();
        _collider = GetComponent<Collider>();
        _holeExit = _hole.GetComponentsInChildren<Transform>()[1];

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
        SubMovement sub = Target.GetComponent<SubMovement>();
        sub.StunSlowCooldown = 40;
        sub.StunPlayer();

        //Drain oxygen from the player
        OxygenVals.Remove(OxygenDrain);

        //Add force to push the player away
        c.rigidbody.AddForce(Direction * _knockbackStrength, ForceMode.Impulse);
        //Change state to return to hole
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