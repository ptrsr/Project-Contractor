using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playerMove : MonoBehaviour {

	float _h;
	float _v;
	public float speed;
	
	void FixedUpdate () {
		_h = Mathf.Lerp (_h, Input.GetAxis ("Horizontal"), 0.1f);
		_v = Mathf.Lerp (_v, Input.GetAxis ("Vertical"), 0.1f);

		transform.position += new Vector3 (_h * speed, _v * speed, 0);
	}
}