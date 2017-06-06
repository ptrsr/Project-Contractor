using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class vignette : MonoBehaviour {
	[Range(0,2)]
	public float range = 0.5f;
	[Range(0,1)]
	public float width = 0, intensity = 1;

	public Material mat;

	void ShaderUpdate () {
		mat.SetFloat ("_range", range);
		mat.SetFloat ("_width", width);
		mat.SetFloat ("_intensity", intensity);
	}

	void OnRenderImage (RenderTexture src, RenderTexture dst) {
		ShaderUpdate ();

		mat.SetTexture ("_scene", src);
		Graphics.Blit (src, dst, mat);
	}
}