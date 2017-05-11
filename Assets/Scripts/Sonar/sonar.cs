using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region values
[System.Serializable]
class SonarValues
{
    public int maxPulses    = 5;

    public float interval   = 1;
    public float width      = 1;
    public float distance   = 8;
    public float speed      = 5;
}

[System.Serializable]
class FogValues
{
    [Range(0f, 30f)]
    public float intensity = 0.5f;

    public Color
        startColor,
        endColor;

    public Light worldLight;
    public Transform spotLight;
}
#endregion

public class Sonar : MonoBehaviour
{
    [SerializeField]
	private SonarValues _sonar = new SonarValues();

    [SerializeField]
    private FogValues _fog = new FogValues();

    [SerializeField]
    private Shader _shader;

    [SerializeField]
    private Transform _origin;

    private Material _mat; 
	private Camera _cam;

	private bool _active;
    private float _depth;

    #region sonar arrays

    private float[]
        aPulse,
        aTravel,
        aWidth;

    private bool[] activepulse;
	private Vector4[] origin;
    #endregion

    public bool multiply = false;
	private int m;

	void Start()
    {
        _cam = Camera.main;
        _cam.depthTextureMode = DepthTextureMode.Depth;

        _mat = new Material(_shader);

        _mat.SetVector("_startColor", _fog.startColor);
        _mat.SetVector("_endColor",   _fog.endColor);

        activepulse = new bool[_sonar.maxPulses];
		aPulse      = new float[_sonar.maxPulses];
		origin      = new Vector4[_sonar.maxPulses];
		aTravel     = new float[_sonar.maxPulses];
		aWidth      = new float[_sonar.maxPulses];
	}

	void Update ()
    {
        _depth = -Camera.main.transform.position.y / 1000;
        _fog.worldLight.intensity = 1 - _depth;

        PassiveSonar ();
		ActiveSonar();
		PulseActivate ();
		FrequencyControl ();
		PulseControl ();

		if (multiply)
			m = 1;
		else
			m = 0;
	}

	void FrequencyControl() {
//		float shift = (_frequency - v.f_min) / (v.f_max - v.f_min); 

//		_interval = Mathf.Lerp (v.i_min, v.i_max, shift);
//		_width = Mathf.Lerp (v.w_min, v.w_max, shift);
//		_distance = Mathf.Lerp (v.d_min, v.d_max, shift);

	}

	void PulseActivate() {
		if (_active) {
			for (int i = 0; i < _sonar.maxPulses; i++) {
				if (!activepulse [i]) {
					activepulse [i] = true;
					origin [i] = _origin.position;
					aWidth [i] = _sonar.width;
					aTravel [i] = _sonar.distance;
					return;
				}
			}
		}
	}

	void PulseControl() {
		for (int i = 0; i < _sonar.maxPulses; i++) {
			if (activepulse [i]) {
				aPulse [i] += Time.deltaTime * _sonar.speed;
				if (aPulse [i] > aTravel[i]) {
					activepulse [i] = false;
					aPulse [i] = 0;
				}
			}
		}
	}

	void ActiveSonar() {
        _active = Input.GetKeyDown (KeyCode.Space);
	}
		
	float time;
	void PassiveSonar () {
		time += Time.deltaTime;
		if (time > _sonar.interval) {
            _active = true;
			time = 0;
		}
		else {
            _active = false;
		}
	}


	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dst)
    {
        //fog
        _mat.SetFloat("_intensity", _fog.intensity);
        _mat.SetFloat("_depth", Mathf.Clamp(_depth, 0, 1));
        _mat.SetFloat("_zoom", -transform.position.z);



        Vector2 spotScreenPos = WorldToScreen(_fog.spotLight.position);

        _mat.SetVector("_spotPos", spotScreenPos);
        _mat.SetVector("_spotDir", MouseDirection(spotScreenPos));

        //sonar
        _mat.SetInt ("_maxPulses", _sonar.maxPulses);
		_mat.SetFloatArray ("_aPulseDist", aPulse);
		_mat.SetVectorArray ("_aOrigin", origin);
		_mat.SetFloatArray ("_aWidth", aWidth);
		_mat.SetInt ("m", m);
//		Graphics.Blit (src, dst, SonarMat);
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


    private Vector2 MouseDirection(Vector2 objectPos)
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        Vector2 direction = (mousePos - objectPos);
        direction.Normalize();

        return direction;
    }

    private Vector2 WorldToScreen(Vector3 worldPos)
    {
        Vector3 localPos = _cam.WorldToScreenPoint(worldPos);
        Vector2 screenPos = new Vector2(localPos.x / Screen.width, localPos.y / Screen.height);

        return screenPos;
    }
}