using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateWayPoint : FishState
{
    public FishStateWayPoint(Fish pFish) : base(pFish) { }

    private List<Transform> _wayPoints;
    private int _wayPointId = 0;

    public override void Initialize()
    {
        _wayPoints = ((FishFriendly)fish).WayPoints;
    }

    public override void Step()
    {
        if (Vector3.Distance(fish.transform.position, _wayPoints[_wayPointId].position) < 4)
        {
            NextWayPoint();
        }

        Vector3 direction = _wayPoints[_wayPointId].position - fish.transform.position;
        Quaternion lookDir = Quaternion.LookRotation(direction);
        lookDir.eulerAngles -= new Vector3(-90, 0, 0);
        fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, lookDir, 0.05f);
        fish.Body.AddRelativeForce(new Vector3(0, 0.5f), ForceMode.Force);
    }

    private void NextWayPoint()
    {
        if (_wayPointId == _wayPoints.Count - 1)
            _wayPointId = 0;
        else
            _wayPointId++;
    }
}