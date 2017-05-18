using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OxygenCrack : MonoBehaviour {

    [SerializeField]
    private GameObject _oxygenPrefab;

    [SerializeField]
    private int _oxygenLifetime = 300;
    [SerializeField]
    private int _minRotPossible = 0;
    [SerializeField]
    private int _maxRotPossible = 0;
    [SerializeField]
    private int _maxDelay = 0;
    [SerializeField]
    private float _speed = 0.2f;
    [SerializeField]
    private float _smoothnessOfTurningUp = 0.1f;

    private Vector3 _randomRotation;

    private Quaternion _rotation;
    private Quaternion _zero;

    private int _counter = 0;
    private int _delay = 0;

    private List<GameObject> _oxygens = new List<GameObject>();
    private List<int> _lifetimes = new List<int>();

	// Use this for initialization
	void Start () {
        GetRandomDelay();
        _zero = GetQuaternionFromVector(new Vector3(0, 0, 0));
    }
	
	// Update is called once per frame
	void Update () {
        if(_counter >= _delay)
        {
            GetRandomDelay();
            _counter = 0;
            CreateOxygen();
        }
        else {
            _counter++;
            for (int i = 0; i < _oxygens.Count; i++)
            {
                GameObject obj = _oxygens[i];
                obj.transform.Translate(new Vector3(0, _speed, 0));
                obj.transform.rotation = Quaternion.Slerp(obj.transform.localRotation, _zero, _smoothnessOfTurningUp);
                _lifetimes[i] -= 1;
                if(_lifetimes[i] <= 0)
                {
                    _oxygens.RemoveAt(i);
                    _lifetimes.RemoveAt(i);
                    Destroy(obj);
                }
            }

        }
	}

    void CreateOxygen()
    {
        GameObject oxygen = (GameObject)Instantiate(_oxygenPrefab);
        oxygen.transform.position = transform.position;
        oxygen.transform.rotation = GetRandomRot();
        _oxygens.Add(oxygen);
        _lifetimes.Add(_oxygenLifetime);
    }

    private void GetRandomDelay()
    {
        _delay = Random.Range(0, _maxDelay);
    }

    private Quaternion GetRandomRot()
    {
        _randomRotation = new Vector3(0, 0, Random.Range(_minRotPossible, _maxRotPossible));
        _rotation = GetQuaternionFromVector(_randomRotation);
        return _rotation;
    }

    private Quaternion GetQuaternionFromVector(Vector3 vec3)
    {
        Quaternion quat = new Quaternion();
        quat.eulerAngles = vec3;
        return quat;
    }
}
