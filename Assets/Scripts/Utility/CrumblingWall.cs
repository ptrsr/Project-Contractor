using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingWall : MonoBehaviour {

    private Rigidbody[] _rigidbodies;
    private Material[] _mats;
    [Tooltip("It takes the velocity of the submarine. If charge speed is 150, then double tap is above 100, while dragging is below so that works as explosive value")]
    [SerializeField]
    private float _explodesOnValue = 100;
    private bool _exploded = false;

	// Use this for initialization
	void Start () {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
        _mats = new Material[_rigidbodies.Length];
        for (int i = 0; i < _rigidbodies.Length; i++)
        {
            _mats[i] = _rigidbodies[i].gameObject.GetComponent<Material>();
        }
	}
	
	// Update is called once per frame
	void Update () {
        if (!_exploded) return;
		
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Rigidbody rb = other.GetComponent<Rigidbody>();
            Debug.Log(rb.velocity.magnitude);
            if (rb.velocity.magnitude > _explodesOnValue)
            {
                foreach (Rigidbody r in _rigidbodies)
                {
                    r.useGravity = true;
                    r.mass = 5;
                    r.isKinematic = false;
                    r.AddExplosionForce(10000, other.gameObject.transform.position, 5);
                    _exploded = true;
                    

                }
            }
            else
            {

            }
        }
    }
}
