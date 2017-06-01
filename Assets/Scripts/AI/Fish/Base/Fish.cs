using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    [Header("Fish General Variables")]
    [SerializeField]
    private float _moveSpeed = 20f;
    public float MoveSpeed { get { return _moveSpeed; } set { _moveSpeed = value; } }

    [SerializeField]
    private int _rotationTime = 60;
    public int RotationTime { get { return _rotationTime; } }
    private int _rotationCount = 0;

    [SerializeField]
    private float _rotationSpeed = 0.05f;
    public float RotationSpeed { get { return _rotationSpeed; } }

    [SerializeField]
    private float _rotationModifier = 0.1f;
    public float RotationModifier { get { return _rotationModifier; } }

    [SerializeField]
    private int _idleTime = 100;
    public int IdleTime { get { return _idleTime; } }

    //Hidden variables
    [NonSerialized]
    public Vector3 Direction;

    protected Vector3 origPos;
    public Vector3 OriginPos { get { return origPos; } }

    protected Quaternion origRot;
    public Quaternion OriginRot { get { return origRot; } }

    protected Rigidbody body;
    public Rigidbody Body { get { return body; } }

    //State tracking
    protected Dictionary<Type, FishState> stateCache = new Dictionary<Type, FishState>();
    private FishState _state;
    public FishState GetState { get { return _state; } }

    public virtual void Start()
    {
        origPos = transform.position;
        origRot = transform.rotation;
        body = GetComponent<Rigidbody>();
    }

    public virtual void Update()
    {
        _state.Step();
    }

    //Switching states
    public void SetState<T>() where T : FishState
    {
        _state = stateCache[typeof(T)];
        _state.Initialize();
    }

    //Removing a state
    public void RemoveState<T>() where T : FishState
    {
        stateCache[typeof(T)] = null;
    }

    public bool RotateTowards(Quaternion lookRotation, float rotationModifier = 0f)
    {
        if (_rotationCount != _rotationTime)
        {
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, _rotationSpeed + rotationModifier);
            _rotationCount++;

            //Still rotating
            return false;
        }
        else
        {
            _rotationCount = 0;

            //Done rotating
            return true;
        }
    }

    public virtual Quaternion GetLookRotation(Vector3 direction)
    {
        return Quaternion.LookRotation(direction);
    }

    //Places the object on z = 0
    protected void BindZ()
    {
        transform.position = new Vector3(transform.position.x, transform.position.y, 0f);
    }
}