using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SharkIdle : FishState
{
    public SharkIdle(Fish pFish) : base(pFish) { }

    private int _counter = 0;

    public override void Initialize()
    {
        //Empty
    }

    public override void Step()
    {
        if (_counter != fish.IdleTime)
        {
            _counter++;
        }
        else
        {
            _counter = 0;
            fish.SetState<SharkReturn>();
        }
    }
}