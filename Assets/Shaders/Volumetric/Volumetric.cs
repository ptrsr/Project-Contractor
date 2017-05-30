using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public abstract class Volumetric : MonoBehaviour
{
    [SerializeField]
    protected Shader _shader;

    protected Camera   _cam;
    protected Material _mat;

    protected RenderTexture[] _pingPong;

    void Start ()
    {
        _cam = GetComponent<Camera>();
        _mat = new Material(_shader);
        _cam.depthTextureMode = DepthTextureMode.Depth;
    }

    void OnValidate()
    {
        SetDimensions(GetComponent<Camera>());
        CreateTextures();
    }

    abstract protected void CreateTextures();

    abstract protected void SetDimensions(Camera cam);

    virtual public void Render(ref RenderTexture src)
    {
        _cam.Render();

        if (_pingPong == null)
            CreateTextures();
    }
}
