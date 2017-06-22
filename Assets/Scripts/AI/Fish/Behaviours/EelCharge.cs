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
            _eel.Anchor.transform.position = Vector3.Lerp(_eel.Anchor.transform.position, _eel.HoleExit.position, _eel.MoveSpeed / 8000f);
        }
        else
            _eel.AnchorBody.isKinematic = true;

        //Move all body segments to an avarage point between the body and the anchor origin pos, to keep the eel alligned
        foreach (Rigidbody body in _eel.Bodies)
        {
            float dis = Vector3.Distance(body.position, _eel.HoleExit.position);
            //Get pos that extends out from the anchor origin pos
            Vector3 newPos = _eel.HoleExit.position + _eel.Direction * dis;
            //Get the avarage pos between the new pos
            Vector3 targetPos = ((body.position + newPos) / 2f);
            Debug.DrawLine(body.position, targetPos, Color.red);
            //Calculate direction from body to targetPos
            Vector3 dir = (targetPos - body.position).normalized;
            //Move towards the targetPos
            body.AddForce(dir * _eel.MoveSpeed / 2f);
        }

        //Set direction for the head
        _eel.Direction = ((_eel.Target.position + (_eel.TargetBody.velocity / _eel.Difficulty)) - _eel.transform.position).normalized;
        //Move and rotate the head towards target
        _eel.Body.AddForce(_eel.Direction * _eel.ChargeSpeed);

        if (Vector3.Distance(_eel.Target.position, _eel.OriginPos) > _eel.Range)
            _eel.SetState<EelReturnToHole>();
        
    }
}