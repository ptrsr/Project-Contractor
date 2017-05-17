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
    private float _range = 10f;
    public float Range { get { return _range; } }
    
    private Transform _target = null;
    public Transform Target { get { return _target; } }

    [SerializeField]
    [Tooltip("Lower is harder\nNo 0 allowed")]
    private float _difficulty = 5;
    public float Difficulty { get { return _difficulty; } }

    [SerializeField]
    private int _wallDetectionRange = 4;
    public int WallDetectionRange { get { return _wallDetectionRange; } }
    
    //Ignores the player for wall detections
    private int _ignoreDetection = (1 << 8);
    public int IgnoreDetection { get { return _ignoreDetection; } }

    public override void Start()
    {
        base.Start();

        _target = FindObjectOfType<SubMovement>().transform;
    }

    public override void Update()
    {
        BindZ();

        base.Update();
    }

    public void SetRandomDirection()
    {
        Vector3 difPos = (transform.position - OriginPos).normalized;

        float xForce;
        float yForce;

        if (difPos.x < -Range)
            xForce = MoveSpeed;
        else if (difPos.x > Range)
            xForce = -MoveSpeed;
        //if (difPos.x < -Range || difPos.x > Range)
        //    xForce = difPos.x;
        else
            xForce = Random.Range(-MoveSpeed, MoveSpeed);

        if (difPos.y < -Range)
            yForce = MoveSpeed;
        else if (difPos.y > Range)
            yForce = -MoveSpeed;
        else
            yForce = Random.Range(-MoveSpeed, MoveSpeed);

        Direction = new Vector3(xForce, yForce, 0).normalized;
    }

    public bool DetectWall()
    {
        RaycastHit hit;
        Debug.DrawRay(transform.position, Direction.normalized * WallDetectionRange);
        if (Physics.Raycast(transform.position, Direction, out hit, WallDetectionRange, ~IgnoreDetection))
        {
            //Wall detected
            if (Vector3.Distance(hit.point, transform.position) < WallDetectionRange)
            {
                Vector3 dir = hit.point - transform.position;

                //Avoid wall by rotating to a different direction
                do
                {
                    dir = new Vector3(dir.x * Mathf.Cos(90) - dir.y * Mathf.Sin(90), dir.x * Mathf.Sin(90) + dir.y * Mathf.Cos(90));
                    Debug.DrawRay(transform.position, dir, Color.red, 1f);
                } while (Physics.Raycast(transform.position, dir, out hit, WallDetectionRange, ~IgnoreDetection));

                Direction = dir.normalized;
            }

            return true;
        }
        else
            return false;
    }

    public bool DetectTarget()
    {
        float enemyDis = Vector3.Distance(transform.position, Target.position);
        float targetDis = Vector3.Distance(OriginPos, Target.position);
        float origDis = Vector3.Distance(transform.position, OriginPos); ;

        if (!Physics.Linecast(transform.position, Target.position, ~IgnoreDetection) && enemyDis < Range && targetDis < Range && origDis < Range)
            return true;
        else
            return false;
    }
}