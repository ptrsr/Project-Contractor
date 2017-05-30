using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#region values
[System.Serializable]
class DarkZone {
	public List<GameObject> objects;
	public List<Vector4> positionData;
	public List<float> rangeData;
//	public List<float> blendData;
}
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
	public Color startColor, endColor;
	public float _surface;
	public float _maxDepth;
}
[System.Serializable]
class Caustics {
	public Color causticsColor;
	[Range(0f,50f)]
	public float size, intensity = 20;
	public float _causticsDepth = -150;
}
#endregion

public class FX : MonoBehaviour {
	[SerializeField]
	private DarkZone _darkZone = new DarkZone ();

	[SerializeField]
	private Sonar _sonar = new Sonar();

	[SerializeField]
	private Fog _fog = new Fog();

	[SerializeField]
	private Caustics _caustics = new Caustics();

	public Material _mat; 
	public Transform _origin;
	private Camera _cam;
	private GameObject _light;

	private float[] aPulse;
	private bool[] activepulse;
	private Vector4[] aOrigin;

    public List<Volumetric> _volumes = new List<Volumetric>();

	private float _depth;

	void Start() {
		_light = GameObject.Find ("Directional Light");
		//_light.GetComponent<Light> ().enabled = false;
		_cam = Camera.main;
		_cam.depthTextureMode = DepthTextureMode.DepthNormals;
		activepulse = new bool[_sonar.maxPulses];
		aPulse = new float[_sonar.maxPulses];
		aOrigin = new Vector4[_sonar.maxPulses];
		//setupDarkZones ();
	}

	void Update () {
		PassiveSonar ();
		PulseActivate ();
		PulseControl ();
		_depth = calculateWorldDepth ();
        lightUpdate();
	}

	void setupDarkZones() {
		for (int i = 0; i < _darkZone.objects.Count; i++) {
			float range = _darkZone.objects [i].GetComponent<darkZone> ().range;
			_darkZone.rangeData.Add (range);
//			float blendWidth = _darkZone.objects [i].GetComponent<darkZone> ().blendWidth;
//			_darkZone.blendData.Add (blendWidth);
		    Vector3 pos = _darkZone.objects[i].transform.position;
			_darkZone.positionData.Add (pos);
		}
		//_mat.SetVectorArray ("_darkZones", _darkZone.positionData);
//		_mat.SetFloatArray ("_rangeData", _darkZone.rangeData);
//		_mat.SetFloatArray ("_blendData", _darkZone.blendData);
	}

	float calculateWorldDepth() {
		float camDepth = Camera.main.transform.position.y;
		float depth = (camDepth - _fog._surface) / (_fog._maxDepth - _fog._surface);
		return depth;
	}

    void lightUpdate() {
        float intensity = 1 - calculateWorldDepth();
        _light.GetComponent<Light>().intensity = intensity;
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

		// fog
		_mat.SetColor("_startColor", _fog.startColor);
		_mat.SetColor("_endColor", _fog.endColor);
		_mat.SetFloat ("_fogEnd", _fog.fogRange);
		_mat.SetFloat("surface", _fog._surface);
		_mat.SetFloat("_fogDepth", _fog._maxDepth);
		_mat.SetFloat("_depth", Mathf.Clamp(_depth, 0, 1));

		// caustics
		_mat.SetFloat("causticsSize", _caustics.size);
		_mat.SetFloat("causticsIntensity", _caustics.intensity);
		_mat.SetFloat ("causticsDepth", _caustics._causticsDepth);
		_mat.SetColor ("causticsColor", _caustics.causticsColor);
	}

	[ImageEffectOpaque]
	void OnRenderImage (RenderTexture src, RenderTexture dst){
		updateShader ();
		RaycastCornerBlit (src, dst, _mat);
		
		foreach (Volumetric volume in _volumes)
			volume.Render(ref dst);
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