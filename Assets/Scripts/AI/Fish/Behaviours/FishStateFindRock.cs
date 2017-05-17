using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateFindRock : FishState
{
    public FishStateFindRock(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    int i = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
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

        do
        {
            _octo.SetRandomDirection();
            Debug.DrawRay(_octo.transform.position, _octo.Direction * _octo.Range, Color.yellow, 2f);
            Physics.Raycast(_octo.transform.position, _octo.Direction, out hit, _octo.Range, ~_octo.IgnoreDetection);

            i++;
            if (i >= 100)
            {
                Debug.Log("Failed to find a rock");
                break;
            }
        } while (!(Vector3.Distance(_octo.OriginPos, hit.point) < _octo.Range));

        _octo.RockPos = hit.point;
        _octo.RockNormal = hit.normal;
    }
}