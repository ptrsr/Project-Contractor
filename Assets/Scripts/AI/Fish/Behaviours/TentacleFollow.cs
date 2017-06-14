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
            tentacle.AddForce(-_tentacles.Octo.Direction * _tentacles.MoveSpeed);
            tentacle.AddForce(-_tentacles.Octo.Body.velocity.normalized * _tentacles.MoveSpeed);
        }
    }
}