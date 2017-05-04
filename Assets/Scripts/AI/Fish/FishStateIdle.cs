using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateIdle : FishState
{
    public FishStateIdle(Fish pFish) : base(pFish) { }

    private int _count = 0;

    public override void Initialize()
    {

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
            fish.SetState<FishStateMove>();
            _count = 0;
        }
    }
}