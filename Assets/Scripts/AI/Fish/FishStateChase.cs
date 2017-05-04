using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateChase : FishState
{
    public FishStateChase(Fish pFish) : base(pFish) { }

    private FishEnemy _fishEnemy;
    int delay = 50;
    int count = 0;

    public override void Initialize()
    {
        _fishEnemy = (FishEnemy)fish;
    }

    public override void Step()
    {
        if (Vector3.Distance(fish.transform.position, fish.OriginPos) > _fishEnemy.DetectionRange)
        {
            fish.SetState<FishStateMove>();
        }

        Vector3 dir = _fishEnemy.Target.position - fish.transform.position;

        if (fish.RotateTowards(dir, fish.RotationModifier))
        {
            fish.Body.AddForce(dir.normalized * _fishEnemy.ChaseSpeed);
            fish.SetState<FishStateIdle>();
        }
    }
}