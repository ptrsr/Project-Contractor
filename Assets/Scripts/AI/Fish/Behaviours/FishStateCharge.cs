using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateCharge : FishState
{
    public FishStateCharge(Fish pFish) : base(pFish) { }

    private ElectricEel _eel;

    private bool _reachedExit = false;

    public override void Initialize()
    {
        _eel = (ElectricEel)fish;
        _reachedExit = false;
    }

    public override void Step()
    {
        if (Vector3.Distance(_eel.HoleExit.position, _eel.transform.position) > 2 && !_reachedExit)
        {
            _eel.Direction = (_eel.HoleExit.position - _eel.transform.position).normalized;
        }
        else
        {
            _reachedExit = true;
            _eel.Direction = (_eel.Target.position - _eel.transform.position).normalized;
        }

        if (Vector3.Distance(_eel.Target.position, _eel.OriginPos) > _eel.DetectionRange)
        {
            _eel.SetState<FishStateReturnToHole>();
        }
        
        _eel.RotateTowards(_eel.GetLookRotation(_eel.Direction));
        _eel.Body.AddForce(_eel.Direction * _eel.MoveSpeed);
    }
}