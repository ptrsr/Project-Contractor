using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusLatchOffRock : FishState
{
    public OctopusLatchOffRock(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _octo.Body.isKinematic = false;

        _octo.Body.AddForce(_octo.IsChasing ? Vector3.zero : _octo.RockNormal * _octo.MoveSpeed / 2f);
        _octo.SetState<OctopusFindRock>();
    }

    public override void Step()
    {
        //Empty
    }
}