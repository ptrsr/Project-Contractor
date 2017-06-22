using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCrack : MonoBehaviour {

    [SerializeField]
    private GameObject _oxygenPrefab;
    [SerializeField]
    private float _smoothness = 0.03f;

    private float _counter = 0;
    private float _delay = 0;
    private bool _increaseSize = false;
    private bool _pickUp = false;
    Vector3 pos = new Vector3(0, 0, 0);
    private float _delayBeforeNew = 120;
    private float _counterBeforeNew = 0;

    private GameObject _oxygen;
    private SubMovement _sub;
    private List<int> _lifetimes = new List<int>();

    private bool _disableCreation = false;

    public bool IsDisabled { get { return _disableCreation; } }

    // Use this for initialization
    void Start () {
        _sub = FindObjectOfType<SubMovement>();
        _delay = (1 / _smoothness);
        CreateOxygen();
    }
	
	// Update is called once per frame
	void Update () {
        if (_disableCreation) return;
        if (_increaseSize)
        {
            _oxygen.transform.position = Vector3.Lerp(_oxygen.transform.position, new Vector3(pos.x, pos.y, pos.z), _smoothness);
            _oxygen.transform.localScale = Vector3.Lerp(_oxygen.transform.localScale, new Vector3(5, 5, 5), _smoothness);
        }
        else if (_pickUp)
        {
            
            pos = _sub.gameObject.transform.position;
            _oxygen.transform.position = Vector3.Lerp(_oxygen.transform.position, new Vector3(pos.x, pos.y, pos.z), 0.2f);
            _oxygen.transform.localScale = Vector3.Lerp(_oxygen.transform.localScale, new Vector3(0.1f, 0.1f, 0.1f), 0.2f);
            if(_counter >= _delay)
            {
                _counter = 0;
                _pickUp = false;
                Destroy(_oxygen);
            }
            else { _counter++; }
        }
        else
        {
            if(_counterBeforeNew > _delayBeforeNew)
            {
                _counterBeforeNew = 0;
                CreateOxygen();
            }
            else {_counterBeforeNew++;  }
        }
	}

    public void DisableCreation()
    {
        _disableCreation = true;
        if(_oxygen != null)
        {
            _oxygen.SetActive(false);
        }
        _sub.LossOfOxygen = 10;
    }

    private void CreateOxygen()
    {
        _oxygen = Instantiate(_oxygenPrefab);
        _oxygen.transform.position = transform.parent.position;
        _oxygen.transform.localScale = new Vector3(0.1f, 0.1f, 0.1f);

        pos = transform.position;
        _increaseSize = true;
    }

    public void PickUp()
    {
        if(_oxygen != null)
        {
            _pickUp = true;
            _increaseSize = false;
        }
    }
    



    

}
