using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkTail : FishEnemy
{
    [Header("Tail Variables")]
    [SerializeField]
    private Rigidbody _head = null;
    public Rigidbody Head { get { return _head; } }

    [SerializeField]
    private Rigidbody _tail = null;
    public Rigidbody Tail { get { return _tail; } }

    private Shark _shark;
    public Shark SharkValues { get { return _shark; } }

    [SerializeField]
    private List<Transform> _positions = null;
    public List<Transform> Positions { get { return _positions; } }

    public override void Start()
    {
        _shark = GetComponent<Shark>();

        origPos = _tail.position;
        origRot = _tail.rotation;
        body = _tail;

        stateCache[typeof(TailIdle)] = new TailIdle(this);
        stateCache[typeof(TailFollow)] = new TailFollow(this);

        SetState<TailFollow>();
    }
}