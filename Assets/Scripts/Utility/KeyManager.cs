using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

    // Use this for initialization
    private bool _key1PickedUp = false;
    private bool _key2PickedUp = false;
    private bool _open = false;
    private Vector3 _newPos;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_open)
        {
            transform.parent.position = Vector3.Lerp(transform.parent.position, _newPos, 0.01f);
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if(_key1PickedUp && _key2PickedUp)
            {
                Transform parent = transform.parent;
                _newPos = new Vector3(parent.position.x - 70.0f, parent.position.y, parent.position.z);
                _open = true;
            }
        }
    }

    public void PickUpKey1()
    {
        _key1PickedUp = true;
    }
    public void PickUpKey2()
    {
        _key2PickedUp = true;
    }
}
