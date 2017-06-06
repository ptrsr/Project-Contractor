using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleLatchOff : FishState
{
    public TentacleLatchOff(Fish pFish) : base(pFish) { }

    private Tentacles _tentacles;

    public override void Initialize()
    {
        _tentacles = (Tentacles)fish;
        _tentacles.SetCollidersActive(false);
        _tentacles.SetKinematic(false);
    }

    public override void Step()
    {
        //Empty
    }
}