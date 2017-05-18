using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyPickup : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private int _id;
    private bool _pickedUp = false;
    private Transform _chaseTarget;
    private KeyManager _keyManager;
	void Start () {
        _keyManager = FindObjectOfType<KeyManager>();
	}
	
	// Update is called once per frame
	void Update () {
        if (_pickedUp)
        {
            transform.position = Vector3.Lerp(transform.position, _chaseTarget.position, 0.1f);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if(other.CompareTag("Player"))
        {
            PickUp(other.transform);
        }
    }

    private void PickUp(Transform chaseTarget)
    {
        _pickedUp = true;
        _chaseTarget = chaseTarget;
        GetComponent<Collider>().enabled = false;
        if (_id == 1) { _keyManager.PickUpKey1(); }
        else if(_id == 2) { _keyManager.PickUpKey2(); }
    }
}
