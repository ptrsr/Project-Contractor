using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class sonar : MonoBehaviour {

	public Material SonarMat; 
	public Transform _origin;
	public Camera _cam;

	public float _speed = 20;
	public float _width = 1;
	public float _travel = 10;

	public bool _sonar;

	public float[] pulse;
	public bool[] activepulse;

	void Start() {
		pulse = new float[3];
		activepulse = new bool[3];
        _cam.depthTextureMode = DepthTextureMode.Depth;
	}

	void Update () {
		SonarControl();
		PulseActivate ();
		PulseControl ();
	}

	void PulseActivate() {
		
		if (_sonar) {
			if (!activepulse [0]) {
				activepulse [0] = true;
				return;
			}
			if (!activepulse [1]) {
				activepulse [1] = true;
				return;
			}
			if (!activepulse [2]) {
				activepulse [2] = true;
				return;
			}
		}
	}

	void PulseControl() {

		if (activepulse [0]) {
			pulse [0] += Time.deltaTime * _speed;
			if (pulse [0] > _travel) {
				activepulse [0] = false;
				pulse [0] = 0;
			}
		}

		if (activepulse [1]) {
			pulse [1] += Time.deltaTime * _speed;
			if (pulse [1] > _travel) {
				activepulse [1] = false;
				pulse [1] = 0;
			}
		}

		if (activepulse [2]) {
			pulse [2] += Time.deltaTime * _speed;
			if (pulse [2] > _travel) {
				activepulse [2] = false;
				pulse [2] = 0;
			}
		}

	}

	void SonarControl() {
		if (Input.GetKeyDown (KeyCode.Space))
			_sonar = true;
		else
			_sonar = false;
	}



	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dst){
		
		SonarMat.SetVector ("_origin", _origin.position);
		SonarMat.SetFloat ("_width", _width);
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