using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Singleton;

public class PingUpdater : MonoBehaviour
{
    private List<Ping> _allPings;
    private List<PingData> _activePings;

    private Material _mat;
    private underwaterFX _fx;

    private Vector3 _pulseOrigin;

    void Start()
    {
        _allPings = Pings.Get();
        _activePings = new List<PingData>();
        _fx = GetComponent<underwaterFX>();
        _mat = _fx._mat;
    }

    void Update()
    {
        UpdateActivePings();
        UpdatePingList();
        UpdateShader();
    }

    void UpdateActivePings()
    {
        for (int i = _activePings.Count - 1; i >= 0; i--)
        {
            PingData ping = _activePings[i];

            if (!ping.Move())
                _activePings.RemoveAt(i);
        }
    }

    void UpdatePingList()
    {
        foreach (Ping ping in _allPings)
        {
            if (!ping._active || !ping.CheckOnScreen())
                continue;

            foreach (Pulse pulse in _fx._pulses)
            {
                if (ping._lastPulse >= pulse._number)
                    continue;

                Vector4 tempOriginPos = pulse._origin;
                Vector2 originPos = new Vector2(tempOriginPos.x, tempOriginPos.y);

                Vector3 tempPingPos = ping.transform.position;
                Vector2 pingPos = new Vector2(tempPingPos.x, tempPingPos.y);

                float distance = Vector2.Distance(originPos, pingPos);

                if (distance > pulse._distance)
                    continue;

                PingData newPing = PingData.Clone(ping._settings);
                newPing._pulseOrigin = pulse._origin;

                _activePings.Add(newPing);
                ping._lastPulse = pulse._number;
            }
        }
    }

    bool TriggerPing(Ping ping)
    {
        
        return false;
    }

    void UpdateShader()
    {

        Color[] colors = new Color[10];
        Vector4[] positions =  new Vector4[10];
        Vector4[] origins = new Vector4[10];
        float[] distances = new float[10];
        float[] maxDistances = new float[10];

        try
        {
            for (int i = 0; i < _activePings.Count; i++)
            {
                PingData ping = _activePings[i];

                colors[i] = ping._color;
                positions[i] = ping._position;
                origins[i] = ping._pulseOrigin;
                distances[i] = ping._currentDistance;
                maxDistances[i] = ping._maxDistance;
            }
        }
        catch { Debug.Break(); }
        distances[_activePings.Count] = -1;

        _mat.SetFloatArray("_pingDistances", distances);
        _mat.SetFloatArray("_pingMaxDistances", maxDistances);
        _mat.SetColorArray("_pingColors", colors);
        _mat.SetVectorArray("_pingPositions", positions);
        _mat.SetVectorArray("_sonarOrigins", origins);
    }
}