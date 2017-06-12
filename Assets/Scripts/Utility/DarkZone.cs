using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class DarkZone : MonoBehaviour
{
    [SerializeField]
    private Color _color;
    public  Color Color { get { return _color; } }


    [SerializeField]
    private float
        _closeRadius,
        _farRadius;

    public float CloseRadius { get { return _closeRadius; } }
    public float FarRadius { get { return _farRadius; } }


    public Vector4 Position { get{ return new Vector4(transform.position.x, transform.position.y, 0, 0); } }

	void Awake ()
    {
        DarkZones.Add(this);
	}
	
}
