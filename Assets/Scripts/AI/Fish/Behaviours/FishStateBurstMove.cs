using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateBurstMove : FishState
{
    public FishStateBurstMove(Fish pFish) : base(pFish) { }

    private FishEnemy _fishEnemy;

    public override void Initialize()
    {
        _fishEnemy = (FishEnemy)fish;

        if (_fishEnemy.DetectTarget())
            fish.SetState<FishStateBurstChase>();
        _fishEnemy.SetRandomDirection();
        _fishEnemy.DetectWall();
    }

    public override void Step()
    {
        Debug.DrawRay(fish.transform.position, fish.Direction);

        if (fish.RotateTowards(fish.Direction))
        {
            fish.Body.AddForce(fish.Direction, ForceMode.Force);
            fish.SetState<FishStateBurstIdle>();
        }
    }
}