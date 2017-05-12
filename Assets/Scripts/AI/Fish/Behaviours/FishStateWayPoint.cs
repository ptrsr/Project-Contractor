using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateWayPoint : FishState
{
    public FishStateWayPoint(Fish pFish) : base(pFish) { }

    private FishFriendly _fishFriendly;
    private int _wayPointId = 0;

    public override void Initialize()
    {
        _fishFriendly = (FishFriendly)fish;
    }

    public override void Step()
    {
        if (Vector3.Distance(fish.transform.position, _fishFriendly.WayPoints[_wayPointId].position) < _fishFriendly.WayPointDistance)
        {
            NextWayPoint();
        }

        fish.Direction = _fishFriendly.WayPoints[_wayPointId].position - fish.transform.position;

        fish.RotateTowards(fish.Direction);
        fish.Body.AddRelativeForce(new Vector3(0, fish.MoveSpeed), ForceMode.Force);
    }

    private void NextWayPoint()
    {
        if (_wayPointId == _fishFriendly.WayPoints.Count - 1)
            _wayPointId = 0;
        else
            _wayPointId++;
    }
}