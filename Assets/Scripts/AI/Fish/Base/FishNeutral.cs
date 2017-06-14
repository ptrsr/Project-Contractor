using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNeutral : Fish
{
    [Header("Neutral Fish Variables")]
    [SerializeField]
    private int _range = 60;
    public int Range { get { return _range; } }

    [SerializeField]
    private int _wallDetectionRange = 4;
    public int WallDetectionRange { get { return _wallDetectionRange; } }

    protected Transform target;
    public Transform Target { get { return target; } }

    protected Rigidbody targetBody;
    public Rigidbody TargetBody { get { return targetBody; } }

    [SerializeField]
    [Tooltip("Lower is harder\nNo 0 allowed")]
    private float _difficulty = 5;
    public float Difficulty { get { return _difficulty; } }

    [SerializeField]
    private int _oxygenDrain = 250;
    public int OxygenDrain { get { return _oxygenDrain; } }

    private Oxygen _oxygen;
    public Oxygen OxygenVals { get { return _oxygen; } }

    //Ignores the player for wall detection
    private LayerMask _ignoreDetection = (1 << 8);
    public LayerMask IgnoreDetection { get { return _ignoreDetection; } }

    public override void Start()
    {
        base.Start();

        _oxygen = FindObjectOfType<Oxygen>();
        target = FindObjectOfType<SubMovement>().transform;
        targetBody = target.GetComponent<Rigidbody>();
    }

    public override void FixedUpdate()
    {
        BindZ();

        base.FixedUpdate();
    }
}