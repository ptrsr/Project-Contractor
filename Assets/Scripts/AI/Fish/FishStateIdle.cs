using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateIdle : FishState
{
    public FishStateIdle(Fish pFish) : base(pFish) { }

    private int _count = 0;
    private int _duration = 100;

    public override void Initialize()
    {

    }

    public override void Step()
    {
        fish.transform.position = new Vector3(fish.transform.position.x, fish.transform.position.y, 0);

        if (_count < _duration)
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