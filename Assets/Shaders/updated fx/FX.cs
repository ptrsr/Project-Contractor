﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region values
[System.Serializable]
class Sonar {
	public int maxPulses = 5;
	public float interval = 1;
	public float width = 2;
	public float distance = 20;
	public float speed = 5;
	public bool active = false;
}
[System.Serializable]
class Fog {
	public float fogRange = 15f;
	public Color
		startColor,
		endColor;
	public float _minDepth;
	public float _maxDepth;
}
#endregion

public class FX : MonoBehaviour {

	[SerializeField]
	private Sonar _sonar = new Sonar();

	[SerializeField]
	private Fog _fog = new Fog();

	public Material _mat; 
	public Transform _origin;
	private Camera _cam;

	private float[] aPulse;
	private bool[] activepulse;
	private Vector4[] aOrigin;
	private float _depth;

	void Start() {
		
		activepulse = new bool[_sonar.maxPulses];
		aPulse = new float[_sonar.maxPulses];
		aOrigin = new Vector4[_sonar.maxPulses];

		_cam = Camera.main;
		_cam.depthTextureMode = DepthTextureMode.Depth;
	}

	void Update () {
		PassiveSonar ();
		PulseActivate ();
		PulseControl ();
		_depth = calculateWorldDepth ();
	}

	float calculateWorldDepth() {
		float camDepth = Camera.main.transform.position.y;
		float depth = (camDepth - _fog._minDepth) / (_fog._maxDepth - _fog._minDepth);
		return depth;
	}

	void PulseActivate() {
		if (_sonar.active) {
			for (int i = 0; i < _sonar.maxPulses; i++) {
				if (!activepulse [i]) {
					activepulse [i] = true;
					aOrigin [i] = _origin.position;

					return;
				}
			}
		}
	}

	void PulseControl() {
		for (int i = 0; i < _sonar.maxPulses; i++) {
			if (activepulse [i]) {
				aPulse [i] += Time.deltaTime * _sonar.speed;
				if (aPulse [i] > _sonar.distance) {
					activepulse [i] = false;
					aPulse [i] = 0;
				}
			}
		}
	}

	float time;
	void PassiveSonar () {
		time += Time.deltaTime;
		_sonar.active = false;
		if (time > _sonar.interval) {
			_sonar.active = true;
			time = 0;
		}
	}

	void updateShader()
	{
		// sonar
		_mat.SetInt ("_pulselength", _sonar.maxPulses);
		_mat.SetFloatArray ("_pulses", aPulse);
		_mat.SetVectorArray ("originarray", aOrigin);
		_mat.SetFloat ("width", _sonar.width);

		//fog
		_mat.SetVector("_startColor", _fog.startColor);
		_mat.SetVector("_endColor", _fog.endColor);
		_mat.SetFloat ("_fogEnd", _fog.fogRange);
		_mat.SetFloat("_depth", Mathf.Clamp(_depth, 0, 1));
	}

	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dst){
		updateShader ();
		RaycastCornerBlit (src, dst, _mat);
	}

	void RaycastCornerBlit(RenderTexture source, RenderTexture dest, Material mat)
	{
		// Compute Frustum Corners
		float camFar = _cam.farClipPlane;
		float camFov = _cam.fieldOfView;
		float camAspect = _cam.aspect;

		float fovWHalf = camFov * 0.5f;

		Vector3 toRight = _cam.transform.right * Mathf.Tan(fovWHalf * Mathf.Deg2Rad) * camAspect;
		Vector3 toTop = _cam.transform.up * Mathf.Tan(fovWHalf * Mathf.Deg2Rad);

		Vector3 topLeft = (_cam.transform.forward - toRight + toTop);
		float camScale = topLeft.magnitude * camFar;

		topLeft.Normalize();
		topLeft *= camScale;

		Vector3 topRight = (_cam.transform.forward + toRight + toTop);
		topRight.Normalize();
		topRight *= camScale;

		Vector3 bottomRight = (_cam.transform.forward + toRight - toTop);
		bottomRight.Normalize();
		bottomRight *= camScale;

		Vector3 bottomLeft = (_cam.transform.forward - toRight - toTop);
		bottomLeft.Normalize();
		bottomLeft *= camScale;

		// Custom Blit, encoding Frustum Corners as additional Texture Coordinates
		RenderTexture.active = dest;

		mat.SetTexture("_Scene", source);

		GL.PushMatrix();
		GL.LoadOrtho();

		mat.SetPass(0);

		GL.Begin(GL.QUADS);

		GL.MultiTexCoord2(0, 0.0f, 0.0f);
		GL.MultiTexCoord(1, bottomLeft);
		GL.Vertex3(0.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 0.0f);
		GL.MultiTexCoord(1, bottomRight);
		GL.Vertex3(1.0f, 0.0f, 0.0f);

		GL.MultiTexCoord2(0, 1.0f, 1.0f);
		GL.MultiTexCoord(1, topRight);
		GL.Vertex3(1.0f, 1.0f, 0.0f);

		GL.MultiTexCoord2(0, 0.0f, 1.0f);
		GL.MultiTexCoord(1, topLeft);
		GL.Vertex3(0.0f, 1.0f, 0.0f);

		GL.End();
		GL.PopMatrix();
	}

}