using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStatePatrol : FishState
{
    public FishStatePatrol(Fish pFish) : base(pFish) { }

    private Shark _shark;
    private int _counter;

    public override void Initialize()
    {
        _shark = (Shark)fish;
        _shark.SetRandomDirection();
        _counter = 0;
    }

    public override void Step()
    {
        fish.RotateTowards(fish.Direction);
        if (_shark.DetectTarget())
            fish.SetState<FishStatePatrolChase>();
        _shark.DetectWall();

        if (_counter == _shark.TimeForRandomDir)
        {
            _shark.SetRandomDirection();
            _counter = 0;
        }
        else
            _counter++;

        fish.Body.AddForce(fish.Direction.normalized * fish.MoveSpeed);   
    }
}