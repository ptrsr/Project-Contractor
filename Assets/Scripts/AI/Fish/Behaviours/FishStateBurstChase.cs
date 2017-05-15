using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateBurstChase : FishState
{
    public FishStateBurstChase(Fish pFish) : base(pFish) { }

    private Octopus _octopus;

    public override void Initialize()
    {
        _octopus = (Octopus)fish;
    }

    public override void Step()
    {
        if (Vector3.Distance(fish.transform.position, fish.OriginPos) > _octopus.Range)
        {
            fish.SetState<FishStateBurstMove>();
        }

        Vector3 dir = _octopus.Target.position - fish.transform.position;

        if (fish.RotateTowards(_octopus.GetLookRotation(dir), fish.RotationModifier))
        {
            fish.Body.AddForce(dir.normalized * _octopus.ChaseSpeed);
            fish.SetState<FishStateBurstIdle>();
        }
    }
}