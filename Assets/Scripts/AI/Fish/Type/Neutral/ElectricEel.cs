using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEel : FishNeutral
{
    [Header("Electric Eel Variables")]
    [SerializeField]
    private Transform _hole = null;
    public Transform Hole { get { return _hole; } }

    private Transform _holeExit;
    public Transform HoleExit { get { return _holeExit; } }

    [SerializeField]
    private float _knockbackStrength = 100f;

    [SerializeField]
    private float _knockUpStrength = 25f;

    private Collider _collider;
    public Collider Collider { get { return _collider; } }

    public override void Start()
    {
        base.Start();

        _target = FindObjectOfType<SubMovement>().transform;
        _collider = GetComponent<Collider>();

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

        c.rigidbody.AddForce(new Vector3(Direction.x * _knockbackStrength, _knockUpStrength), ForceMode.Impulse);
    }

    public override Quaternion GetLookRotation(Vector3 direction)
    {
        Quaternion lookRot = base.GetLookRotation(direction);
        return lookRot;
    }
}