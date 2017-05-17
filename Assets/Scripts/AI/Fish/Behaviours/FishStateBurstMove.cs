using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateBurstMove : FishState
{
    public FishStateBurstMove(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;

        //if (_octo.DetectTarget())
        //    fish.SetState<FishStateBurstChase>();
    }

    public override void Step()
    {
        Debug.DrawLine(_octo.transform.position, _octo.RockPos);
        if (_octo.CheckLatchOnRange())
            _octo.SetState<FishStateLatchOn>();

        Vector3 dir = (_octo.RockPos - _octo.transform.position).normalized;
        if (_octo.RotateTowards(_octo.GetLookRotation(_octo.Direction)))
        {
            _octo.Body.AddForce(_octo.Direction * _octo.MoveSpeed);
            _octo.SetState<FishStateBurstIdle>();
        }
    }
}