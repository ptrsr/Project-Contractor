using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateMove : FishState
{
    public FishStateMove(Fish pFish) : base(pFish) { }

    private FishEnemy _fishEnemy;
    float range = 6;
    int delay = 50;
    int count = 0;

    public override void Initialize()
    {
        _fishEnemy = (FishEnemy)fish;

        DetectTarget();
        SetRandomDirection();
        DetectWall();
    }

    public override void Step()
    {
        Debug.DrawRay(fish.transform.position, fish.Direction);

        if (fish.RotateTowards(fish.Direction))
        {
            fish.Body.AddForce(fish.Direction, ForceMode.Force);
            fish.SetState<FishStateIdle>();
        }
    }

    private void SetRandomDirection()
    {
        Vector3 difPos = fish.transform.position - fish.OriginPos;

        float xForce;
        float yForce;

        if (difPos.x < -range)
            xForce = fish.MoveSpeed;
        else if (difPos.x > range)
            xForce = -fish.MoveSpeed;
        else
            xForce = Random.Range(-fish.MoveSpeed, fish.MoveSpeed);

        if (difPos.y < -range)
            yForce = fish.MoveSpeed;
        else if (difPos.y > range)
            yForce = -fish.MoveSpeed;
        else
            yForce = Random.Range(-fish.MoveSpeed, fish.MoveSpeed);

        fish.Direction = new Vector3(xForce, yForce, 0);
    }

    private void DetectWall()
    {
        RaycastHit hit;
        if (Physics.Raycast(fish.transform.position, fish.Direction, out hit, _fishEnemy.WallDetectionRange, ~_fishEnemy.IgnoreDetection))
        {
            //Wall detected
            if (Vector3.Distance(hit.point, fish.transform.position) < _fishEnemy.WallDetectionRange)
            {
                Vector3 dir = hit.point - fish.transform.position;

                //Avoid wall by rotating to a different direction
                do
                {
                    dir = new Vector3(dir.x * Mathf.Cos(90) - dir.y * Mathf.Sin(90), dir.x * Mathf.Sin(90) + dir.y * Mathf.Cos(90));
                    Debug.DrawRay(fish.transform.position, dir, Color.red, 1f);
                } while (Physics.Raycast(fish.transform.position, dir, out hit, _fishEnemy.WallDetectionRange, ~_fishEnemy.IgnoreDetection));

                dir = dir.normalized;
                fish.Direction = new Vector3(dir.x * fish.MoveSpeed, dir.y * fish.MoveSpeed, 0);
            }
        }
    }

    private void DetectTarget()
    {
        float targetDis = Vector3.Distance(fish.OriginPos, _fishEnemy.Target.position);
        float origDis = Vector3.Distance(fish.transform.position, fish.OriginPos); ;

        if (!Physics.Linecast(fish.transform.position, _fishEnemy.Target.position, ~((FishEnemy)fish).IgnoreDetection) && targetDis < _fishEnemy.DetectionRange && origDis < _fishEnemy.DetectionRange)
        {
            fish.SetState<FishStateChase>();
        }
    }
}