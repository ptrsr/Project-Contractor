using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateFindRock : FishState
{
    public FishStateFindRock(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    private int i = 0;

    public override void Initialize()
    {
        i = 0;
        _octo = (Octopus)fish;
        _octo.Collider.enabled = true;
        FindRock();
        _octo.SetState<FishStateBurstMove>();
    }

    public override void Step()
    {
        //Empty
    }

    private void FindRock()
    {
        RaycastHit hit;

        //Attempt to find a rock within range
        do
        {
            _octo.SetRandomDirection();
            Debug.DrawRay(_octo.transform.position, _octo.Direction * _octo.Range, Color.yellow, 2f);
            Physics.Raycast(_octo.transform.position, _octo.Direction, out hit, _octo.Range * 2f, ~_octo.IgnoreDetection);
            i++;
            if (i >= 100)
            {
                Debug.Log("Failed to find a rock");
                break;
            }
            //Keep trying until the point is in range of origin and is not right next to the Octopus
        } while
        (Vector3.Distance(_octo.OriginPos, hit.point) > _octo.Range ||
        Vector3.Distance(_octo.transform.position, hit.point) < 20);

        _octo.RockPos = hit.point;
        _octo.RockNormal = hit.normal;
    }
}