using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonar : MonoBehaviour {

	public Material SonarMat; 
	public Transform _origin;
	public Camera _cam;

	private bool _sonar;
	private float _speed = 5;

	public float _frequency;
	private float _interval;
	private float _width;
	private float _traveldistance;

	public int _pulselength;
	private float[] pulse;
	private bool[] activepulse;

	void Start() {
		pulse = new float[_pulselength];
		activepulse = new bool[_pulselength];
		_cam.depthTextureMode = DepthTextureMode.Depth;
	}

	void Update () {
		PassiveSonar ();
		PulseActivate ();
		FrequencyControl ();
		PulseControl ();
	}

	void FrequencyControl() {
		float shift = (_frequency - 1) / (5 - 1);

		_interval = Mathf.Lerp (0.25f, 3f, shift);
		_width = Mathf.Lerp (1f, 5f, shift);
		_traveldistance = Mathf.Lerp (5f, 20f, shift);
	}

	void PulseActivate() {
		if (_sonar) {
			for (int i = 0; i < _pulselength; i++) {
				if (!activepulse [i]) {
					activepulse [i] = true;
					return;
				}
			}
		}
	}

	void PulseControl() {
		for (int i = 0; i < _pulselength; i++) {
			if (activepulse [i]) {
				pulse [i] += Time.deltaTime * _speed;
				if (pulse [i] > _traveldistance) {
					activepulse [i] = false;
					pulse [i] = 0;
				}
			}
		}
	}
		
	void ActiveSonar() {
		_sonar = Input.GetKeyDown (KeyCode.Space);
	}
		
	float time;
	void PassiveSonar () {
		time += Time.deltaTime;
		if (time > _interval) {
			_sonar = true;
			time = 0;
		} else {
			_sonar = false;
		}
	}



	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dst){
		SonarMat.SetVector ("_origin", _origin.position);
		SonarMat.SetFloat ("_width", _width);
		SonarMat.SetInt ("_pulselength", _pulselength);
		SonarMat.SetFloatArray ("_pulses", pulse);
		RaycastCornerBlit (src, dst, SonarMat);
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

		mat.SetTexture("_MainTex", source);

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