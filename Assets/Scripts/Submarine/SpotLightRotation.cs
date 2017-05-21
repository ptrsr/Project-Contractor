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
        Vector3 mouseWorldPos = Camera.main.ViewportToWorldPoint(Input.mousePosition + new Vector3(0,0,1));

	}
}
