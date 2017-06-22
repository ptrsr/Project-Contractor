using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeOrtho : Volumetric
{
    [SerializeField]
    private float
        _size = 2,
        _height = 2,
        _thick = 1,
        _quality = 1;

    [SerializeField]
    private int
        _passes = 1,
        _layers = 1,
        _blurLayers = 1;

    private Vector3
         _tl,
         _tr,
         _bl,
         _br;

    private BoxCollider2D _collider;

    private bool _done = false;

    override protected void OnValidate()
    {
        base.OnValidate();

        if (Application.isPlaying)
            CreateTextures();
    }

    protected override void CreateTextures()
    {
        _cam.enabled = false;

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

    protected override void applySettings(Camera cam)
    {
        cam.orthographicSize = _size / 2;
        cam.aspect = _thick / _size;

        cam.pixelRect = new Rect(0, 0, _layers, _size * _quality * 2);
        cam.farClipPlane = _height;
    }

    public override void Render()
    {
        if (!_done)
            SetupBox();

        if (!CheckOnScreen())
        {
            _cam.enabled = false;
            return;
        }

        _cam.enabled = true;

        _mat.SetFloat("_height", _height);
        _mat.SetInt("_layers", _layers);
        _mat.SetFloat("_time", Time.time);

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

        _mat.SetPass(2);
        _mat.SetTexture("_texture", _pingPong[0]);

        GL.Begin(GL.QUADS);
        {
            GL.TexCoord2(0, 0);
            GL.Vertex(_tl);

            GL.TexCoord2(1, 0);
            GL.Vertex(_tr);

            GL.TexCoord2(1, 1);
            GL.Vertex(_br);

            GL.TexCoord2(0, 1);
            GL.Vertex(_bl);
        }
        GL.End();
    }

    private bool CheckOnScreen()
    {
        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position;

        Vector3 tl = cam.WorldToViewportPoint(_tl);
        Vector3 tr = cam.WorldToViewportPoint(_tr);
        Vector3 bl = cam.WorldToViewportPoint(_bl);

        if (tl.x <= 1 && tr.x >= 0 && bl.y <= 1 && tl.y >= 0)
            return true;

        return false;
    }

    private void SetupBox()
    {
        Vector3 pos = transform.position;
        Vector3 toRight = transform.up * _size / 2;
        Vector3 toBottom = transform.forward * _height;

         _tl = pos - toRight;
         _tr = pos + toRight;
         _bl = pos - toRight + toBottom;
         _br = pos + toRight + toBottom;

        _done = true;
    }
}
