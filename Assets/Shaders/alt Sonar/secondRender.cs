using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class secondRender : MonoBehaviour {

	public Material mat;	

	private Camera mainCam;

	private RenderTexture scene;

	void Start () {
		mainCam = Camera.main;
		scene = new RenderTexture (mainCam.pixelWidth, mainCam.pixelHeight, 16, RenderTextureFormat.Depth);
	}

	void Update () {
		RenderScene ();
	}

	void RenderScene () {
		mainCam.cullingMask = 8;
		mainCam.targetTexture = scene;
		mainCam.Render ();

		mat.SetTexture ("sonarScene", scene);

		mainCam.cullingMask = 1;
		mainCam.targetTexture = null;
	}
}