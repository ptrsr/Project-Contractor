using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateRest : FishState
{
    public FishStateRest(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
    }

    public override void Step()
    {
        
    }
}