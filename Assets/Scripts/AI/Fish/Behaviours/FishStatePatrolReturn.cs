using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStatePatrolReturn : FishState
{
    public FishStatePatrolReturn(Fish pFish) : base(pFish) { }

    private Shark _shark;

    public override void Initialize()
    {
        _shark = (Shark)fish;
        FindWayPoint();
        _shark.SetState<FishStatePatrolPoint>();
    }

    public override void Step()
    {
        //Empty
    }

    private void FindWayPoint()
    {
        Transform point = _shark.GetNearestWayPoint();
        int id = _shark.GetWayPointId(point);
        int startId = _shark.GetWayPointId(point);

        //Try to find a waypoint in front of the shark to prevent back tracking
        do
        {
            _shark.WayId = id + 1 >= _shark.WayPoints.Length - 1 ? 0 : id + 1;
            point = _shark.WayPoints[_shark.WayId];

            id++;
            id = id == _shark.WayPoints.Length - 1 ? 0 : id;

            if (id == startId) //Checked all, break the loop
            {
                _shark.WayId = id + 1 >= _shark.WayPoints.Length - 1 ? 0 : id + 1;
                break;
            }
            //Check if a waypoint is in front of the shark
        } while (Vector3.Angle(-_shark.transform.right, point.position - _shark.transform.position) > 45f);
    }
}