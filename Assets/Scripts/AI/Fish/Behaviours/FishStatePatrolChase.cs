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
            fish.SetState<FishStatePatrolReturn>();
        
        _shark.Direction = ((_shark.Target.transform.position + (_targetBody.velocity / _shark.Difficulty)) - fish.transform.position).normalized;
        _shark.RotateTowards(_shark.GetLookRotation(_shark.Direction), _shark.RotationModifier);
        _shark.Body.AddForce(_shark.Direction * _shark.ChaseSpeed);
    }
}