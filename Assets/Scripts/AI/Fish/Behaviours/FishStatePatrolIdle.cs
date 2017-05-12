using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStatePatrolIdle : FishState
{
    public FishStatePatrolIdle(Fish pFish) : base(pFish) { }

    private int _counter = 0;

    public override void Initialize()
    {
        
    }

    public override void Step()
    {
        if (_counter != fish.IdleTime)
        {
            _counter++;
        }
        else
        {
            _counter = 0;
            fish.SetState<FishStatePatrol>();
        }
    }
}