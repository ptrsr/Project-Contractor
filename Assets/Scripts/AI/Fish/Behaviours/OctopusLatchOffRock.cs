using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusLatchOffRock : FishState
{
    public OctopusLatchOffRock(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    private int _counter = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _octo.Body.isKinematic = false;
        _octo.Body.mass = 0.1f;

        //Quick force to get off the rock
        _octo.Body.AddForce(_octo.RockNormal * _octo.MoveSpeed);
        _octo.TentacleControl.SetState<TentacleLatchOff>();
    }

    public override void Step()
    {
        //Delay time
        if (_counter == _octo.LatchOffDelay)
        {
            _counter = 0;
            _octo.SetState<OctopusBurstChase>();
        }
        else
            _counter++;
    }
}