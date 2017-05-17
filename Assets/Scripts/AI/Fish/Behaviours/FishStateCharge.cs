﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateCharge : FishState
{
    public FishStateCharge(Fish pFish) : base(pFish) { }

    private ElectricEel _eel;

    public override void Initialize()
    {
        _eel = (ElectricEel)fish;
    }

    public override void Step()
    {
        if (Vector3.Distance(_eel.HoleExit.position, _eel.Anchor.position) > 0)
        {
            Vector3 dir = (_eel.HoleExit.position - _eel.Anchor.position).normalized * _eel.ChargeSpeed;
            _eel.AnchorBody.AddForce(dir);
        }
        else
        {
            _eel.AnchorBody.isKinematic = true;
        }

        if (Vector3.Distance(_eel.AnchorOrigPos, _eel.Anchor.position) > 4)
            _eel.Direction = (_eel.Target.position - _eel.transform.position).normalized;

        if (Vector3.Distance(_eel.Target.position, _eel.OriginPos) > _eel.DetectionRange)
        {
            _eel.SetState<FishStateReturnToHole>();
        }
        
        _eel.RotateTowards(_eel.GetLookRotation(_eel.Direction));
        _eel.Body.AddForce(_eel.Direction * _eel.ChargeSpeed);
    }
}