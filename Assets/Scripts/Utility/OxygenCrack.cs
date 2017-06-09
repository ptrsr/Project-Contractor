using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCrack : MonoBehaviour {

    [SerializeField]
    private GameObject _oxygenPrefab;

    private int _counter = 0;
    private int _delay = 0;
    private bool _increaseSize = false;
    private bool _pickUp = false;
    Vector3 pos = new Vector3(0, 0, 0);

    private GameObject _oxygen;
    private List<int> _lifetimes = new List<int>();

	// Use this for initialization
	void Start () {

    }
	
	// Update is called once per frame
	void Update () {
        if (_increaseSize)
        {
            _oxygen.transform.position = Vector3.Lerp(_oxygen.transform.position, new Vector3(pos.x, pos.y + 5, pos.z), 0.03f);
            _oxygen.transform.localScale = Vector3.Lerp(_oxygen.transform.localScale, new Vector3(5, 5, 5), 0.03f);
        }
        else if (_pickUp)
        {

        }
        if (Input.GetKeyDown(KeyCode.O))
        {
            CreateOxygen();
        }
	}

    void CreateOxygen()
    {
        _oxygen = (GameObject)Instantiate(_oxygenPrefab);
        _oxygen.transform.position = transform.position;
        _oxygen.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        pos = _oxygen.transform.position;
        _increaseSize = true;
    }


    

}
