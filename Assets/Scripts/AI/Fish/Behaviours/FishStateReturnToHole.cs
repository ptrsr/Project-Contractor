using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateReturnToHole : FishState
{
    public FishStateReturnToHole(Fish pFish) : base(pFish) { }

    private ElectricEel _eel;

    private bool _isAtExit = false;

    public override void Initialize()
    {
        _eel = (ElectricEel)fish;
        _eel.Collider.enabled = false;
        _isAtExit = false;
    }

    public override void Step()
    {
        if (Vector3.Distance(_eel.HoleExit.position, _eel.transform.position) > 5 && !_isAtExit)
        {
            _eel.Direction = (_eel.HoleExit.position - _eel.transform.position).normalized;
        }
        else
        {
            _isAtExit = true;

            if (Vector3.Distance(_eel.OriginPos, _eel.transform.position) > 5)
                _eel.Direction = (_eel.OriginPos - _eel.transform.position).normalized;
            else
                _eel.SetState<FishStateHide>();
        }
        
        _eel.RotateTowards(_eel.GetLookRotation(_eel.Direction));
        _eel.Body.AddForce(_eel.Direction * _eel.MoveSpeed);
    }
}