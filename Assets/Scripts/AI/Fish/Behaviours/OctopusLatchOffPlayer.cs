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

        _octo.IsChasing = false;
        _octo.transform.SetParent(null);
        _octo.AttackCounter = 0;
        _octo.Target.GetComponent<SubMovement>().SlowDownPlayer(false);

        _octo.Body.AddForce(_octo.IsChasing ? Vector3.zero : _octo.RockNormal * _octo.MoveSpeed / 2f);
        _octo.TentacleControl.SetState<TentacleLatchOff>();
    }

    public override void Step()
    {
        if (_counter == _octo.LatchOffDelay)
        {
            _counter = 0;
            _octo.SetState<OctopusFindRock>();
        }
        else
            _counter++;
    }
}