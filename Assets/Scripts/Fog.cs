using UnityEngine;

[ExecuteInEditMode]
public class Fog : MonoBehaviour
{
    [Range(0f, 3f)]
    public float intensity = 0.5f;

    [SerializeField]
    private Shader _shader;

    [SerializeField]
    private Color
        _startColor,
        _endColor;

    private Material _material;

    private void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        _material = new Material(_shader);
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        float depth = -Camera.main.transform.position.y / 80;

        if (_shader != null && depth > 0)
        {
            _material.SetFloat("_intensity", intensity);
            _material.SetVector("_startColor", _startColor);
            _material.SetFloat("_depth", depth);
            Graphics.Blit(src, dest, _material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}