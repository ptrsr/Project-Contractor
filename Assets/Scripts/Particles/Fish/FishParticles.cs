using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class FishParticles : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField]
    private int _rotateStrength = 15;

    private ParticleSystem _fish;
    private ParticleSystem.Particle[] _particles = new ParticleSystem.Particle[100];
    
    public void Start()
    {
        _fish = GetComponent<ParticleSystem>();
    }

    public void Update()
    {
        int size = _fish.GetParticles(_particles);
        RotateToDirection(size);
        _fish.SetParticles(_particles, size);
    }

    private void RotateToDirection(int size)
    {
        for (int i = 0; i < size; i++)
        {
            _particles[i].rotation = _particles[i].velocity.x * _rotateStrength;
        }
    }
}