using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TailIdle : FishState
{
    public TailIdle(Fish pFish) : base(pFish) { }

    private SharkTail _tail;
    private int _counter = 0;

    public override void Initialize()
    {
        _tail = (SharkTail)fish;
    }

    public override void Step()
    {
        if (_counter >= _tail.IdleTime)
        {
            _counter = 0;
            _tail.SetState<TailFollow>();
        }
        else
            _counter++;
    }
}