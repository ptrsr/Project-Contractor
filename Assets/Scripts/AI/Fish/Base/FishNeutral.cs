using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishNeutral : Fish
{
    [Header("Neutral Fish Variables")]
    [SerializeField]
    private int _wallDetectionRange = 4;
    public int WallDetectionRange { get { return _wallDetectionRange; } }
    
    //Ignores the player for wall detection
    private LayerMask _ignoreDetection = 1 << 8;
    public LayerMask IgnoreDetection { get { return _ignoreDetection; } }

    public override void Start()
    {
        base.Start();
    }

    public override void Update()
    {
        BindZ();

        base.Update();
    }
}