using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStatePatrolPoint : FishState
{
    public FishStatePatrolPoint(Fish pFish) : base(pFish) { }

    private Shark _shark;
    private Transform _curPoint;

    public override void Initialize()
    {
        _shark = (Shark)fish;
        _curPoint = _shark.GetCurrentWayPoint();
    }

    public override void Step()
    {
        if (_shark.DetectTarget())
            _shark.SetState<FishStatePatrolChase>();

        //Check if shark is in range for next waypoint
        if (Vector3.Distance(_shark.transform.position, _curPoint.position) < _shark.PointRange)
            _curPoint = NextWaypoint();

        //Move and rotate towards the next waypoint
        _shark.Direction = (_shark.GetCurrentWayPoint().position - _shark.transform.position).normalized;
        _shark.RotateTowards(_shark.GetLookRotation(_shark.Direction));
        _shark.Body.AddForce(_shark.Direction * _shark.MoveSpeed);
    }

    private Transform NextWaypoint()
    {
        _shark.WayId = _shark.WayId + 1 == _shark.WayPoints.Length ? 0 : _shark.WayId + 1;
        return _shark.WayPoints[_shark.WayId];
    }
}