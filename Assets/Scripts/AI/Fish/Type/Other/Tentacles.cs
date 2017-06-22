using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tentacles : Fish
{
    [Header("Tentacle Variables")]
    [SerializeField]
    private Rigidbody[] _tentacleTips = null;
    public Rigidbody[] TentacleTips { get { return _tentacleTips; } }

    private Octopus _octo;
    public Octopus Octo { get { return _octo; } }

    [SerializeField]
    [Tooltip("The time it takes for the tentacles to attempt to spread")]
    private int _spreadDuration = 20;
    public int SpreadDuration { get { return _spreadDuration; } }

    [SerializeField]
    [Tooltip("A small time frame to give the grab a more round outcome")]
    private int _grabDuration = 50;
    public int GrabDuration { get { return _grabDuration; } }

    [SerializeField]
    private float _grabOffset = 2.5f;
    public float GrabOffset { get { return _grabOffset; } }

    private Collider[] _colliders;
    List<Rigidbody> _tentacleBodies;


    public override void Start()
    {
        base.Start();

        _octo = GetComponent<Octopus>();
        _colliders = GetComponentsInChildren<SphereCollider>();
        _tentacleBodies = GetComponentsInChildren<Rigidbody>().ToList();
        _tentacleBodies.RemoveRange(0, 2);

        stateCache[typeof(TentacleFollow)] = new TentacleFollow(this);
        stateCache[typeof(TentacleLatchOn)] = new TentacleLatchOn(this);
        stateCache[typeof(TentacleLatchOff)] = new TentacleLatchOff(this);

        SetState<TentacleFollow>();
    }

    public void SetCollidersActive(bool value)
    {
        //Set collider value for each collider
        foreach (Collider col in _colliders)
        {
            col.enabled = value;
        }
    }

    public void SetKinematic(bool value)
    {
        //Set all tentacle rigidbodies kinematic values
        foreach (Rigidbody body in _tentacleBodies)
        {
            body.isKinematic = value;
        }
    }

    public void SetKinematic(bool value, int id)
    {
        //Return if it's already kinematic
        if (_tentacleTips[id].isKinematic)
            return;

        //Set kinemtic value
        _tentacleTips[id].isKinematic = value;

        //Set the remaining rigidbodies to the same kinematic value
        SetKinematic(value, _tentacleTips[id].transform.parent, 0);
    }

    private void SetKinematic(bool value, Transform parent, int counter)
    {
        //Check when it reaches the highest parent
        if (counter == 6)
            return;

        //Set rigidbody value
        parent.GetComponent<Rigidbody>().isKinematic = true;
        //Keep doing the same until it reaches the highest parent
        SetKinematic(value, parent.parent, counter + 1);
    }
}