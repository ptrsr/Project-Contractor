using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateLatchOn : FishState
{
    public FishStateLatchOn(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    private SubMovement _subMove;
    private int _restCounter = 0;
    private int _rotCounter = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _restCounter = 0;
        _rotCounter = 0;

        if (_octo.IsChasing)
        {
            //Parent the player to keep equal positions
            _octo.transform.SetParent(_octo.Target.transform);
            RaycastHit hit;
            Physics.Raycast(_octo.transform.position, _octo.Direction, out hit);
            _octo.TargetNormal = hit.normal;
            _subMove = _octo.Target.GetComponent<SubMovement>();
            //Slow down the player
            _subMove.SlowDownPlayer(true);
        }

        _octo.Body.isKinematic = true;
        _octo.Collider.enabled = false;
    }

    public override void Step()
    {
        if (!_octo.IsChasing)
        {
            if (_octo.DetectTarget())
                _octo.SetState<FishStateLatchOff>();

            SetPos(_octo.RockPos, _octo.RockNormal);
            SetRot(_octo.RockNormal);

            if (_restCounter != _octo.RestTime)
                _restCounter++;
            else
                _octo.SetState<FishStateLatchOff>();
        }
        else
        {
            if (_subMove.Charged)
                _octo.SetState<FishStateLatchOff>();

            if (_octo.transform.parent == null)
                return;

            SetPos(_octo.Target.position, _octo.TargetNormal);
            SetRot(_octo.TargetNormal);
        }
    }

    private void SetPos(Vector3 target, Vector3 normal)
    {
        _octo.transform.position = Vector3.Lerp(_octo.transform.position, target + (normal * _octo.LatchOnOffset), _octo.RotationSpeed);
    }

    private void SetRot(Vector3 normal)
    {
        if (_rotCounter != _octo.RotationTime)
        {
            _octo.transform.rotation = Quaternion.Lerp(_octo.transform.rotation, _octo.GetLatchOnRot(normal), _octo.RotationSpeed);
            _rotCounter++;
        }
    }
}