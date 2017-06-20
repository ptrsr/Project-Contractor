using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EelReturnToHole : FishState
{
    public EelReturnToHole(Fish pFish) : base(pFish) { }

    private Eel _eel;

    public override void Initialize()
    {
        _eel = (Eel)fish;
        //Remove any velocity to prevent eel leaving it's hole from previous forces
        _eel.Body.velocity = Vector3.zero;
        _eel.Body.angularVelocity = Vector3.zero;
        _eel.AnchorBody.velocity = Vector3.zero;
        _eel.AnchorBody.angularVelocity = Vector3.zero;
        //Disable collider for a smooth return
        _eel.Collider.enabled = false;
        _eel.AnchorBody.isKinematic = false;
    }

    public override void Step()
    {
        //Return anchor to start position
        if (Vector3.Distance(_eel.AnchorOrigPos, _eel.Anchor.position) > 1)
        {
            //Move towards anchor start position
            Vector3 dir = (_eel.AnchorOrigPos - _eel.Anchor.position).normalized;
            _eel.AnchorBody.AddForce(dir * _eel.MoveSpeed);

            //Set the direction pointing towards the hole exit position
            _eel.Direction = (_eel.HoleExit.position - _eel.Hole.position).normalized;
        }
        else
        {
            _eel.Collider.enabled = true;
            _eel.AnchorBody.isKinematic = true;
            _eel.SetState<EelHide>();
        }

        //Move all body segments to an avarage point between the body and the anchor origin pos, to keep the eel alligned
        foreach (Rigidbody body in _eel.Bodies)
        {
            float dis = Vector3.Distance(body.position, _eel.AnchorOrigPos);
            //Get pos that extends out from the anchor origin pos
            Vector3 newPos = _eel.AnchorOrigPos + _eel.Direction * dis;
            //Get the avarage pos between the new pos
            Vector3 targetPos = ((body.position + newPos) / 2f);
            Debug.DrawLine(body.position, targetPos, Color.red);
            //Calculate direction from body to targetPos
            Vector3 dir = (targetPos - body.position).normalized;
            //Move towards the targetPos
            body.AddForce(dir * _eel.MoveSpeed / 2f);
        }
    }
}