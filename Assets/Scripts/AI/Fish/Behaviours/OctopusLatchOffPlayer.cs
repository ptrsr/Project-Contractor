using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusLatchOffPlayer : FishState
{
    public OctopusLatchOffPlayer(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _octo.Body.isKinematic = false;

        _octo.IsChasing = false;
        _octo.transform.SetParent(null);
        _octo.AttackCounter = 0;
        _octo.Target.GetComponent<SubMovement>().SlowDownPlayer(false);

        _octo.Body.AddForce(_octo.IsChasing ? Vector3.zero : _octo.RockNormal * _octo.MoveSpeed / 2f);
        _octo.SetState<OctopusFindRock>();
    }

    public override void Step()
    {
        //Empty
    }
}