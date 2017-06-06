using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusSleep : FishState
{
    public OctopusSleep(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    //Sleep = 0, Aware = 1, Awake = 2
    private int _awakeCounter = 0;

    private int _awareDuration = 500;
    private int _awareCounter = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _awakeCounter = 0;
        _awareCounter = 0;
    }

    public override void Step()
    {
        
    }
}