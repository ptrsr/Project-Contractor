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
        //Find a position to latch on the player
        RaycastHit hit;
        Physics.Raycast(_octo.transform.position, _octo.Direction, out hit);
        //Assign found value
        _octo.TargetNormal = hit.normal;
        //Get movement script
        _subMove = _octo.Target.GetComponent<SubMovement>();

        _subMove.PlayParticles();

        //Pre-check if player is charging, so you can avoid the Octopus quickly
        if (_subMove.Charged)
            _octo.SetState<OctopusLatchOffPlayer>();
        else
        {
            _octo.Collider.enabled = false;
            //Slow down the player
            _subMove.SlowDownPlayer(true);
            //Prevents odd behaviours
            _octo.Body.isKinematic = true;
            //Stun the player to make it less possible for odd movements and instant escapes
            _subMove.StunSlowCooldown = 120;
            _subMove.StunPlayer();
        }
    }

    public override void Step()
    {
        if (_subMove.Charged)
            _octo.SetState<OctopusLatchOffPlayer>();

        if (Vector3.Distance(_octo.transform.position, _octo.OriginPos) > _octo.Range)
            _octo.SetState<OctopusLatchOffPlayer>();

        //Drain oxygen
        _octo.OxygenVals.Remove(_octo.OxygenDrain);

        //Set position and rotation
        SetPos(_octo.Target.position, _octo.TargetNormal);
        SetRot(_octo.TargetNormal);
    }

    private void SetPos(Vector3 target, Vector3 normal)
    {
        _octo.transform.position = Vector3.Lerp(_octo.transform.position, target + (normal * _octo.LatchOnOffset), _octo.RotationSpeed * 2f);
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