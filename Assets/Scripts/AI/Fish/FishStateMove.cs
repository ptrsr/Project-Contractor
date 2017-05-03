using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FishStateMove : FishState
{
    public FishStateMove(Fish pFish) : base(pFish) { }

    Transform target;
    float range = 6;
    float xForce = 0;
    float yForce = 0;
    float force = 20;
    int delay = 50;
    int count = 0;

    public override void Initialize()
    {
        target = ((FishEnemy)fish).Target;

        Vector3 difPos = fish.transform.position - fish.OriginPos;

        if (xForce == 0 && yForce == 0)
        {
            if (difPos.x < -range)
            {
                xForce = force;
            }
            else if (difPos.x > range)
            {
                xForce = -force;
            }
            else
            {
                xForce = Random.Range(-force, force);
            }

            if (difPos.y < -range)
            {
                yForce = force;
            }
            else if (difPos.y > range)
            {
                yForce = -force;
            }
            else
            {
                yForce = Random.Range(-force, force);
            }
        }

        RaycastHit hit;
        if (Physics.Raycast(fish.transform.position, new Vector3(xForce, yForce, 0), out hit, 6, ~((FishEnemy)fish).mask))
        {
            //Wall detected
            if (Vector3.Distance(hit.point, fish.transform.position) < 6)
            {
                Vector3 dir = hit.point - fish.transform.position;

                do
                {
                    dir = new Vector3(dir.x * Mathf.Cos(90) - dir.y * Mathf.Sin(90), dir.x * Mathf.Sin(90) + dir.y * Mathf.Cos(90));
                    Debug.DrawRay(fish.transform.position, dir, Color.red, 2f);
                } while (Physics.Raycast(fish.transform.position, dir, out hit, 6, ~((FishEnemy)fish).mask));

                dir = dir.normalized;
                xForce = dir.x * force;
                yForce = dir.y * force;
            }
        }

        float wallDis = Vector3.Distance(fish.transform.position, hit.point);
        float targetDis = Vector3.Distance(fish.OriginPos, target.position);
        float origDis = Vector3.Distance(fish.transform.position, fish.OriginPos);;

        if (!Physics.Linecast(fish.transform.position, target.position, ~((FishEnemy)fish).mask) && targetDis < 10 && origDis < 10)
            fish.SetState<FishStateChase>();
    }

    public override void Step()
    {
        fish.transform.position = new Vector3(fish.transform.position.x, fish.transform.position.y, 0);

        Debug.DrawRay(fish.transform.position, new Vector3(xForce, yForce, 0));

        if (count != delay)
        {
            Quaternion lookDir = Quaternion.LookRotation(new Vector3(xForce, yForce, 0));
            lookDir.eulerAngles -= new Vector3(-90, 0, 0);
            fish.transform.rotation = Quaternion.Slerp(fish.transform.rotation, lookDir, 0.05f);
            count++;
        }
        else
        {
            count = 0;

            fish.Body.AddForce(new Vector3(xForce, yForce, 0), ForceMode.Force);

            xForce = 0;
            yForce = 0;

            fish.SetState<FishStateIdle>();
        }
    }
}