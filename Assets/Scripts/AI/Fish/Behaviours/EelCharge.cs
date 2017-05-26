using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelCharge : FishState
{
    public EelCharge(Fish pFish) : base(pFish) { }

    private ElectricEel _eel;

    public override void Initialize()
    {
        _eel = (ElectricEel)fish;
    }

    public override void Step()
    {
        //Move anchor to hole exit
        if (Vector3.Distance(_eel.HoleExit.position, _eel.Anchor.position) > 0)
        {
            Vector3 dir = (_eel.HoleExit.position - _eel.Anchor.position).normalized * _eel.ChargeSpeed;
            _eel.AnchorBody.AddForce(dir);
        }
        else
            _eel.AnchorBody.isKinematic = true;

        //Set direction for the head
        if (Vector3.Distance(_eel.AnchorOrigPos, _eel.Anchor.position) > 4)
            _eel.Direction = ((_eel.Target.position + (_eel.TargetBody.velocity / _eel.Difficulty)) - _eel.transform.position).normalized;

        if (Vector3.Distance(_eel.Target.position, _eel.OriginPos) > _eel.DetectionRange)
            _eel.SetState<EelReturnToHole>();
        
        //Move and rotate the head towards target
        _eel.RotateTowards(_eel.GetLookRotation(_eel.Direction));
        _eel.Body.AddForce(_eel.Direction * _eel.ChargeSpeed);
    }
}