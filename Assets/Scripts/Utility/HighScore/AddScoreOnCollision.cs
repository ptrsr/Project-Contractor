using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AddScoreOnCollision : MonoBehaviour {

    [SerializeField]
    private int _scoreToAdd = 0;

    private HighScoreManager _manager;
    private TreasureId _id;
    private Vector3 _newPos;

    private bool _update = false;
    
	void Start () {
        _manager = FindObjectOfType<HighScoreManager>();
        _id = GetComponent<TreasureId>();
	}

    private void Update()
    {
        if (_update)
        {
            if(InterpolateWithScale(gameObject,_newPos,new Vector3(0.1f, 0.1f, 0.1f), 0.2f)){
				gameObject.GetComponent<PingInter> ().active = false;
				gameObject.GetComponent<PingInter> ().getPing = 0;

                gameObject.GetComponentInChildren<MeshRenderer>().enabled = false;

                _update = false;
            }
        }
    }
    private bool InterpolateWithScale(GameObject obj, Vector3 pos, Vector3 scale, float distanceToDone)
    {
        obj.transform.position = Vector3.Lerp(obj.transform.position, pos, 0.1f);
        obj.transform.localScale = Vector3.Lerp(obj.transform.localScale, scale, 0.3f);
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
        }


    }



}
