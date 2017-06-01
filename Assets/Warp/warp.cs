using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class warp : MonoBehaviour {

	public Material mat;

	void OnRenderImage(RenderTexture src, RenderTexture dst) {
		mat.SetTexture ("_cleanScene", src);
		Graphics.Blit (src, dst, mat);
	}
}