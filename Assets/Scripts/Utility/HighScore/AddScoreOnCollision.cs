using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AddScoreOnCollision : MonoBehaviour {

    [SerializeField]
    private int _scoreToAdd = 0;

    public int Score { get { return _scoreToAdd; } }

    private HighScoreManager _manager;
    private TreasureId _id;
    private Vector3 _newPos;
    private PickUp _pickUp;

    private bool _update = false;
    
	void Start () {
        _manager = FindObjectOfType<HighScoreManager>();
        _id = GetComponent<TreasureId>();
        _pickUp = GetComponent<PickUp>();
	}

    private void Update()
    {
        if (_update)
        {
            if(_pickUp.Finished){

                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;
                gameObject.GetComponentInChildren<Light>().enabled = false;
                _update = false;
            }
        }
    }
    private bool InterpolateWithScale(GameObject obj, Vector3 pos, Vector3 scale, float distanceToDone)
    {
        obj.transform.position = Vector3.Lerp(obj.transform.position, pos, Time.deltaTime);
        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, scale, Time.deltaTime * 2);
        if (Vector3.Distance(obj.transform.position, pos) < distanceToDone)
        {
            return true;
        }
        else { return false; }
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.GetComponent<BoxCollider>().enabled = false;
            switch (_id.ID)
            {
                case 1:
                    _manager.Treasure1Pickup(gameObject);
                    break;
                case 2:
                    _manager.Treasure2Pickup(gameObject);
                    break;
                case 3:
                    _manager.Treasure3Pickup(gameObject);
                    break;
                case 4:
                    _manager.Treasure4Pickup(gameObject);
                    break;
                default:
                    Debug.Log("Error on treasure ID");
                    break;
            }
            _update = true;
            _newPos = collider.transform.position;
            _manager.AddScore(_scoreToAdd);
            _pickUp.Pick();
            
        }


    }



}
