using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateLatchOn : FishState
{
    public FishStateLatchOn(Fish pFish) : base(pFish) { }

    private FishEnemy _fishEnemy;

    public override void Initialize()
    {
        _fishEnemy = (FishEnemy)fish;
        fish.transform.SetParent(_fishEnemy.Target.transform);
        fish.Body.isKinematic = true;
        fish.GetComponent<Collider>().enabled = false;
    }

    public override void Step()
    {
        //fish.transform.position = _fishEnemy.Target.transform.position + new Vector3(0f, 0f, -1f);

        //if (_fishEnemy.Target.GetComponent<ZBound>().tap)
        //    fish.SetState<FishStateLatchOff>();
    }
}