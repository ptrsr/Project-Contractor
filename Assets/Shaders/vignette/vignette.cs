			using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vignette : MonoBehaviour {

	[SerializeField]
	private bool update = false;

	[Range(0,1)]
	public float 
		range = 0.5f, 
		width = 0.5f, 
		intensity = 1,
		curve = 0;

	public Material mat;

	void Start () {
		ShaderUpdate ();
	}

	void ShaderUpdate () {
		mat.SetFloat ("_range", Mathf.Lerp(0.5f, 2f, range));
		mat.SetFloat ("_width", width);
		mat.SetFloat ("_intensity", intensity);
		mat.SetFloat ("_curve", curve * 2);
	}

	void Update () {
		if (!update)
			return;
		ShaderUpdate ();
	}

	void OnRenderImage (RenderTexture src, RenderTexture dst) {
		mat.SetTexture ("_scene", src);
		Graphics.Blit (src, dst, mat);
	}
}