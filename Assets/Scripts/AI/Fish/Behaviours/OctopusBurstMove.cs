using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusBurstMove : FishState
{
    public OctopusBurstMove(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _octo.Body.angularVelocity = Vector3.zero;
    }

    public override void Step()
    {
        if (_octo.DetectTarget())
            fish.SetState<OctopusBurstChase>();

        Debug.DrawLine(_octo.transform.position, _octo.RockPos);
        if (_octo.CheckLatchOnRange())
            _octo.SetState<OctopusLatchOnRock>();

        //Move towards rock
        _octo.Direction = (_octo.RockPos - _octo.transform.position).normalized;

        //Wait for rotation to be done
        if (_octo.RotateTowards(_octo.GetLookRotation(_octo.Direction)))
        {
            //Move towards direction
            _octo.Body.AddForce(_octo.Direction * _octo.MoveSpeed);
            _octo.SetState<OctopusBurstIdle>();
        }
    }
}