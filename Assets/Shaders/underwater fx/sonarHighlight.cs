using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonarHighlight : MonoBehaviour {

	private Material mat;

	private Camera mainCam;
	private Camera sonarCam;

	private RenderTexture scene;

	void Start () {
		mainCam = Camera.main;
		sonarCam = GetComponent<Camera> ();
		sonarCam.depthTextureMode = DepthTextureMode.Depth;
		scene = new RenderTexture (sonarCam.pixelWidth, sonarCam.pixelHeight, 16, RenderTextureFormat.Depth);

		mat = mainCam.GetComponent<UnderwaterFX> ()._mat;
	}

	void Update () {
		CamPosition ();
		RenderScene ();
	}

	void CamPosition () {
		transform.position = mainCam.transform.position;
		transform.rotation = transform.rotation;
	}
		
	void RenderScene () {
		sonarCam.targetTexture = scene;
		sonarCam.Render ();
		mat.SetTexture ("sonarScene", scene);
	}
}