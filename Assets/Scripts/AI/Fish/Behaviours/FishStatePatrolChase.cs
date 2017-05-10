using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStatePatrolChase : FishState
{
    public FishStatePatrolChase(Fish pFish) : base(pFish) { }

    private Shark _shark;

    public override void Initialize()
    {
        _shark = (Shark)fish;
    }

    public override void Step()
    {
        if (!_shark.DetectTarget())
            fish.SetState<FishStatePatrol>();

        Vector3 dir = _shark.Target.transform.position - fish.transform.position;
        fish.RotateTowards(dir, _shark.RotationModifier);
        fish.Body.AddForce(dir.normalized * _shark.ChaseSpeed);
    }
}