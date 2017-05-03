using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Fish : MonoBehaviour
{
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
}