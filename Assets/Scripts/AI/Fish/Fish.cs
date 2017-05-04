using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{
    [Header("Fish General Variables")]
    [SerializeField]
    private float _moveSpeed = 20f;
    public float MoveSpeed { get { return _moveSpeed; } }

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

    private Vector3 _originPos;
    public Vector3 OriginPos { get { return _originPos; } }

    private Rigidbody _body;
    public Rigidbody Body { get { return _body; } }

    //State tracking
    protected Dictionary<Type, FishState> stateCache = new Dictionary<Type, FishState>();
    private FishState _state;

    public virtual void Start()
    {
        _originPos = transform.position;
        _body = GetComponent<Rigidbody>();
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

    public bool RotateTowards(Vector3 target, float rotationModifier = 0f)
    {
        if (_rotationCount != _rotationTime)
        {
            Quaternion lookDir = Quaternion.LookRotation(target);
            lookDir.eulerAngles -= new Vector3(-90, 0, 0);
            transform.rotation = Quaternion.Slerp(transform.rotation, lookDir, _rotationSpeed + rotationModifier);
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
}