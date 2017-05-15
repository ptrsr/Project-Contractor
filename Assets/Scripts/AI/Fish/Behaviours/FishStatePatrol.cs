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
        fish.RotateTowards(_shark.GetLookRotation(fish.Direction));
        if (_shark.DetectTarget())
            fish.SetState<FishStatePatrolChase>();

        //Check for walls and solve collision issues
        if (_shark.DetectWall())
            _counter = 0;

        //Counting till next direction
        if (_counter == _shark.TimeForRandomDir)
        {
            _shark.SetRandomDirection();
            _counter = 0;
        }
        else
            _counter++;

        //Move in direction
        fish.Body.AddForce(_shark.Direction * fish.MoveSpeed);   
    }
}