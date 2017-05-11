using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStatePatrolChase : FishState
{
    public FishStatePatrolChase(Fish pFish) : base(pFish) { }

    private Shark _shark;
    private Rigidbody _targetBody;

    public override void Initialize()
    {
        _shark = (Shark)fish;
        _targetBody = _shark.Target.GetComponent<Rigidbody>();
    }

    public override void Step()
    {
        if (!_shark.DetectTarget())
            fish.SetState<FishStatePatrol>();
        
        Vector3 dir = (_shark.Target.transform.position + (_targetBody.velocity / _shark.Difficulty)) - fish.transform.position;
        fish.RotateTowards(dir, _shark.RotationModifier);
        fish.Body.AddForce(dir.normalized * _shark.ChaseSpeed);
    }
}