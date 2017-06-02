using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarCollider : MonoBehaviour
{
    private GameObject _pulseObj;
    private float _time = 0;
    private List<SphereCollider> _pulses = new List<SphereCollider>();

    public void Start()
    {
        SpawnPulse();
    }

    public void Update()
    {
        _time += Time.deltaTime;

        for (int i = 0; i < _pulses.Count; i++)
        {
            if (_time > 1.5f)
            {
                _time = 0f;
                SpawnPulse();
            }

            if (_pulses[i].radius > 200)
            {
                Destroy(_pulses[i].gameObject);
                _pulses.RemoveAt(i);
            }
            else
                _pulses[i].radius += Time.deltaTime * 70f;
        }
    }

    private void SpawnPulse()
    {
        GameObject pulseObj = new GameObject("Pulse");
        pulseObj.transform.position = transform.position;
        pulseObj.layer = 8;
        pulseObj.tag = "Pulse";
        SphereCollider pulse = pulseObj.AddComponent<SphereCollider>();
        pulse.isTrigger = true;
        pulse.radius = 2;

        _pulses.Add(pulse);
    }
}