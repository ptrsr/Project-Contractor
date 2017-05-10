using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEel : FishNeutral
{
    public override void Start()
    {
        base.Start();

        Direction = Vector3.right * WallDetectionRange;

        stateCache[typeof(FishStateContinuesMove)] = new FishStateContinuesMove(this);
        stateCache[typeof(FishStateFlipRotation)] = new FishStateFlipRotation(this);

        SetState<FishStateContinuesMove>();
    }

    public override void Update()
    {
        BindZ();

        base.Update();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.rigidbody == null)
            return;

        //Stun player

        c.rigidbody.AddForce(new Vector3(MoveSpeed / 2f, 2f, 0), ForceMode.Impulse);
    }
}