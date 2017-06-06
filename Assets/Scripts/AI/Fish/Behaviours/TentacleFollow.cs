using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleFollow : FishState
{
    public TentacleFollow(Fish pFish) : base(pFish) { }

    private Tentacles _tentacles;

    public override void Initialize()
    {
        _tentacles = (Tentacles)fish;
        _tentacles.SetCollidersActive(false);
        _tentacles.SetKinematic(false);
    }

    public override void Step()
    {
        foreach (Rigidbody tentacle in _tentacles.TentacleTips)
        {
            //Use the direction to prevent tentacles hanging down
            if (_tentacles.Octo.Body.velocity == Vector3.zero)
                tentacle.AddForce(-_tentacles.Octo.Direction * _tentacles.MoveSpeed);
            else
                tentacle.AddForce(-_tentacles.Octo.Body.velocity.normalized * _tentacles.MoveSpeed);
        }
    }
}