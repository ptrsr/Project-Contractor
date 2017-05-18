using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateBurstChase : FishState
{
    public FishStateBurstChase(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
    }

    public override void Step()
    {
        if (Vector3.Distance(_octo.transform.position, _octo.OriginPos) > _octo.Range)
            _octo.SetState<FishStateBurstMove>();

        if (Vector3.Distance(_octo.transform.position, _octo.Target.position) < _octo.LatchOnRange)
            _octo.SetState<FishStateLatchOn>();

        _octo.Direction = (_octo.Target.position - _octo.transform.position).normalized;

        if (_octo.RotateTowards(_octo.GetLookRotation(_octo.Direction), _octo.RotationModifier))
        {
            _octo.Body.AddForce(_octo.Direction * _octo.ChaseSpeed);
            _octo.SetState<FishStateBurstIdle>();
        }
    }
}