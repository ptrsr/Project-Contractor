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
        if (Vector3.Distance(_octo.transform.position, _octo.OriginPos) > _octo.Range)
            _octo.SetState<OctopusBurstMove>();

        if (Vector3.Distance(_octo.transform.position, _octo.Target.position) < _octo.LatchOnRange)
            _octo.SetState<OctopusLatchOnPlayer>();

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