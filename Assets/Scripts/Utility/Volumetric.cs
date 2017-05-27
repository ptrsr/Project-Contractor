using System.Collections;
using System;
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
        _thick = 1,
        _quality = 1;

    [SerializeField]
    int
        _passes = 1,
        _layers = 1,
        _blurLayers = 1;

    private Camera   _cam;
    private Material _mat;

    private bool _orth = true;

    private RenderTexture[] _pingPong;

    void Start ()
    {
        _cam = GetComponent<Camera>();
        _mat = new Material(_shader);
        _cam.depthTextureMode = DepthTextureMode.Depth;

        _orth = _cam.orthographic;
    }

    void CreateTextures()
    {
        if (_pingPong != null)
            foreach (var texture in _pingPong)
                if (texture.IsCreated())
                    texture.Release();

        _pingPong = new RenderTexture[_blurLayers * 2];

        int divide = 0;

        for (int i = 0; i < _blurLayers * 2; i++)
        {
            if (i % 2 == 0)
                divide++;

            Vector2 size = new Vector2(_size * 2, _height) * (_quality / divide);

            _pingPong[i] = new RenderTexture((int)size.x, (int)size.y, 0);
            _pingPong[i].Create();
        }
    }

    void OnValidate()
    {
        SetDimensions(GetComponent<Camera>());
        CreateTextures();
    }

    void SetDimensions(Camera cam)
    {
        _orth = cam.orthographic;
        
        if (_orth)
        {
            cam.orthographicSize = _size / 2;
            cam.aspect = _layers / _size;

            cam.pixelRect = new Rect(0, 0, _layers, _size * _quality * 2);
            cam.farClipPlane = _height;
        }
        else
        {
            cam.fieldOfView = (_size);
            cam.aspect = 1 / (_size);


            cam.rect = new Rect(new Vector2(), new Vector2(_quality, 0.1f));
            cam.farClipPlane = _height;
        }
    }

    void RenderTriangle()
    {
        Vector3 pos = transform.position;
        Vector3 forward = transform.forward * _height;
        float temp = _height * Mathf.Asin((_size / 2) * Mathf.Deg2Rad);
        Vector3 toTop = transform.up * _height * Mathf.Asin((_size / 2) * Mathf.Deg2Rad);

        _mat.SetPass(1);
        _mat.SetFloat("_height", _height);

        GL.Begin(GL.TRIANGLES);
        {
            GL.TexCoord2(0, 0);
            GL.Vertex(pos);

            GL.TexCoord2(_height, -temp);
            GL.Vertex(pos - toTop + forward);

            GL.TexCoord2(_height, temp);
            GL.Vertex(pos + toTop + forward);
        }
        GL.End();

    }

    public void Render(ref RenderTexture src)
    {
        _cam.Render();

        if (_pingPong == null)
            CreateTextures();

        if (_orth)
            RenderQuad(ref src);
        else
            RenderTriangle();
    }

    void RenderQuad(ref RenderTexture src)
    {
        _mat.SetFloat("_height", _height);
        _mat.SetInt("_layers", _layers);
        _mat.SetFloat("_time", Time.time);

        print(_cam.pixelRect);

        Graphics.Blit(_pingPong[0], _pingPong[0], _mat, 0);

        int nextTexture = _pingPong.Length - 1;
        int currentTexture = 0;

        for (int j = _blurLayers - 1; j >= 0; j--)
        {
            for (int i = 0; i < _passes * 2; i++)
            {
                int pass = i % 2;

                _mat.SetInt("_horizontal", pass);
                _mat.SetVector("_size", new Vector2(1.0f / _pingPong[nextTexture].width, 1.0f / _pingPong[nextTexture].height));

                _mat.SetTexture("_blur", _pingPong[currentTexture]);

                Graphics.Blit(_pingPong[currentTexture], _pingPong[nextTexture], _mat, 1);

                currentTexture = nextTexture;
                nextTexture = j * 2 + (i % 2);
            }
            nextTexture = (j - 1) * 2 + 1;
        }

        RenderTexture.active = src;

        _mat.SetPass(2);
        _mat.SetTexture("_texture", _pingPong[0]);

        Vector3 pos = transform.position;
        Vector3 toRight = transform.up * _size / 2;
        Vector3 toBottom = transform.forward * _height;

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
