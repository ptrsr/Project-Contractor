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
        KeyManager[] km = FindObjectsOfType<KeyManager>();
        if (_id == 1)
        {
            if (km[0].ID == 1)
            {
                _keyManager = km[0];
            }
            else if (km[1].ID == 1)
            {
                _keyManager = km[1];
			}
			else if (km[2].ID == 1)
			{
				_keyManager = km[2];
			}
        }
        else if (_id == 2)
        {
            if (km[0].ID == 2)
            {
                _keyManager = km[0];
            }
            else if (km[1].ID == 2)
            {
                _keyManager = km[1];
			}
			else if (km[2].ID == 2)
			{
				_keyManager = km[2];
			}
		}
		else if (_id == 3)
		{
			if (km[0].ID == 3)
			{
				_keyManager = km[0];
			}
			else if (km[1].ID == 3)
			{
				_keyManager = km[1];
			}
			else if (km[2].ID == 3)
			{
				_keyManager = km[2];
			}
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {
        if (_pickedUp && !_keyManager.Open)
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
        if (_id == 1) { _keyManager.PickUpKey1(this.gameObject); }
		else if(_id == 2) { _keyManager.PickUpKey2(this.gameObject); }
		else if(_id == 3) { _keyManager.PickUpKey3(this.gameObject); }
    }
}
