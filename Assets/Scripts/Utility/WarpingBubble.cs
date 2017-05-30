using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WarpingBubble : MonoBehaviour {

	private Material _mat;
	
	void Start () 
	{
		_mat = GetComponent<Material>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		_mat.SetFloat("_time", Time.time);
	}
}
