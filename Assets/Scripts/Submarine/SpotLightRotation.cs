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
	
	void LateUpdate ()
    {
        Vector3 pos = GetMousePosition();
        Vector3 dir = pos - transform.position;
        dir = dir.normalized;
        float angle = Mathf.Atan2(dir.x, dir.y) * Mathf.Rad2Deg;

        transform.eulerAngles = new Vector3(angle + 90, 90, 0);
	}

    private Vector3 GetMousePosition()
    {
        Vector3 pos = Input.mousePosition;
        pos.z = _mainCamera.transform.position.z;
        pos = _mainCamera.ScreenToWorldPoint(pos);
        return pos;
    }
}
