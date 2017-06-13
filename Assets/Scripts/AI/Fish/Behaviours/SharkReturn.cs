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
        _waypoints = new Queue<Transform>();
        _shark = (Shark)fish;
        _shark.Body.velocity = Vector3.zero;
        //FindWayPoint();
        _shark.WayId = _shark.SyncTarget.WayId + _shark.SyncStep >= _shark.WayPoints.Length ? _shark.SyncTarget.WayId + _shark.SyncStep - _shark.WayPoints.Length : _shark.SyncTarget.WayId + _shark.SyncStep;
        //_shark.WayId = _shark.WayId + 1 >= _shark.WayPoints.Length ? 0 : _shark.WayId + 1;
        //_shark.SetState<SharkWayPoint>();
        _otherDistance = Vector3.Distance(_shark.SyncTarget.WayPoints[_shark.SyncTarget.WayId].position, _shark.SyncTarget.transform.position);
        //if (_otherDistance < 35)
        //{
        //    _otherDistance = Vector3.Distance(_shark.SyncTarget.WayPoints[_shark.SyncTarget.WayId].position, _shark.SyncTarget.transform.position) +
        //        Vector3.Distance(_shark.SyncTarget.WayPoints[_shark.SyncTarget.WayId + 1 >= _shark.SyncTarget.WayPoints.Length ? 0 : _shark.SyncTarget.WayId + 1].position, _shark.SyncTarget.transform.position);
        //    _shark.WayId = _shark.WayId + 1 >= _shark.WayPoints.Length ? 0 : _shark.WayId + 1;
        //    Debug.Log("Taking next waypoint");
        //}

        if (Physics.Linecast(_shark.transform.position, _shark.WayPoints[_shark.WayId].position, ~_shark.IgnoreDetection))
        {
            int wayId = _shark.GetWayPointId(_shark.GetNearestWayPointTo(_shark.transform));

            _waypoints.Enqueue(_shark.WayPoints[wayId]);
            int i = wayId;
            if (_shark.SyncTarget.WayId > wayId)
            {
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
            _waypoints.Enqueue(_shark.WayPoints[_shark.WayId]);

        _time = _otherDistance / _shark.SyncTarget.MoveSpeed / 1.5f;
        float distance = 0;
        //Debug.Log("Count: " + _waypoints.Count);
        foreach (Transform point in _waypoints)
        {
            distance += Vector3.Distance(point.position, _shark.transform.position);
        }
        //_speed = Vector3.Distance(_shark.transform.position, _shark.WayPoints[_shark.WayId].position) / _time;
        _speed = distance / _time;
        //Debug.Log(string.Format("Speed: {0} Distance: {1}, Time: {2}", _speed, distance, _time));
        _curPoint = _waypoints.Dequeue();
    }

    public override void Step()
    {
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