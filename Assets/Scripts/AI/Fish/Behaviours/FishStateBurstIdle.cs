using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateBurstIdle : FishState
{
    public FishStateBurstIdle(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    private int _count = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
    }

    public override void Step()
    {
        if (_octo.CheckLatchOnRange())
            _octo.SetState<FishStateLatchOn>();

        //Idle time
        if (_count < (_octo.IsChasing ? fish.IdleTime / _octo.IdleIntervalChange : fish.IdleTime))
        {
            _count++;
        }
        else
        {
            _octo.SetState<FishStateBurstMove>();
            _count = 0;
        }
    }
}