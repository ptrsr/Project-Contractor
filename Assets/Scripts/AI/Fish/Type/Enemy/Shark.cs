using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Shark : FishEnemy
{
    [Header("Shark Variables")]
    [SerializeField]
    private int _timeForRandomDir = 200;
    public int TimeForRandomDir { get { return _timeForRandomDir; } }

    [SerializeField]
    private int _knockBackStrength = 60;
    public int KnockBackStrength { get { return _knockBackStrength; } }

    [SerializeField]
    private Transform _path = null;

    [SerializeField]
    private int _detectionAngle = 45;
    public int DetectionAngle { get { return _detectionAngle; } }

    [SerializeField]
    private int _pointRange = 4;
    public int PointRange { get { return _pointRange; } }

    private Transform[] _waypoints;
    public Transform[] WayPoints { get { return _waypoints; } }

    private int _wayId = 0;
    public int WayId { get { return _wayId; } set { _wayId = value; } }

    public override void Start()
    {
        base.Start();

        //Remove parent from array
        List<Transform> temp = _path.GetComponentsInChildren<Transform>().ToList();
        temp.RemoveAt(0);
        _waypoints = temp.ToArray();

        stateCache[typeof(FishStatePatrolIdle)] = new FishStatePatrolIdle(this);
        stateCache[typeof(FishStatePatrolPoint)] = new FishStatePatrolPoint(this);
        stateCache[typeof(FishStatePatrolChase)] = new FishStatePatrolChase(this);
        stateCache[typeof(FishStatePatrolReturn)] = new FishStatePatrolReturn(this);

        SetState<FishStatePatrolPoint>();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.transform != Target)
            return;

        c.rigidbody.AddForce(Direction * KnockBackStrength, ForceMode.Impulse);

        SetState<FishStatePatrolIdle>();
    }

    public Transform GetNearestWayPoint()
    {
        int id = -1;
        float lowestRange = 9999f;

        for (int i = 0; i < _waypoints.Length - 1; i++)
        {
            float testRange = Vector3.Distance(transform.position, _waypoints[i].position);
            if (testRange < lowestRange)
            {
                id = i;
                lowestRange = testRange;
            }
        }

        return _waypoints[id];
    }

    public int GetWayPointId(Transform waypoint)
    {
        for (int i = 0; i < _waypoints.Length - 1; i++)
        {
            if (_waypoints[i] == waypoint)
                //Waypoint found
                return i;
        }

        //Waypoint not found
        return -1;
    }

    public override bool DetectTarget()
    {
        Transform nearest = GetNearestWayPoint();
        float targetDis = Vector3.Distance(transform.position, Target.position);
        float pointDis = Vector3.Distance(transform.position, nearest.position);
        float targetPointDis = Vector3.Distance(nearest.position, Target.position);

        /*Check if:
         * target is in an angle in front
         * is not obstructed by walls
         * target is in range to the shark
         * shark is in range to a waypoint
         * target is in range to a waypoint*/
        return (Vector3.Angle(-transform.right, Target.position - transform.position) < _detectionAngle &&
            !Physics.Raycast(transform.position, Target.position, ~IgnoreDetection) &&
            targetDis < Range && pointDis < Range && targetPointDis < Range);
    }

    public override Quaternion GetLookRotation(Vector3 direction)
    {
        //Proper rotation for the model for rotating towards points
        Quaternion lookRot = base.GetLookRotation(direction);
        lookRot.eulerAngles = new Vector3(0, lookRot.eulerAngles.y + 90f, lookRot.eulerAngles.x);
        return lookRot;
    }

    public Transform GetCurrentWayPoint()
    {
        return _waypoints[_wayId];
    }
}