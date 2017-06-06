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

        _octo.Body.AddForce(_octo.RockNormal * _octo.MoveSpeed);
        _octo.TentacleControl.SetState<TentacleLatchOff>();
        //_octo.SetState<OctopusFindRock>();
        _octo.SetState<OctopusBurstChase>();
    }

    public override void Step()
    {
        //Empty
    }
}