using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyManager : MonoBehaviour {

    // Use this for initialization
    private bool _key1PickedUp = false;
	private bool _key2PickedUp = false;
	private bool _key3PickedUp = false;
    private bool _open = false;
    private Vector3 _newPos;
    private GameObject _key1;
	private GameObject _key2;
	private GameObject _key3;
    private GameObject _key4;
    private Transform _keyPos;
    private SkullDoorAnimator _skullAnimator;
	private ReefAnimation _reefAnimation;
    [SerializeField]
    private int _id;

    public int ID { get { return _id; } }
	void Start () {
        _skullAnimator = GetComponentInParent<SkullDoorAnimator>();
		if (_id == 3) {
			_reefAnimation = GetComponentInParent<ReefAnimation>();
		}
		
	}
	
	// Update is called once per frame
	void Update () {
        if (_open)
        {
            //transform.parent.position = Vector3.Lerp(transform.parent.position, _newPos, 0.01f);
            if (_key1 != null)
            {

                _key1.transform.position = Vector3.Lerp(_key1.transform.position, _keyPos.position, Time.deltaTime * 4);
                if(Vector3.Distance(_key1.transform.position,_keyPos.position) < 1)
                {
                    _skullAnimator.Key1InPlace();
                }
            }
            if (_key2 != null)
            {
                _key2.transform.position = Vector3.Lerp(_key2.transform.position, _keyPos.position, Time.deltaTime * 4);
                if (Vector3.Distance(_key2.transform.position, _keyPos.position) < 1)
                {
                    _skullAnimator.Key2InPlace();
                }
			}
			if (_key3 != null)
			{
				_key3.transform.position = Vector3.Lerp(_key3.transform.position, _keyPos.position, Time.deltaTime * 4);
				if (Vector3.Distance(_key3.transform.position, _keyPos.position) < 1)
				{
					_reefAnimation.Key3InPlace();
				}
			}
            if (_key4 != null)
            {
                _key4.transform.position = Vector3.Lerp(_key4.transform.position, _keyPos.position, Time.deltaTime * 4);
            }
        }
	}

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
			if(_key1PickedUp || _key2PickedUp || _key3PickedUp)
            {

                _open = true;
            }
        }
    }

    public void PickUpKey1(GameObject key1)
    {
        _key1PickedUp = true;
        _keyPos = transform;
        _key1 = key1;
    }
    public void PickUpKey2(GameObject key2)
    {
        _key2PickedUp = true;
        _keyPos = transform;
        _key2 = key2;
    }

	public void PickUpKey3(GameObject key3)
	{
		_key3PickedUp = true;
		_keyPos = transform;
		_key3 = key3;
	}
    public void PickUpKey4(GameObject key4)
    {
        _key3PickedUp = true;
        _keyPos = transform;
        _key4 = key4;
    }


    public bool Open { get { return _open; } }
}
