using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpotLightRotation : MonoBehaviour
{
    private Camera _mainCamera;

	void Start ()
    {
        _mainCamera = Camera.main;
	}
	
	void Update ()
    {
        Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(Input.mousePosition + new Vector3(0,0,1));

        Vector3 delta = Vector3.Normalize(mouseWorldPos - transform.position);

        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(angle, 90, 0);
	}
}
