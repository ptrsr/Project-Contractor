using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateLatchOn : FishState
{
    public FishStateLatchOn(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    private int _counter = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _counter = 0;

        if (_octo.IsChasing)
            fish.transform.SetParent(_octo.Target.transform);

        fish.Body.isKinematic = true;
        _octo.Collider.enabled = false;
    }

    public override void Step()
    {
        if (!_octo.IsChasing)
        {
            if (_octo.DetectTarget())
                _octo.SetState<FishStateBurstChase>();

            SetPos(_octo.Target.position, _octo.TargetNormal);
            SetRot(_octo.TargetNormal);

            if (_counter != _octo.RestTime)
                _counter++;
            else
                _octo.SetState<FishStateLatchOff>();
        }
        else
        {
            SetPos(_octo.Target.position, _octo.TargetNormal);
        }
    }

    private void SetPos(Vector3 target, Vector3 normal)
    {
        _octo.transform.position = Vector3.Lerp(_octo.transform.position, target + (normal * _octo.LatchOnOffset), _octo.RotationSpeed);
    }

    private void SetRot(Vector3 normal)
    {
        _octo.transform.rotation = Quaternion.Lerp(_octo.transform.rotation, _octo.GetLatchOnRot(normal), _octo.RotationSpeed);
    }
}