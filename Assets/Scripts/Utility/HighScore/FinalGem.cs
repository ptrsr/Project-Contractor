using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalGem : MonoBehaviour {

    // Use this for initialization
    [SerializeField]
    private GameObject _gem;
    private bool _update = false;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_update)
        {
            _gem.transform.position = Vector3.Lerp(_gem.transform.position, transform.position, 0.1f);
            if(Vector3.Distance(_gem.transform.position,transform.position) < 0.3f)
            {
                _update = false;
            }
        }
	}

    public void PlaceFinalGem()
    {
        _update = true;
    }
}
