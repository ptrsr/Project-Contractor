using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Volumetric : MonoBehaviour
{
    [SerializeField]
    Shader _shader;

    [SerializeField]
    float
        _size = 2,
        _height = 2,
        _quality = 1;

    private Camera   _cam;
    private Material _mat;

    private Vector2 _quadSize;

    private bool _orth = true;

    void Start ()
    {
        _cam = GetComponent<Camera>();
        _mat = new Material(_shader);
        _cam.depthTextureMode = DepthTextureMode.Depth;

        _orth = _cam.orthographic;

        print(_orth);
    }

    void Update ()
    {
        
    }

    void OnValidate()
    {
        SetDimensions(GetComponent<Camera>());
    }

    void SetDimensions(Camera cam)
    {
        _orth = cam.orthographic;

        cam.orthographicSize = _size / 2;
        cam.aspect = 1 / _size;

        cam.rect = new Rect(new Vector2(), new Vector2(_quality, 1));
        cam.farClipPlane = _height;
    }

    void RenderTriangle()
    {
        Vector3 pos = transform.position;
        Vector3 toRight = transform.up * Mathf.Asin(_height) / _size;

    }

    public void Render()
    {
        _cam.Render();

        if (_orth)
            RenderQuad();
        else
            RenderTriangle();
    }

    void RenderQuad()
    {
        Vector3 pos = transform.position;
        Vector3 toRight = transform.up * _size / 2;
        Vector3 toBottom = transform.forward * _height;

        _mat.SetPass(0);
        _mat.SetFloat("_height", _height);

        GL.Begin(GL.QUADS);
        {
            GL.TexCoord2(0, 0);
            GL.Vertex(pos - toRight);

            GL.TexCoord2(1, 0);
            GL.Vertex(pos + toRight);

            GL.TexCoord2(1, 1);
            GL.Vertex(pos + toRight + toBottom);

            GL.TexCoord2(0, 1);
            GL.Vertex(pos - toRight + toBottom);
        }
        GL.End();
    }
}
