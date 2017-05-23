using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateReturnToHole : FishState
{
    public FishStateReturnToHole(Fish pFish) : base(pFish) { }

    private ElectricEel _eel;

    public override void Initialize()
    {
        _eel = (ElectricEel)fish;
        _eel.Body.velocity = Vector3.zero;
        _eel.Body.angularVelocity = Vector3.zero;
        _eel.Collider.enabled = false;
        _eel.AnchorBody.isKinematic = false;
    }

    public override void Step()
    {
        //Prevent anchor from leaving hole exit
        if ((_eel.HoleExit.position - _eel.Anchor.position).x < 0)
            _eel.Anchor.position = new Vector3(_eel.HoleExit.position.x, _eel.Anchor.position.y, 0);

        //Return anchor to start position
        if (Vector3.Distance(_eel.AnchorOrigPos, _eel.Anchor.position) > 5)
        {
            Vector3 dir = (_eel.AnchorOrigPos - _eel.Anchor.position).normalized;
            _eel.AnchorBody.AddForce(dir * _eel.MoveSpeed);

            _eel.Direction = (_eel.HoleExit.position - _eel.transform.position).normalized;
        }
        else
            _eel.SetState<FishStateHide>();
        
        //Only move the y force to prevent curling up behaviour
        _eel.Body.AddForce(new Vector3(0, _eel.Direction.y, 0) * _eel.MoveSpeed / 2f);
    }
}