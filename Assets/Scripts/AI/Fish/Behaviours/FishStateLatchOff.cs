using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateLatchOff : FishState
{
    public FishStateLatchOff(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _octo.Body.isKinematic = false;

        if (_octo.IsChasing && Vector3.Distance(_octo.transform.position, _octo.RockPos) > 5)
        {
            _octo.IsChasing = false;
            _octo.transform.SetParent(null);
            _octo.AttackCounter = 0;
            _octo.Target.GetComponent<SubMovement>().SlowDownPlayer(false);
        }

        _octo.Body.AddForce(_octo.IsChasing ? Vector3.zero : _octo.RockNormal * _octo.MoveSpeed / 2f);
        _octo.SetState<FishStateFindRock>();
    }

    public override void Step()
    {
        //Empty
    }
}