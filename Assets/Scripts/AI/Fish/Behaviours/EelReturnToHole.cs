using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelReturnToHole : FishState
{
    public EelReturnToHole(Fish pFish) : base(pFish) { }

    private ElectricEel _eel;
    private Vector3 _holeDir;

    public override void Initialize()
    {
        _eel = (ElectricEel)fish;
        _eel.Body.velocity = Vector3.zero;
        _eel.Body.angularVelocity = Vector3.zero;
        _eel.AnchorBody.velocity = Vector3.zero;
        _eel.AnchorBody.angularVelocity = Vector3.zero;
        _eel.AnchorBody.isKinematic = false;

        _holeDir = _eel.HoleExit.position - _eel.AnchorOrigPos;
    }

    public override void Step()
    {
        //Return anchor to start position
        if (Vector3.Distance(_eel.AnchorOrigPos, _eel.Anchor.position) > 5)
        {
            Vector3 dir = (_eel.AnchorOrigPos - _eel.Anchor.position).normalized;
            _eel.AnchorBody.AddForce(dir * _eel.MoveSpeed);

            _eel.Direction = (_eel.HoleExit.position - _eel.transform.position).normalized;
        }
        else
            _eel.SetState<EelHide>();

        //Only move one force to prevent curling up behaviour
        if (_eel.AnchorBody.constraints == _eel.XCons)
            _eel.Body.AddForce(new Vector3(0, _eel.Direction.y, 0) * _eel.MoveSpeed / 2f);
        else if (_eel.AnchorBody.constraints == _eel.YCons)
            _eel.Body.AddForce(new Vector3(_eel.Direction.x, 0, 0) * _eel.MoveSpeed / 2f);
    }
}