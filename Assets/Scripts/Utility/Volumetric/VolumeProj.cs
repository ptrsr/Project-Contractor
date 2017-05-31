using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VolumeProj : Volumetric
{
    [SerializeField]
    float
        _angle = 2,
        _distance = 2,
        _thick = 1,
        _quality = 1;

    [SerializeField]
    int
        _passes = 1,
        _layers = 1,
        _blurLayers = 1;

    protected override void CreateTextures()
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

            Vector2 size = new Vector2(_angle * 2, _distance) * (_quality / divide);

            _pingPong[i] = new RenderTexture((int)size.x, (int)size.y, 0);
            _pingPong[i].Create();
        }
    }

    protected override void SetDimensions(Camera cam)
    {
        cam.fieldOfView = (_angle);
        cam.aspect = 1 / (_angle);

        cam.rect = new Rect(new Vector2(), new Vector2(_quality, 0.1f));
        cam.farClipPlane = _distance;
    }

    public override void Render(ref RenderTexture src)
    {
        base.Render(ref src);

        _mat.SetPass(3);

        Vector3 pos = transform.position;
        Vector3 forward = transform.forward;
        float angle = Mathf.Asin((_angle / 2) * Mathf.Deg2Rad);
        Vector3 toTop = transform.up * angle;

        float near = _cam.nearClipPlane;
        float far  = _cam.farClipPlane;

        Vector2 texCoord = new Vector2(0, 1) * angle * near + new Vector2(near, 0);
        Vector2 fTexCoord = new Vector2(0, -1) * angle * far + new Vector2(far, 0);

        _mat.SetVector("_nCorner", texCoord);
        _mat.SetFloat("_far", Vector2.Distance(texCoord, fTexCoord));
        _mat.SetFloat("_cDir", angle);
        _mat.SetFloat("_dist", _distance - near);

        GL.Begin(GL.QUADS);
        {
            GL.TexCoord2(texCoord.x, texCoord.y);
            GL.Vertex(pos + forward * near + toTop * near);

            texCoord = new Vector2(0, -1) * angle * near + new Vector2(near, 0);
            GL.TexCoord2(texCoord.x, texCoord.y);
            GL.Vertex(pos + forward * near - toTop * near);

            GL.TexCoord2(fTexCoord.x, fTexCoord.y);
            GL.Vertex(pos + forward * far - toTop * far);

            texCoord = new Vector2(0, 1) * angle * far + new Vector2(far, 0);
            GL.TexCoord2(texCoord.x, texCoord.y);
            GL.Vertex(pos + forward * far + toTop * far);
        }
        GL.End();
    }
}