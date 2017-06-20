using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelCharge : FishState
{
    public EelCharge(Fish pFish) : base(pFish) { }

    private Eel _eel;
    private int _counter = 0;

    public override void Initialize()
    {
        _eel = (Eel)fish;
        _counter = 0;
    }

    public override void Step()
    {
        //Counter for if the Player is in a dead-zone
        if (_counter >= _eel.AttackDuration)
        {
            _eel.SetState<EelReturnToHole>();
            return;
        }
        else
            _counter++;

        //Move anchor to hole exit
        if (Vector3.Distance(_eel.HoleExit.position, _eel.Anchor.position) > 3)
        {
            _eel.Anchor.transform.position = Vector3.Lerp(_eel.Anchor.transform.position, _eel.HoleExit.position, _eel.ChargeSpeed / 10000f);
        }
        else
            _eel.AnchorBody.isKinematic = true;

        //Set direction for the head
        _eel.Direction = ((_eel.Target.position + (_eel.TargetBody.velocity / _eel.Difficulty)) - _eel.transform.position).normalized;

        if (Vector3.Distance(_eel.Target.position, _eel.OriginPos) > _eel.Range)
            _eel.SetState<EelReturnToHole>();
        
        //Move and rotate the head towards target
        _eel.RotateTowards(_eel.GetLookRotation(_eel.Direction));
        _eel.Body.AddForce(_eel.Direction * _eel.ChargeSpeed);
    }
}