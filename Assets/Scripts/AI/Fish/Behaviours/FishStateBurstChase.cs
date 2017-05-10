using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateBurstChase : FishState
{
    public FishStateBurstChase(Fish pFish) : base(pFish) { }

    private FishEnemy _fishEnemy;

    public override void Initialize()
    {
        _fishEnemy = (FishEnemy)fish;
    }

    public override void Step()
    {
        if (Vector3.Distance(fish.transform.position, fish.OriginPos) > _fishEnemy.Range)
        {
            fish.SetState<FishStateBurstMove>();
        }

        Vector3 dir = _fishEnemy.Target.position - fish.transform.position;

        if (fish.RotateTowards(dir, fish.RotationModifier))
        {
            fish.Body.AddForce(dir.normalized * _fishEnemy.ChaseSpeed);
            fish.SetState<FishStateBurstIdle>();
        }
    }
}