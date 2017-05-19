using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStatePatrolReturn : FishState
{
    public FishStatePatrolReturn(Fish pFish) : base(pFish) { }

    private Shark _shark;
    private Transform _point;

    public override void Initialize()
    {
        _shark = (Shark)fish;
        _point = _shark.GetNearestWayPoint();
        _shark.WayId = _shark.GetWayPointId(_point);
        _shark.SetState<FishStatePatrolPoint>();
    }

    public override void Step()
    {
        //Empty
    }
}