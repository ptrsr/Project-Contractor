using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusBurstIdle : FishState
{
    public OctopusBurstIdle(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    private int _count = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
    }

    public override void Step()
    {
        if (_octo.IsChasing && _octo.CheckLatchOnRange())
            _octo.SetState<OctopusLatchOnPlayer>();
        else if (!_octo.IsChasing && _octo.CheckLatchOnRange())
            _octo.SetState<OctopusLatchOnRock>();

        //Idle time
        if (_count < (_octo.IsChasing ? fish.IdleTime / _octo.IdleIntervalChange : fish.IdleTime))
        {
            _octo.Body.AddForce(-_octo.Direction / 1.2f);
            _count++;
        }
        else
        {
            _count = 0;
            if (_octo.IsChasing)//_octo.DetectTarget())
                _octo.SetState<OctopusBurstChase>();
            else
                _octo.SetState<OctopusBurstMove>();
        }
    }
}