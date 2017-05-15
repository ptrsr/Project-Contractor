using System.Collections;
using System.Collections.Generic;
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

    public override void Start()
    {
        base.Start();

        stateCache[typeof(FishStatePatrolIdle)] = new FishStatePatrolIdle(this);
        stateCache[typeof(FishStatePatrol)] = new FishStatePatrol(this);
        stateCache[typeof(FishStatePatrolChase)] = new FishStatePatrolChase(this);

        SetState<FishStatePatrolIdle>();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.transform != Target)
            return;

        c.rigidbody.AddForce(Direction * KnockBackStrength, ForceMode.Impulse);

        SetState<FishStatePatrolIdle>();
    }

    public override Quaternion GetLookRotation(Vector3 direction)
    {
        Quaternion lookRot = base.GetLookRotation(direction);
        lookRot.eulerAngles = new Vector3(0, lookRot.eulerAngles.y + 90f, lookRot.eulerAngles.x);
        return lookRot;
    }
}