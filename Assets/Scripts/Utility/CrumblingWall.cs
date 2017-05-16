using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CrumblingWall : MonoBehaviour {

    private Rigidbody[] _rigidbodies;
    [Tooltip("It takes the velocity of the submarine. If charge speed is 150, then double tap is above 100, while dragging is below so that works as explosive value")]
    [SerializeField]
    private float _explodesOnValue = 100;

	// Use this for initialization
	void Start () {
        _rigidbodies = GetComponentsInChildren<Rigidbody>();
	}
	
	// Update is called once per frame
	void Update () {
		
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

                }
            }
            else
            {

            }
        }
    }
}
