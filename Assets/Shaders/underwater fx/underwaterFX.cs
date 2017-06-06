using System.Collections;
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
    [Range(0f, 10f)]
    public float fallOff = 10;
	public Color startColor, endColor;
	public float surface;
	public float maxDepth;
}
[System.Serializable]
class Caustics {
	public Color causticsColor;
	[Range(0f,50f)]
	public float size, intensity = 20;
	public float causticsDepth = -150;
}
#endregion

public class underwaterFX : MonoBehaviour
{
    [SerializeField]
    Shader _shader;

	[SerializeField]
	private Sonar _sonar = new Sonar();

	[SerializeField]
	private Fog _fog = new Fog();

	[SerializeField]
	private Caustics _caustics = new Caustics();

    [SerializeField]
    Light _sceneLight;

    [SerializeField]
    List<Volumetric> _volumes = new List<Volumetric>();

	private Material _mat; 
	public Transform _origin;
	private Camera _cam;
	private GameObject _light;

	private float[] _pulses;
	private bool[] _activePulses;
	private Vector4[] _origins;

	private float _depth;

	void Start()
    {
        _mat = new Material(_shader);
        SetShaderValues();

        _cam = Camera.main;

        _activePulses = new bool[_sonar.maxPulses];
		_pulses = new float[_sonar.maxPulses];
		_origins = new Vector4[_sonar.maxPulses];

	}

	void Update () {
		PassiveSonar ();
		PulseActivate ();
		PulseControl ();
		_depth = CalculateWorldDepth ();
	}
		

	float CalculateWorldDepth()
    {
		float camDepth = Camera.main.transform.position.y;
		float depth = (camDepth - _fog.surface) / (_fog.maxDepth - _fog.surface);
		return depth;
	}

	void PulseActivate()
    {
        if (!_sonar.active)
            return;

		for (int i = 0; i < _sonar.maxPulses; i++)
        {
            if (_activePulses[i])
                continue;

            _activePulses [i] = true;
			_origins [i] = _origin.position;
			return;
		}
	}

	void PulseControl()
    {
		for (int i = 0; i < _sonar.maxPulses; i++)
        {
            if (!_activePulses[i])
                continue;

			_pulses [i] += Time.deltaTime * _sonar.speed;

			if (_pulses [i] > _sonar.distance)
            {
				_activePulses [i] = false;
				_pulses [i] = 0;
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

    private void OnValidate()
    {
        if (_mat != null)
            SetShaderValues();
    }

    // used for applying settings, only has to be called once
    void SetShaderValues()
	{
		// sonar
		_mat.SetFloat ("_width", _sonar.width);

		// fog
		_mat.SetColor("_startColor", _fog.startColor);
		_mat.SetColor("_endColor", _fog.endColor);
		_mat.SetFloat ("_fogFallOff", _fog.fallOff);
		_mat.SetFloat("_surface", _fog.surface);
		_mat.SetFloat("_fogDepth", _fog.maxDepth);
		_mat.SetFloat("_depth", Mathf.Clamp(_depth, 0, 1));

		// caustics
		_mat.SetFloat("_causticsSize", _caustics.size);
		_mat.SetFloat("_causticsIntensity", _caustics.intensity);
		_mat.SetFloat ("_causticsDepth", _caustics.causticsDepth);
		_mat.SetColor ("_causticsColor", _caustics.causticsColor);
	}

    // used for updating shader values at runtime
    void UpdateShader(ref RenderTexture src)
    {
        //scene
        _cam.depthTextureMode = DepthTextureMode.DepthNormals;
        _mat.SetTexture("_scene", src);

        //sonar
        _mat.SetInt("_pulselength", _sonar.maxPulses);
        _mat.SetFloatArray("_pulses", _pulses);
        _mat.SetVectorArray("_originarray", _origins);
    }

	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dst)
    {
        UpdateShader(ref src);
		RaycastCornerBlit(src, dst, _mat);

        foreach (var volume in _volumes)
            volume.Render(ref dst);
	}

	void RaycastCornerBlit(RenderTexture src, RenderTexture dst, Material mat)
	{
		// Compute Frustum Corners
		float camFar = _cam.farClipPlane;
		float camFov = _cam.fieldOfView;
		float camAspect = _cam.aspect;

		float fovWHalf = Mathf.Tan(camFov * 0.5f * Mathf.Deg2Rad);

		Vector3 toRight = _cam.transform.right * fovWHalf * camAspect;
		Vector3 toTop = _cam.transform.up * fovWHalf;

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
		RenderTexture.active = dst;

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