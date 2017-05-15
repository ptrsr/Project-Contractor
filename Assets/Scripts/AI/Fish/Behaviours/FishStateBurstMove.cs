using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateBurstMove : FishState
{
    public FishStateBurstMove(Fish pFish) : base(pFish) { }

    private Octopus _octopus;

    public override void Initialize()
    {
        _octopus = (Octopus)fish;

        if (_octopus.DetectTarget())
            fish.SetState<FishStateBurstChase>();
        _octopus.SetRandomDirection();
        _octopus.DetectWall();
    }

    public override void Step()
    {
        Debug.DrawRay(fish.transform.position, fish.Direction);

        if (fish.RotateTowards(_octopus.GetLookRotation(fish.Direction)))
        {
            fish.Body.AddForce(fish.Direction * fish.MoveSpeed, ForceMode.Force);
            fish.SetState<FishStateBurstIdle>();
        }
    }
}