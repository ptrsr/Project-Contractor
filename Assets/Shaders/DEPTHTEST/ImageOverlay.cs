using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class ImageOverlay : MonoBehaviour {
	[SerializeField]
	private Material mat;
	void Start() { GetComponent<Camera> ().depthTextureMode = DepthTextureMode.Depth; }
	void OnRenderImage(RenderTexture src, RenderTexture dst) { Graphics.Blit (src, dst, mat); }	
}