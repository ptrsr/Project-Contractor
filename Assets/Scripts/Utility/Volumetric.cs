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

	void Start ()
    {
        _cam = GetComponent<Camera>();
        _mat = new Material(_shader);
        //_quadSize = new Vector2(_cam._cam.farClipPlane);
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
        cam.orthographicSize = _size / 2;
        cam.aspect = 1 / _size;

        cam.rect = new Rect(new Vector2(), new Vector2(_quality, 1));
        cam.farClipPlane = _height;
    }

    public void RenderQuad()
    {
        Vector3 pos = transform.position;
        Vector3 toRight = transform.right * _size;
        Vector3 toTop = transform.up * _height;

        GL.PushMatrix();
        GL.LoadIdentity();
        GL.Begin(GL.QUADS);
        {
            _mat.SetPass(0);
            GL.Vertex(pos + toRight + toTop);
            GL.Vertex(pos - toRight + toTop);
            GL.Vertex(pos + toRight - toTop);
            GL.Vertex(pos - toRight - toTop);
        }
        GL.End();
        GL.PopMatrix();
    }
}
