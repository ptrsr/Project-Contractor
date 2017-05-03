using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishFriendly : Fish
{
    [SerializeField]
    private List<Transform> _wayPoints;
    public List<Transform> WayPoints { get { return _wayPoints; } }

    public override void Start()
    {
        base.Start();

        stateCache[typeof(FishStateIdle)] = new FishStateIdle(this);
        stateCache[typeof(FishStateMove)] = new FishStateWayPoint(this);

        SetState<FishStateIdle>();
    }
}