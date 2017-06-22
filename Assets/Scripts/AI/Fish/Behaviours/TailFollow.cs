using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class TailFollow : FishState
{
    public TailFollow(Fish pFish) : base(pFish) { }

    private SharkTail _tail;

    private int _id = 0;

    private float _headWobble = -1f;
    private float _normalCap = 1f;
    private float _chaseCap = 0.5f;
    private float _changeVal = 0.1f;

    public override void Initialize()
    {
        _tail = (SharkTail)fish;
    }

    public override void Step()
    {
        //Head counter
        _headWobble += _changeVal;

        //Switch direction on cap values
        if (_headWobble <= (_tail.SharkValues.IsChasing ? -_chaseCap : -_normalCap))
        {
            _headWobble = _tail.SharkValues.IsChasing ? -_chaseCap : -_normalCap;
            _changeVal = -_changeVal;
        }
        else if (_headWobble >= (_tail.SharkValues.IsChasing ? _chaseCap : _normalCap))
        {
            _headWobble = _tail.SharkValues.IsChasing ? _chaseCap : _normalCap;
            _changeVal = -_changeVal;
        }

        //Add torque
        _tail.Head.AddTorque(0, _tail.SharkValues.IsChasing ? _headWobble * _tail.ChaseSpeed : _headWobble * _tail.MoveSpeed, 0, ForceMode.Acceleration);

        //Keep original rotation to prevent odd rotated tail
        _tail.Tail.rotation = Quaternion.Slerp(_tail.Tail.rotation, _tail.OriginRot, _tail.RotationSpeed);

        //Switch between left and right movement
        if (Vector3.Distance(_tail.Tail.position, _tail.Positions[_id].position) < 6.4f)
            _id = _id == 0 ? 1 : 0;

        //Move towards swimming point
        _tail.Direction = (_tail.Positions[_id].position - _tail.Tail.position).normalized;
        _tail.Tail.AddForce(_tail.Direction * (_tail.SharkValues.IsChasing ? _tail.ChaseSpeed : _tail.MoveSpeed));
    }
}