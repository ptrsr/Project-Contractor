using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateHide : FishState
{
    public FishStateHide(Fish pFish) : base(pFish) { }

    private ElectricEel _eel;

    public override void Initialize()
    {
        _eel = (ElectricEel)fish;
        _eel.Body.velocity = Vector3.zero;
        _eel.Direction = (_eel.HoleExit.position - _eel.Hole.position).normalized;
        _eel.Collider.enabled = true;
    }

    public override void Step()
    {
        if (Vector3.Distance(_eel.Target.position, _eel.OriginPos) < _eel.DetectionRange)
        {
            _eel.SetState<FishStateCharge>();
        }

        _eel.transform.position = _eel.OriginPos;
        _eel.transform.rotation = _eel.OriginRot;
    }
}