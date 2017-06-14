using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SonarCollider : MonoBehaviour
{
    private Sonar _sonar;
    private float _time = 0;
    private List<SphereCollider> _pulses = new List<SphereCollider>();
    private bool _enabled = true;

    public void Start()
    {
        _sonar = FindObjectOfType<underwaterFX>().SonarVals;
        SpawnPulse();
    }

    public void Update()
    {
        CheckSubTap();
        UpdatePulses();
    }

    private void SetSonarActive(bool val)
    {
        _enabled = val;
        _sonar.enabled = val;
    }

    private void CheckSubTap()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = -Camera.main.transform.position.z;
            mousePos = Camera.main.ScreenToWorldPoint(mousePos);

            if (Vector3.Distance(transform.position, mousePos) < 2.5f)
                SetSonarActive(!_enabled);
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

    private void UpdatePulses()
    {
        _time += Time.deltaTime;

        //Time till a new pulse spawns
        if (_time > _sonar.interval && _enabled)
        {
            _time = 0f;
            SpawnPulse();
        }

        for (int i = 0; i < _pulses.Count; i++)
        {
            if (_pulses[i].radius > _sonar.distance)
            {
                Destroy(_pulses[i].gameObject);
                _pulses.RemoveAt(i);
            }
            else
                _pulses[i].radius += Time.deltaTime * _sonar.speed;
        }
    }
}