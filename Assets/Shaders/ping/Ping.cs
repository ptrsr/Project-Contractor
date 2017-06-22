using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

[System.Serializable]
public class PingData
{
    public float
        _maxDistance = 20,
        _currentDistance = 0,
        _speed = 50;

    public Color _color;

    public Vector3
        _position,
        _pulseOrigin;


    public bool Move()
    {
        _currentDistance += _speed * Time.deltaTime;

        if (_currentDistance > _maxDistance * 1.5f)
        {
            _currentDistance = 0;
            return false;
        }
        return true;
    }

    public static PingData Clone(PingData original)
    {
        PingData ping = new PingData();
        ping._color = original._color;
        ping._speed = original._speed;
        ping._position = original._position;

        return ping;
    }
}

public class Ping : MonoBehaviour
{
    public PingData _settings = new PingData();

    [SerializeField]
    private bool _moveable;

    public int _lastPulse = -1;

    public bool _active = true;

    private Vector3
        _tl,
        _tr,
        _bl;

    void Start ()
    {
        Pings.Add(this);
        _settings._position = transform.position;
	}

    void Update()
    {
        if (_moveable)
            _settings._position = transform.position;
    }

    public bool CheckOnScreen()
    {
        return true; //REPLACE

        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position;

        Vector3 tl = cam.WorldToViewportPoint(_tl);
        Vector3 tr = cam.WorldToViewportPoint(_tr);
        Vector3 bl = cam.WorldToViewportPoint(_bl);

        if (tl.x <= 1 && tr.x >= 0 && bl.y <= 1 && tl.y >= 0)
            return true;

        return false;
    }
}