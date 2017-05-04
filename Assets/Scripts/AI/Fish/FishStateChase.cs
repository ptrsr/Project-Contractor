using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateChase : FishState
{
    public FishStateChase(Fish pFish) : base(pFish) { }

    Transform target;
    int delay = 50;
    int count = 0;

    public override void Initialize()
    {
        target = ((FishEnemy)fish).Target;
    }

    public override void Step()
    {
        fish.transform.position = new Vector3(fish.transform.position.x, fish.transform.position.y, 0);

        if (Vector3.Distance(fish.transform.position, fish.OriginPos) > 4)
            fish.SetState<FishStateMove>();

        Vector3 dir = target.position - fish.transform.position;

        if (count != delay)
        {
            Quaternion lookDir = Quaternion.LookRotation(dir);
            lookDir.eulerAngles -= new Vector3(-90, 0, 0);
            fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, lookDir, 0.15f);
            count++;
        }
        else
        {
            fish.Body.AddForce(dir.normalized * 25, ForceMode.Force);

            count = 0;

            fish.SetState<FishStateIdle>();
        }
    }
}