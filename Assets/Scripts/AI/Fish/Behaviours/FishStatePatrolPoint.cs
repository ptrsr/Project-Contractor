﻿using System.Collections;
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

        if (Vector3.Distance(_shark.transform.position, _curPoint.position) < _shark.PointRange)
            _curPoint = NextWaypoint();

        _shark.Direction = (_shark.GetCurrentWayPoint().position - _shark.transform.position).normalized;
        _shark.RotateTowards(_shark.GetLookRotation(_shark.Direction));
        _shark.Body.AddForce(_shark.Direction * _shark.MoveSpeed);
    }

    private Transform NextWaypoint()
    {
        _shark.WayId++;

        if (_shark.WayId == _shark.WayPoints.Length - 1)
            _shark.WayId = 0;

        return _shark.WayPoints[_shark.WayId];
    }
}