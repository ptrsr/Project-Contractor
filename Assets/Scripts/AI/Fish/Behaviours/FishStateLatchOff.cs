using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateLatchOff : FishState
{
    public FishStateLatchOff(Fish pFish) : base(pFish) { }

    public override void Initialize()
    {
        fish.transform.SetParent(null);
        fish.Body.isKinematic = false;
        fish.GetComponent<Collider>().enabled = true;
        fish.Body.AddForce(-((FishEnemy)fish).Target.GetComponent<Rigidbody>().velocity, ForceMode.Impulse);
        //Animation for swimming away
    }

    public override void Step()
    {
        
    }
}