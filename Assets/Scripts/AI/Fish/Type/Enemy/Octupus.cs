using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octupus : FishEnemy
{
    public override void Start()
    {
        base.Start();

        stateCache[typeof(FishStateBurstIdle)] = new FishStateBurstIdle(this);
        stateCache[typeof(FishStateBurstMove)] = new FishStateBurstMove(this);
        stateCache[typeof(FishStateBurstChase)] = new FishStateBurstChase(this);
        stateCache[typeof(FishStateLatchOn)] = new FishStateLatchOn(this);
        stateCache[typeof(FishStateLatchOff)] = new FishStateLatchOff(this);

        SetState<FishStateBurstIdle>();
    }

    public override void Update()
    {
        base.Update();

        //if (Target.GetComponent<ZBound>().tap)
        //    SetState<FishStateLatchOff>();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.transform != Target)
            return;

        SetState<FishStateLatchOn>();
    }
}