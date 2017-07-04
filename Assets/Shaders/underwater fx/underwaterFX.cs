using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

#region values
[System.Serializable]
public class Sonar
{
    public Color
        highlightColor,
        outlineColor;

    public float
        interval = 2,
        outlineWidth = 2,
        fade = 30,
        highlightWidth = 1,
        distance = 100,
        speed = 50,
        start = 5;

	public bool enabled = true;
}
[System.Serializable]
class Fog
{
	public Color 
        startColor, 
        endColor;

    public float 
        surface = 100f,
	    maxDepth = -200f;

	[Range(0,1)]
	public float intensity = 0;
	[Range(1,10)]
	public float curveShift = 0;
}
[System.Serializable]
class Caustics
{
	public Color causticsColor;

    [Range(0f,50f)]
	public float 
        size = 30f, 
        intensity = 30f;

    public float causticsDepth = -50f;
}
#endregion

public class Pulse
{
    public int _number;
    public Vector4 _origin;
    public float _distance;

    public Pulse(int number, Vector3 spawnPosition, float spawnDist)
    {
        _number = number;
        _origin = spawnPosition;
        _distance = spawnDist;
    }
}

public class underwaterFX : MonoBehaviour {

	[SerializeField]
	private bool update = false;

	[SerializeField]
	private Sonar _sonar = new Sonar();
	public Sonar SonarVals{get{ return _sonar; }}

	[SerializeField]
	private Fog _fog = new Fog();

	[SerializeField]
	private Caustics _caustics = new Caustics();

	public Material _mat; 
	public Transform _player;
	private Camera _cam;
	private Light _light;

    public List<Pulse> _pulses;

    private float _pulseTimer;
    private int _pulsesSpawned;

	void Start() {

		_light = GameObject.Find ("Directional Light").GetComponent<Light>();
		_cam = Camera.main;
		_cam.depthTextureMode = DepthTextureMode.DepthNormals;
		//_cam.depthTextureMode = DepthTextureMode.Depth;

        // PULSE SETUP
        _pulses = new List<Pulse>();
        _pulseTimer = 0;
        _pulsesSpawned = 0;

		updateStart ();
	}

	void Update ()
    {
		lightUpdate();
	}

    void lightUpdate()
    {
        float intensity = 1 - (_cam.transform.position.y - _fog.surface) / (_fog.maxDepth - _fog.surface);

        Vector2 worldPos = new Vector2(_player.position.x, _player.position.y);

        foreach (var zone in DarkZones.Get())
        {
            Vector2 zonePos = new Vector2(zone.Position.x, zone.Position.y);
            float dist = Vector2.Distance(worldPos, zonePos);

            if (dist > zone.CloseRadius + zone.FarRadius)
                continue;

            if (dist < zone.CloseRadius)
                intensity *= zone.Color.grayscale;
            else
                intensity *= Mathf.Abs((zone.CloseRadius - dist) / zone.FarRadius);
            
            break;
        }
        _light.intensity = intensity;
    }


	void updateStart ()
    {
        // sonar
        _mat.SetColor("_outlineColor", _sonar.outlineColor);
        _mat.SetColor("_highlightColor", _sonar.highlightColor);

        _mat.SetFloat ("_fade", _sonar.fade);
		_mat.SetFloat ("_highlightWidth", _sonar.highlightWidth);
        _mat.SetFloat("_outlineWidth", _sonar.outlineWidth);
        _mat.SetFloat ("_start", _sonar.start);

		// fog
		_mat.SetColor("_startColor", _fog.startColor);
		_mat.SetColor("_endColor", _fog.endColor);
		_mat.SetFloat("surface", _fog.surface);
		_mat.SetFloat("_fogDepth", _fog.maxDepth);


		_mat.SetFloat ("_intensity", _fog.intensity);
		_mat.SetFloat ("_curve", _fog.curveShift);

        int zoneCount = DarkZones.Get().Count;

        if (zoneCount > 0)
        {
            _mat.SetFloat("_darkZones", zoneCount);
            _mat.SetVectorArray("_darkPositions", DarkZones.Positions());
            _mat.SetFloatArray("_darkCloseRadius", DarkZones.CloseRadius());
            _mat.SetFloatArray("_darkFarRadius", DarkZones.FarRadius());

            _mat.SetVectorArray("_darkColors", DarkZones.Colors());
        }

        // caustics
        _mat.SetFloat("causticsSize", _caustics.size);
		_mat.SetFloat("causticsIntensity", _caustics.intensity);
		_mat.SetFloat ("causticsDepth", _caustics.causticsDepth);
		_mat.SetColor ("causticsColor", _caustics.causticsColor);
	}

	void UpdatePulses()
	{
        _pulseTimer += Time.deltaTime;

        // move old pulses, remove if exceeds sonar distance
        for (int i = _pulses.Count - 1; i >= 0; i--)
        {
            Pulse pulse = _pulses[i];

            pulse._distance += _sonar.speed * Time.deltaTime;

            if (pulse._distance > _sonar.distance)
                _pulses.RemoveAt(i);
        }

        // spawn new pulse
        if (_pulseTimer >= _sonar.interval)
        {
            _pulseTimer -= _sonar.interval;
            Pulse pulse = new Pulse(_pulsesSpawned, _player.position, _sonar.start + _sonar.speed * _pulseTimer);
            _pulsesSpawned++;
            _pulses.Add(pulse);
        }

        // update the actual shader
        Vector4[] origins = new Vector4[10];
        float[] distances = new float[10];

        for (int i = 0; i < _pulses.Count; i++)
        {
            Pulse pulse = _pulses[i];

            origins[i] = pulse._origin;
            distances[i] = pulse._distance;
        }
        distances[_pulses.Count] = -1;

        _mat.SetFloatArray ("_pulseDistances", distances);
		_mat.SetVectorArray ("_pulseOrigins", origins);
	}

	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dst)
    {
		UpdatePulses ();

		if (update)
			updateStart ();

		RaycastCornerBlit (src, dst, _mat);


    }

    void OnPostRender()
    {
        RenderTexture screen = RenderTexture.active;

        foreach (var volume in Volumes.Get())
            volume.Render(ref screen);
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