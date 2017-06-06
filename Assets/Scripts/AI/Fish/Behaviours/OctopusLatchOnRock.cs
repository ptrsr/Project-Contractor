using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusLatchOnRock : FishState
{
    public OctopusLatchOnRock(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    private int _restCounter = 0;
    private int _rotCounter = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _restCounter = 0;
        _rotCounter = 0;

        _octo.Collider.enabled = true;
        _octo.Body.isKinematic = true;
    }

    public override void Step()
    {
        if (_octo.DetectTarget())
            _octo.SetState<OctopusLatchOffRock>();

        SetPos(_octo.RockPos, _octo.RockNormal);
        SetRot(_octo.RockNormal);

        if (_restCounter != _octo.RestTime / 4)
            _restCounter++;
        else
            //_octo.SetState<OctopusLatchOffRock>();
            _octo.SetState<OctopusSleep>();
    }

    private void SetPos(Vector3 target, Vector3 normal)
    {
        _octo.transform.position = Vector3.Lerp(_octo.transform.position, target + (normal * _octo.LatchOnOffset), _octo.RotationSpeed / 2f);
    }

    private void SetRot(Vector3 normal)
    {
        if (_rotCounter != _octo.RotationTime)
        {
            _octo.transform.rotation = Quaternion.Lerp(_octo.transform.rotation, _octo.GetLatchOnRot(normal), _octo.RotationSpeed);
            _rotCounter++;
            _octo.TentacleControl.SetState<TentacleLatchOn>();
        }
    }
}