using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FishParticles : MonoBehaviour
{
    private ParticleSystem _fish;
    private ParticleSystem.Particle[] _particles;
    
    public void Start()
    {
        _fish = GetComponent<ParticleSystem>();
        _fish.GetParticles(_particles);
    }

    public void Update()
    {

        var main = _fish.main;

    }
}