using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusLatchOffPlayer : FishState
{
    public OctopusLatchOffPlayer(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    private int _counter = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _octo.Body.isKinematic = false;
        _octo.Body.mass = 0.1f;

        //Disable chasing
        _octo.IsChasing = false;
        //Release octopus from player
        _octo.transform.SetParent(null);
        //Reset attack delay
        _octo.AttackCounter = 0;
        //Disable slowing of the player
        _octo.Target.GetComponent<SubMovement>().SlowDownPlayer(false);

        //Short force to get off the player
        _octo.Body.AddForce(_octo.TargetNormal * _octo.MoveSpeed / 2f);
        _octo.TentacleControl.SetState<TentacleLatchOff>();
    }

    public override void Step()
    {
        //Delay time
        if (_counter == _octo.LatchOffDelay)
        {
            _counter = 0;
            _octo.SetState<OctopusFindRock>();
        }
        else
            _counter++;
    }
}