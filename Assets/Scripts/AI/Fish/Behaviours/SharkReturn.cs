using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkReturn : FishState
{
    public SharkReturn(Fish pFish) : base(pFish) { }

    private Shark _shark;
    private float _time;
    private float _speed;
    private float _otherDistance;
    private Queue<Transform> _waypoints;
    private Transform _curPoint;

    public override void Initialize()
    {
        _shark = (Shark)fish;

        if (!_shark.SyncTarget)
        {
            FindWayPoint();
            _shark.SetState<SharkWayPoint>();
        }
        else
        {
            SyncUp();
        }
    }

    public override void Step()
    {
        if (!_shark.SyncTarget)
            return;

        if (_shark.DetectTarget())
            _shark.SetState<SharkChase>();

        //Check if shark is in range for it's destination
        if (Vector3.Distance(_shark.transform.position, _shark.WayPoints[_shark.WayId].position) < _shark.PointRange)
        {
            _shark.Body.velocity = Vector3.zero;
            _shark.SetState<SharkWayPoint>();
            return;
        }

        if (Vector3.Distance(_shark.transform.position, _curPoint.position) < _shark.PointRange && _waypoints.Count > 0)
        {
            _shark.Body.velocity /= 4f;
            _curPoint = _waypoints.Dequeue();
        }
        
        //Move and rotate towards the next waypoint
        //_shark.Direction = (_shark.GetCurrentWayPoint().position - _shark.transform.position).normalized;
        _shark.Direction = (_curPoint.position - _shark.transform.position).normalized;
        _shark.RotateTowards(_shark.GetLookRotation(_shark.Direction));
        _shark.Body.AddForce(_speed * _shark.Direction);
        //_shark.Body.AddForce(_shark.Direction * _shark.MoveSpeed);
    }

    private void FindWayPoint()
    {
        Transform point = _shark.GetNearestWayPointTo(_shark.transform);
        int id = _shark.GetWayPointId(point);
        int startId = _shark.GetWayPointId(point);

		int count = 0;

        //Try to find a waypoint in front of the shark to prevent back tracking
        do
        {
            _shark.WayId = id + 1 >= _shark.WayPoints.Length - 1 ? 0 : id + 1;
            point = _shark.WayPoints[_shark.WayId];

            id++;
            id = id >= _shark.WayPoints.Length ? 0 : id;

            if (id == startId) //Checked all, break the loop
            {
                _shark.WayId = id + 1 >= _shark.WayPoints.Length ? 0 : id + 1;
                break;
            }
			count++;
            //Check if a waypoint is in front of the shark
		} while (Vector3.Angle(-_shark.transform.right, point.position - _shark.transform.position) > 45f || count != _shark.WayPoints.Length);
    }

    private void SyncUp()
    {
        _waypoints = new Queue<Transform>();
        //Remove remaining movement
        _shark.Body.velocity = Vector3.zero;
        //Assign the waypoint id that is on sync distance from the sync target
        _shark.WayId = _shark.SyncTarget.WayId + _shark.SyncStep >= _shark.WayPoints.Length ? _shark.SyncTarget.WayId + _shark.SyncStep - _shark.WayPoints.Length : _shark.SyncTarget.WayId + _shark.SyncStep;
        //Distance from the sync target to it's current waypoint
        _otherDistance = Vector3.Distance(_shark.SyncTarget.WayPoints[_shark.SyncTarget.WayId].position, _shark.SyncTarget.transform.position);

        //Check if direct path is obstructed
        if (CheckLOS(_shark.transform.position, _shark.WayPoints[_shark.WayId].position))
        {
            int wayId = _shark.GetWayPointId(_shark.GetNearestWayPointTo(_shark.transform));

            //Enqueue the first point
            _waypoints.Enqueue(_shark.WayPoints[wayId]);
            int i = wayId;
            //Check in which direction the waypoint is to avoid collision with the sync target
            if (_shark.SyncTarget.WayId > wayId)
            {
                //Save waypoint in decreasing order
                do
                {
                    i--;
                    if (i < 0)
                        i = _shark.WayPoints.Length - 1;
                    _waypoints.Enqueue(_shark.WayPoints[i]);
                } while (i != _shark.WayId);
            }
            else
            {
                //Save waypoints in increasing order
                do
                {
                    i++;
                    if (i >= _shark.WayPoints.Length)
                        i = 0;
                    _waypoints.Enqueue(_shark.WayPoints[i]);
                } while (i != _shark.WayId);
            }
        }
        else
            //Save waypoint directly
            _waypoints.Enqueue(_shark.WayPoints[_shark.WayId]);

        //Time it takes for our shark to reach the sync waypoint
        _time = _otherDistance / _shark.SyncTarget.MoveSpeed / 1.5f;

        //Get the total distance of all the waypoint we need to pass
        float distance = 0;
        foreach (Transform point in _waypoints)
        {
            distance += Vector3.Distance(point.position, _shark.transform.position);
        }

        //Calculate the speed it needs to reach the sync waypoint in time
        _speed = distance / _time;

        //Assign first waypoint
        _curPoint = _waypoints.Dequeue();
    }

    //Checks if there is any obstruction between 2 points
    private bool CheckLOS(Vector3 from, Vector3 to)
    {
        return Physics.Linecast(from, to, ~_shark.IgnoreDetection);
    }
}