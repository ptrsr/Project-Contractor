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
    private Light _worldLight;

    [SerializeField]
    private Transform _spotLight;

    private Material _material;

    [SerializeField]
    private float _depth;

    private Camera _camera;

    private void Start()
    {
        _camera = GetComponent<Camera>();
        _camera.depthTextureMode = DepthTextureMode.Depth;

        _material = new Material(_shader);

        _material.SetVector("_startColor", _startColor);
        _material.SetVector("_endColor", _endColor);

    }

    private void Update()
    {
        _depth = -Camera.main.transform.position.y / 80;
        _worldLight.intensity = 1 - _depth;
    }

    //private void OnRenderImage(RenderTexture src, RenderTexture dest)
    //{
    //    if (_shader != null && _depth > 0)
    //    {
    //        _material.SetFloat("_intensity", intensity);
    //        _material.SetFloat("_depth", Mathf.Clamp(_depth, 0, 1));
    //        _material.SetFloat("_zoom", -transform.position.z);

    //        _material.SetVector("_spotPos", WorldToScreen(_spotLight.transform.position));
    //        _material.SetVector("_spotDir", MouseDirection(spotScreenPos));
    //        Graphics.Blit(src, dest, _material);

    //    }
    //    else
    //    {
    //        Graphics.Blit(src, dest);
    //    }
    //}

    private Vector2 MouseDirection(Vector2 objectPos)
    {
        Vector2 mousePos = new Vector2(Input.mousePosition.x / Screen.width, Input.mousePosition.y / Screen.height);
        Vector2 direction = (mousePos - objectPos);
        direction.Normalize();

        print(direction);

        return direction;
    }

    private Vector2 WorldToScreen(Vector3 worldPos)
    {
        Vector3 localPos = _camera.WorldToScreenPoint(worldPos);
        Vector2 screenPos = new Vector2(localPos.x / Screen.width, localPos.y / Screen.height);

        return screenPos;
    }

}