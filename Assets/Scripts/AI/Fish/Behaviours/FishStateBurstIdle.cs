using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateBurstIdle : FishState
{
    public FishStateBurstIdle(Fish pFish) : base(pFish) { }

    private int _count = 0;

    public override void Initialize()
    {
        //Empty
    }

    public override void Step()
    {
        fish.transform.position = new Vector3(fish.transform.position.x, fish.transform.position.y, 0);

        //Idle time
        if (_count < fish.IdleTime)
        {
            _count++;
        }
        else
        {
            fish.SetState<FishStateBurstMove>();
            _count = 0;
        }
    }
}