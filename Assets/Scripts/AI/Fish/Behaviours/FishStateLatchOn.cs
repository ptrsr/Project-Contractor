using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateLatchOn : FishState
{
    public FishStateLatchOn(Fish pFish) : base(pFish) { }

    private Octopus _octo;

    public override void Initialize()
    {
        _octo = (Octopus)fish;

        if (_octo.IsChasing)
            fish.transform.SetParent(_octo.Target.transform);

        fish.Body.isKinematic = true;
        _octo.Collider.enabled = false;
    }

    public override void Step()
    {
        if (!_octo.IsChasing)
        {
            _octo.transform.position = Vector3.Lerp(_octo.transform.position, _octo.RockPos + (_octo.RockNormal * 5f), _octo.RotationSpeed);
            _octo.transform.rotation = Quaternion.Lerp(_octo.transform.rotation, _octo.GetLatchOnRot(_octo.RockNormal), _octo.RotationSpeed);
        }
    }
}