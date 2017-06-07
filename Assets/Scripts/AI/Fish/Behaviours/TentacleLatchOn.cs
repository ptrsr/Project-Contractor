using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TentacleLatchOn : FishState
{
    public TentacleLatchOn(Fish pFish) : base(pFish) { }

    private Tentacles _tentacles;
    
    private int _spreadCounter = 0;
    private int _grabCounter = 0;

    public override void Initialize()
    {
        _tentacles = (Tentacles)fish;
        _tentacles.SetCollidersActive(true);
        
        _spreadCounter = 0;
        _grabCounter = 0;

        //Temp disable Octo collider to prevent odd behaviours
        _tentacles.Octo.Collider.enabled = false;
    }

    public override void Step()
    {
        if (_spreadCounter != _tentacles.SpreadDuration)
        {
            //Time to spread the tentalces open
            for (int i = 0; i < _tentacles.TentacleTips.Length; i++)
            {
                Vector3 dir = _tentacles.TentacleTips[i].position - _tentacles.transform.position - (_tentacles.Octo.IsChasing ? _tentacles.Octo.TargetNormal * 2f : _tentacles.Octo.RockNormal * 2f);
                _tentacles.TentacleTips[i].AddForceAtPosition(dir.normalized * _tentacles.MoveSpeed * 2f, _tentacles.TentacleTips[i].transform.position);
            }

            _spreadCounter++;
        }
        else
        {
            //Re-enable Octo collider for proper behaviours
            _tentacles.Octo.Collider.enabled = true;

            //Position on the opposide of the player normal
            Vector3 pos = _tentacles.Octo.Target.position - (_tentacles.Octo.TargetNormal * _tentacles.Octo.LatchOnOffset);

            for (int i = 0; i < _tentacles.TentacleTips.Length; i++)
            {
                if (_tentacles.Octo.IsChasing)
                {
                    //Small time frame to give the tentacles a more round outcome
                    if (_grabCounter <= _tentacles.GrabDuration)
                    {
                        _tentacles.TentacleTips[i].AddForce(-_tentacles.Octo.TargetNormal * _tentacles.MoveSpeed);
                        _grabCounter++;
                    }
                    else
                    {
                        //Calculate direction vector
                        Vector3 dir = pos - _tentacles.TentacleTips[i].transform.position;

                        //Keep moving the tentacle until it reaches the spot within the given range
                        if (Vector3.Distance(_tentacles.TentacleTips[i].transform.position, pos) < _tentacles.GrabOffset)
                        {
                            _tentacles.SetKinematic(true, i);
                            _tentacles.SetCollidersActive(false);
                        }
                        else
                        {
                            //Move the tentacles towards the point direction
                            _tentacles.TentacleTips[i].AddForce(dir.normalized * _tentacles.MoveSpeed * 1.5f);
                        }
                    }
                }
                else
                    //Keep moving in the opposide direction of the normal to latch on to the ground
                    _tentacles.TentacleTips[i].AddForce(-_tentacles.Octo.RockNormal * _tentacles.MoveSpeed / 2f);
            }
        }
    }
}