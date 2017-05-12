using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateFlipRotation : FishState
{
    public FishStateFlipRotation(Fish pFish) : base(pFish) { }
    
    private int _count = 0;
    private Quaternion _targetRot;

    public override void Initialize()
    {
        fish.Body.velocity = Vector3.zero;
        fish.Direction = -fish.Direction;
        //Play rotation animaion
        _targetRot = fish.transform.rotation;
        _targetRot.eulerAngles = new Vector3(0f, fish.transform.eulerAngles.y - 180f, 0f);
    }

    public override void Step()
    {
        if (_count != fish.RotationTime)
        {
            fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, _targetRot, fish.RotationSpeed);
            _count++;
        }
        else
        {
            _count = 0;
            fish.SetState<FishStateContinuesMove>();
        }
    }
}