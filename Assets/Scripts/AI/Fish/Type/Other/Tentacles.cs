using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Tentacles : Fish
{
    [SerializeField]
    private Rigidbody[] _tentacleTips = null;
    public Rigidbody[] TentacleTips { get { return _tentacleTips; } }

    private Octopus _octo;
    public Octopus Octo { get { return _octo; } }

    [SerializeField]
    [Tooltip("The time it takes for the tentacles to attempt to spread")]
    private int _spreadDuration = 20;
    public int SpreadDuration { get { return _spreadDuration; } }
    private int _spreadCounter = 0;

    [SerializeField]
    [Tooltip("A small time frame to give the grab a more round outcome")]
    private int _grabDuration = 50;
    public int GrabDuration { get { return _grabDuration; } }

    [SerializeField]
    private float _grabOffset = 2.5f;
    public float GrabOffset { get { return _grabOffset; } }


    public override void Start()
    {
        base.Start();

        _octo = GetComponent<Octopus>();

        stateCache[typeof(TentacleFollow)] = new TentacleFollow(this);
        stateCache[typeof(TentacleLatchOn)] = new TentacleLatchOn(this);
        stateCache[typeof(TentacleLatchOff)] = new TentacleLatchOff(this);

        SetState<TentacleFollow>();
    }

    public void SetCollidersActive(bool value)
    {
        Collider[] cols = GetComponentsInChildren<SphereCollider>();
        foreach (Collider col in cols)
        {
            col.enabled = value;
        }
    }

    public void SetKinematic(bool value)
    {
        List<Rigidbody> bodies = GetComponentsInChildren<Rigidbody>().ToList();
        bodies.RemoveRange(0, 2);

        foreach (Rigidbody body in bodies)
        {
            body.isKinematic = value;
        }
        //foreach (Rigidbody tentacle in _tentacleTips)
        //{
        //    tentacle.isKinematic = value;
        //}
    }

    public void SetKinematic(bool value, int id)
    {
        if (_tentacleTips[id].isKinematic)
            return;

        _tentacleTips[id].isKinematic = value;

        SetKinematic(value, _tentacleTips[id].transform.parent, 0);
    }

    private void SetKinematic(bool value, Transform parent, int counter)
    {
        if (counter == 6)
            return;

        parent.GetComponent<Rigidbody>().isKinematic = true;
        SetKinematic(value, parent.parent, counter + 1);
    }
}