using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region values
[System.Serializable]
class Sonar
{
    public int maxPulses    = 5;

    public float interval   = 1;
    public float width      = 1;
    public float distance   = 8;
    public float speed      = 5;
}

[System.Serializable]
class Fog
{
    [Range(0f, 3f)]
    public float intensity = 2f;

    public Color
        startColor,
        endColor;

    public Light worldLight;
}

[System.Serializable]
class SpotLight
{
    public Transform transform;

    public float angle = 9;
    public float fallOff = 2;
    public float camDist = 15;
    public float scaling = 20;
}
#endregion

public class PostFX : MonoBehaviour
{
    [SerializeField]
	private Sonar _sonar = new Sonar();

    [SerializeField]
    private Fog _fog = new Fog();

    [SerializeField]
    private SpotLight _light = new SpotLight();

    [SerializeField]
    private Shader _fxShader;

    [SerializeField]
    private Transform _origin;

    private Material _fxMat;

    private Camera _cam;

	private bool _active;
    private float _depth;


    public float testDist;

    #region sonar arrays

    private float[]
        aPulse,
        aTravel,
        aWidth;

    private bool[] activepulse;
	private Vector4[] origin;
    #endregion

	void Start()
    {
        _cam = Camera.main;
        _cam.depthTextureMode = DepthTextureMode.Depth;

        _fxMat    = new Material(_fxShader);


        activepulse = new bool[_sonar.maxPulses];
		aPulse      = new float[_sonar.maxPulses];
		origin      = new Vector4[_sonar.maxPulses];
		aTravel     = new float[_sonar.maxPulses];
		aWidth      = new float[_sonar.maxPulses];

        updateShader();
    }

	void Update ()
    {
        _depth = -Camera.main.transform.position.y / 1000;
        _fog.worldLight.intensity = 1 - _depth;


        Vector2 dir = MouseDirection(WorldToScreen(_light.transform.position));
        _light.transform.eulerAngles = new Vector3(-Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg, 90, 0);

        PassiveSonar ();
		ActiveSonar();
		PulseActivate ();
		PulseControl ();
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
        updateShader();

        //sonar
        _fxMat.SetInt("_maxPulses", _sonar.maxPulses);
        _fxMat.SetFloatArray("_aPulseDist", aPulse);
        _fxMat.SetVectorArray("_aOrigin", origin);
        _fxMat.SetFloatArray("_aWidth", aWidth);

        Vector2 spotScreenPos = WorldToScreen(_light.transform.position);
        _fxMat.SetVector("_spotPos", spotScreenPos);
        _fxMat.SetVector("_spotDir", MouseDirection(spotScreenPos));

        _fxMat.SetFloat("_depth", Mathf.Clamp(_depth, 0, 1));

        RaycastCornerBlit(src, dst, _fxMat);
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

		mat.SetTexture("_SceneTex", source);

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

        //mat.SetPass(1);

        //GL.Begin(GL.QUADS);

        //GL.Vertex3(-100, -100, testDist);

        //GL.Vertex3(100, -100, testDist);

        //GL.Vertex3(100, 100, testDist);

        //GL.Vertex3(-100, 100, testDist);

        //GL.End();

        //Graphics.Blit(source, mat, 2);
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

    void OnValidate()
    {
        if (Application.isPlaying && _fxMat != null)
            updateShader();
    }

    void updateShader()
    {
        //fog
        _fxMat.SetFloat("_intensity", _fog.intensity);
        _fxMat.SetVector("_startColor", _fog.startColor);
        _fxMat.SetVector("_endColor", _fog.endColor);


        //spotlight
        _fxMat.SetFloat("_zoom", -transform.position.z);

        _fxMat.SetFloat("_spotAngle", _light.angle);
        _fxMat.SetFloat("_spotFallOff", _light.fallOff);

        _fxMat.SetFloat("_camDist", _light.camDist);
        _fxMat.SetFloat("_spotScaling", _light.scaling);
    }
}