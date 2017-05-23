using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkChase : FishState
{
    public SharkChase(Fish pFish) : base(pFish) { }

    private Shark _shark;

    public override void Initialize()
    {
        _shark = (Shark)fish;
    }

    public override void Step()
    {
        if (!_shark.DetectTarget())
            _shark.SetState<SharkReturn>();
        
        //Move and rotate towards the tharget
        _shark.Direction = ((_shark.Target.transform.position + (_shark.TargetBody.velocity / _shark.Difficulty)) - _shark.transform.position).normalized;
        _shark.RotateTowards(_shark.GetLookRotation(_shark.Direction), _shark.RotationModifier);
        _shark.Body.AddForce(_shark.Direction * _shark.ChaseSpeed);
    }
}