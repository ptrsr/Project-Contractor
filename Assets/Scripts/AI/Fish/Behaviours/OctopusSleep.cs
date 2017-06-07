using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusSleep : FishState
{
    public OctopusSleep(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    
    private int _awareCounter = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _octo.Body.isKinematic = false;
        _octo.Body.mass = 2000;
        _octo.AwakeCounter = 0;
    }

    public override void Step()
    {
        //Prevent the body from sleeping
        _octo.Body.WakeUp();

        //Becomes aware
        if (_octo.AwakeCounter == 1)
        {
            if (_awareCounter == _octo.AwareDuration)
            {
                //Go back to sleep
                _awareCounter = 0;
                _octo.AwakeCounter = 0;
            }
            else
                _awareCounter++;
        }
        //Awakes and chases
        else if (_octo.AwakeCounter == 2)
        {
            _octo.IsChasing = true;
            _octo.SetState<OctopusLatchOffRock>();
        }
    }
}