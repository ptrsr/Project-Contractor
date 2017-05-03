using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemy : Fish
{
    [SerializeField]
    private Transform _target;
    public Transform Target { get { return _target; } }

    public LayerMask mask;

    public override void Start()
    {
        base.Start();

        stateCache[typeof(FishStateIdle)] = new FishStateIdle(this);
        stateCache[typeof(FishStateMove)] = new FishStateMove(this);
        stateCache[typeof(FishStateChase)] = new FishStateChase(this);

        SetState<FishStateIdle>();
    }


    public void OnCollisionEnter(Collision c)
    {
        Rigidbody body = c.collider.GetComponent<Rigidbody>();

        if (body != null)
            body.AddForce((_target.position - transform.position).normalized * 20);
    }
}