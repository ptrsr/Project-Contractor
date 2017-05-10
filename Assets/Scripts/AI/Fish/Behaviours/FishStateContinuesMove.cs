using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateContinuesMove : FishState
{
    public FishStateContinuesMove(Fish pFish) : base(pFish) { }

    private FishEnemy _fishEnemy;

    public override void Initialize()
    {
        _fishEnemy = (FishEnemy)fish;
    }

    public override void Step()
    {
        fish.Body.AddForce(new Vector3(fish.MoveSpeed, 0, 0), ForceMode.Acceleration);
        
        Debug.DrawRay(fish.transform.position, fish.Direction * _fishEnemy.WallDetectionRange);
        if (Physics.Raycast(fish.transform.position, fish.Direction, 2f, ~_fishEnemy.IgnoreDetection))
        {
            fish.MoveSpeed = -fish.MoveSpeed;
            fish.SetState<FishStateFlipRotation>();
        }

        fish.Body.velocity = Vector3.ClampMagnitude(fish.Body.velocity, 5f);
    }
}