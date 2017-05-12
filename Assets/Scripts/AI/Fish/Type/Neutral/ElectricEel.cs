using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ElectricEel : FishNeutral
{
    [Header("Electric Eel Variables")]
    [SerializeField]
    private float _knockbackStrength = 100f;

    [SerializeField]
    private float _knockUpStrength = 25f;

    public override void Start()
    {
        base.Start();

        Direction = (Vector3.right * WallDetectionRange).normalized;

        stateCache[typeof(FishStateContinuesMove)] = new FishStateContinuesMove(this);
        stateCache[typeof(FishStateFlipRotation)] = new FishStateFlipRotation(this);

        SetState<FishStateContinuesMove>();
    }

    public override void Update()
    {
        BindZ();

        base.Update();
    }

    public void OnCollisionEnter(Collision c)
    {
        if (c.rigidbody == null)
            return;

        //Stun player

        c.rigidbody.AddForce(new Vector3(Direction.x * _knockbackStrength, _knockUpStrength), ForceMode.Impulse);
    }
}