using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrailFollow : MonoBehaviour {

	public Transform player;
	private GameObject smooth;
	private GameObject lazysmooth;
	public Material mat;

	public bool follow = false;

	private Vector4 smoothVec;
	private Vector4 lazysmoothVec;

	void Start () {
		SetupTargets ();
		UpdateShader ();
	}

	void Update () {
		if (!follow)
			return;
		TargetFollow ();
		UpdateShader ();
	}

	void TargetFollow () {
		smooth.transform.position = Vector3.Lerp 
			(smooth.transform.position, player.position, 0.1f);
		lazysmooth.transform.position = Vector3.Lerp 
			(lazysmooth.transform.position, smooth.transform.position, 0.1f);

		smoothVec = smooth.transform.position - player.position;
		lazysmoothVec = lazysmooth.transform.position - smooth.transform.position;
	}

	void SetupTargets () {
		smooth = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		smooth.name = "smootTarget";
		smooth.transform.position = player.position;
		smooth.transform.position += new Vector3 (0, -20, 0);

		lazysmooth = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		lazysmooth.name = "smootTarget";
		lazysmooth.transform.position = smooth.transform.position;
		lazysmooth.transform.position += new Vector3 (0, -20, 0);
	}

	void UpdateShader () {
		mat.SetVector ("player", player.position);
		mat.SetVector ("smooth", smoothVec);
		mat.SetVector ("lazysmooth", lazysmoothVec);
	}

}