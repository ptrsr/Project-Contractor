using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleBound : MonoBehaviour {

    // Use this for initialization
    private ParticleSystem _ps;
    private Renderer _psRen;
	void Start () {
        _ps = GetComponent<ParticleSystem>();
        _psRen = _ps.GetComponent<Renderer>();
	}
	
	// Update is called once per frame
	void Update () {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[_ps.particleCount];
        int num = _ps.GetParticles(particles);
        for(int i = 0; i < num; i++)
        {
            if (particles[i].position.x > _psRen.bounds.max.x)
            {
                particles[i].remainingLifetime = 0;
            }
            if (particles[i].position.y > _psRen.bounds.max.y)
            {
                particles[i].remainingLifetime = 0;
            }
            if (particles[i].position.x < _psRen.bounds.min.x)
            {
                particles[i].remainingLifetime = 0;
            }
            if (particles[i].position.y < _psRen.bounds.min.y)
            {
                particles[i].remainingLifetime = 0;
            }
        }

        
    }
}
