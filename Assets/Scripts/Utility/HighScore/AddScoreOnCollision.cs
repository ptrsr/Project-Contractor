using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class AddScoreOnCollision : MonoBehaviour {

    [SerializeField]
    private int _scoreToAdd = 0;

    private HighScoreManager _manager;

    
	void Start () {
        _manager = FindObjectOfType<HighScoreManager>();	
	}

    private void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.CompareTag("Player"))
        {
            gameObject.SetActive(false);
            _manager.AddScore(_scoreToAdd);
        }


    }



}
