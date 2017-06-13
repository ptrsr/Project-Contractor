using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DustParticleManager : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private GameObject[] _dustParticleSystems;
    private Renderer[] _childrenRenderers;
	void Awake () {
        _childrenRenderers = new Renderer[_dustParticleSystems.Length * 4];
		for(int i = 0; i < _dustParticleSystems.Length; i++)
        {
            int counter = 0;
            if (i != 0) counter = i * 4; 
            ParticleSystem ps = _dustParticleSystems[i].GetComponent<ParticleSystem>();
            Renderer ren = ps.GetComponent<Renderer>();
            GameObject object1 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            object1.transform.parent = _dustParticleSystems[i].transform;
            GameObject object2 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            object2.transform.parent = _dustParticleSystems[i].transform;
            GameObject object3 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            object3.transform.parent = _dustParticleSystems[i].transform;
            GameObject object4 = GameObject.CreatePrimitive(PrimitiveType.Cube);
            object4.transform.parent = _dustParticleSystems[i].transform;
            object1.transform.position += new Vector3(ren.bounds.max.x,ren.bounds.max.y,0);
            object1.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            object2.transform.position += new Vector3(ren.bounds.min.x, ren.bounds.max.y, 0);
            object2.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            object3.transform.position += new Vector3(ren.bounds.max.x, ren.bounds.min.y, 0);
            object3.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            object4.transform.position += new Vector3(ren.bounds.min.x, ren.bounds.min.y, 0);
            object4.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
            _childrenRenderers[counter] = object1.GetComponent<MeshRenderer>();
            _childrenRenderers[counter + 1] = object2.GetComponent<MeshRenderer>();
            _childrenRenderers[counter + 2] = object3.GetComponent<MeshRenderer>();
            _childrenRenderers[counter + 3] = object4.GetComponent<MeshRenderer>();

        }
	}
	
	// Update is called once per frame
	void Update () {
        for(int i = 0; i < _dustParticleSystems.Length; i++)
        {
            int counter = 0;
            if (i != 0) counter = i * 4;

            if (_childrenRenderers[counter].isVisible ||
                _childrenRenderers[counter + 1].isVisible ||
                _childrenRenderers[counter + 2].isVisible ||
                _childrenRenderers[counter + 3].isVisible)
            {
                _dustParticleSystems[i].GetComponent<ParticleSystem>().Play();
            }
            else
            {
                _dustParticleSystems[i].GetComponent<ParticleSystem>().Stop(true,ParticleSystemStopBehavior.StopEmittingAndClear);
            }
        }
    }
}
