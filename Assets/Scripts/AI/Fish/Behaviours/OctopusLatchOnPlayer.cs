using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OctopusLatchOnPlayer : FishState
{
    public OctopusLatchOnPlayer(Fish pFish) : base(pFish) { }

    private Octopus _octo;
    private SubMovement _subMove;
    private int _rotCounter = 0;

    public override void Initialize()
    {
        _octo = (Octopus)fish;
        _rotCounter = 0;

        //Parent the player to keep equal positions
        _octo.transform.SetParent(_octo.Target.transform);
        RaycastHit hit;
        Physics.Raycast(_octo.transform.position, _octo.Direction, out hit);
        _octo.TargetNormal = hit.normal;
        _subMove = _octo.Target.GetComponent<SubMovement>();
        //Slow down the player
        _subMove.SlowDownPlayer(true);
        //Prevents odd behaviours
        _octo.Body.isKinematic = true;
        //Stun the player to make it less possible for odd movements and instant escapes
        _octo.Target.GetComponent<SubMovement>().StunPlayer();
    }

    public override void Step()
    {
        if (_subMove.Charged)
            _octo.SetState<OctopusLatchOffPlayer>();

        SetPos(_octo.Target.position, _octo.TargetNormal);
        SetRot(_octo.TargetNormal);
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
            _octo.TentacleControl.SetState<TentacleLatchOn>();
        }
    }
}