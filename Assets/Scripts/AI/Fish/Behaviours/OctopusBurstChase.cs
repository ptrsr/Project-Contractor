using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusBurstChase : FishState
{
    public OctopusBurstChase(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _octo.TentacleControl.SetState<TentacleFollow>();
    }

    public override void Step()
    {
        //Return to normal movement when the target is out of range or broke line of sight
        if (Vector3.Distance(_octo.transform.position, _octo.OriginPos) > _octo.Range ||
            Physics.Linecast(_octo.transform.position, _octo.Target.position, ~_octo.IgnoreDetection))
        {
            _octo.IsChasing = false;
            _octo.SetState<OctopusFindRock>();
            return;
        }

        //Check for latch on distance and line of sight
        if (Vector3.Distance(_octo.transform.position, _octo.Target.position) < _octo.LatchOnRange &&
            !Physics.Linecast(_octo.transform.position, _octo.Target.position, ~_octo.IgnoreDetection))
        {
            _octo.SetState<OctopusLatchOnPlayer>();
            return;
        }

        //Move towards target, predicts target movement based on target's velocity
        _octo.Direction = ((_octo.Target.position + (_octo.TargetBody.velocity / _octo.Difficulty)) - _octo.transform.position).normalized;

        //Wait for rotation to be done
        if (_octo.RotateTowards(_octo.GetLookRotation(_octo.Direction), _octo.RotationModifier))
        {
            //Move towards target
            _octo.Body.AddForce(_octo.Direction * _octo.ChaseSpeed);
            _octo.SetState<OctopusBurstIdle>();
        }
    }
}