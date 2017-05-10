using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Squid : FishEnemy
{
    public override void Start()
    {
        base.Start();

        stateCache[typeof(FishStateBurstIdle)] = new FishStateBurstIdle(this);
        stateCache[typeof(FishStateBurstMove)] = new FishStateBurstMove(this);
        stateCache[typeof(FishStateBurstChase)] = new FishStateBurstChase(this);
        stateCache[typeof(FishStateInk)] = new FishStateInk(this);

        SetState<FishStateBurstIdle>();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.transform == Target)
            SetState<FishStateInk>();
    }
}