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

    [SerializeField]
    Light _light;

    private Material _material;

    [SerializeField]
    private float _depth;

    private void Start()
    {
        GetComponent<Camera>().depthTextureMode = DepthTextureMode.Depth;
        _material = new Material(_shader);

        _material.SetVector("_startColor", _startColor);
        _material.SetVector("_endColor", _endColor);

    }

    private void Update()
    {
        _depth = -Camera.main.transform.position.y / 80;
        _light.intensity = 1 - _depth;
    }

    private void OnRenderImage(RenderTexture src, RenderTexture dest)
    {
        if (_shader != null && _depth > 0)
        {
            _material.SetFloat("_intensity", intensity);
            _material.SetFloat("_depth", _depth);
            Graphics.Blit(src, dest, _material);
        }
        else
        {
            Graphics.Blit(src, dest);
        }
    }
}