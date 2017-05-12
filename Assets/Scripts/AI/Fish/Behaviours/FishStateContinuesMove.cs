using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateContinuesMove : FishState
{
    public FishStateContinuesMove(Fish pFish) : base(pFish) { }

    private ElectricEel _eel;

    public override void Initialize()
    {
        _eel = (ElectricEel)fish;
    }

    public override void Step()
    {
        fish.RotateTowards(fish.Direction - new Vector3(0f, 0f, fish.Direction.x > 0 ? -90f : 90f));

        Debug.DrawRay(fish.transform.position, fish.Direction * _eel.WallDetectionRange);
        if (Physics.Raycast(fish.transform.position, fish.Direction, _eel.WallDetectionRange, ~_eel.IgnoreDetection))
        {
            fish.MoveSpeed = -fish.MoveSpeed;
            fish.SetState<FishStateFlipRotation>();
        }

        fish.Body.AddForce(new Vector3(fish.MoveSpeed, 0, 0), ForceMode.Acceleration);

        fish.Body.velocity = Vector3.ClampMagnitude(fish.Body.velocity, 5f);
    }
}