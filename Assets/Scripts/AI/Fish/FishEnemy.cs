using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishEnemy : Fish
{
    [Header("Enemy Fish Variables")]
    [SerializeField]
    private float _chaseSpeed = 25f;
    public float ChaseSpeed { get { return _chaseSpeed; } }

    [SerializeField]
    private float _detectionRange = 10f;
    public float DetectionRange { get { return _detectionRange; } }

    [SerializeField]
    private Transform _target = null;
    public Transform Target { get { return _target; } }

    [SerializeField]
    private int _wallDetectionRange = 4;
    public int WallDetectionRange { get { return _wallDetectionRange; } }

    [SerializeField]
    private LayerMask _ignoreDetection = -1;
    public LayerMask IgnoreDetection { get { return _ignoreDetection; } }

    public override void Start()
    {
        base.Start();

        stateCache[typeof(FishStateIdle)] = new FishStateIdle(this);
        stateCache[typeof(FishStateMove)] = new FishStateMove(this);
        stateCache[typeof(FishStateChase)] = new FishStateChase(this);

        SetState<FishStateIdle>();
    }

    public override void Update()
    {
        base.Update();

        transform.position = new Vector3(transform.position.x, transform.position.y, 0);
    }

    public void OnCollisionEnter(Collision c)
    {
        Rigidbody body = c.collider.GetComponent<Rigidbody>();

        if (body != null)
            body.AddForce((_target.position - transform.position).normalized * 20);
    }
}