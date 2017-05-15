using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Octopus : FishEnemy
{
    [Header("Octopus Variables")]
    [SerializeField]
    private float _idleIntervalChange = 2f;
    public float IdleIntervalChange { get { return _idleIntervalChange; } }

    //Resting location
    private Transform _rock;
    public Transform Rock { get { return _rock; } }

    private bool _isChasing = false;
    public bool IsChasing { get { return _isChasing; } }

    public override void Start()
    {
        base.Start();

        stateCache[typeof(FishStateBurstIdle)] = new FishStateBurstIdle(this);
        stateCache[typeof(FishStateBurstMove)] = new FishStateBurstMove(this);
        stateCache[typeof(FishStateBurstChase)] = new FishStateBurstChase(this);
        stateCache[typeof(FishStateLatchOn)] = new FishStateLatchOn(this);
        stateCache[typeof(FishStateLatchOff)] = new FishStateLatchOff(this);

        SetState<FishStateBurstIdle>();
    }

    public override void Update()
    {
        base.Update();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.transform != Target)
            return;

        SetState<FishStateLatchOn>();
    }

    public override Quaternion GetLookRotation(Vector3 direction)
    {
        Quaternion lookRot = base.GetLookRotation(direction);
        lookRot.eulerAngles -= new Vector3(180f, 0f, 180f);
        return lookRot;
    }
}