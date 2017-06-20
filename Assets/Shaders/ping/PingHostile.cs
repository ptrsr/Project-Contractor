using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PingHostile : MonoBehaviour {

	public float distance = 20;
	public float speed = 50;

	private Transform player;
	private underwaterFX sonar;

	private Vector3 _origin;
	public Vector3 getOrigin{get{ return _origin; }}

	private float ping = 0;
	public float getPing{ get { return ping; } set { ping = value; } }

	private int maxPulses;
	private int pingHits = 0;
	private bool pinged = false;

	private float dist = 0;

	void Awake () {
		// define origin on awake so data is available on start
		_origin = transform.position;
	}

	void Start () {
		sonar = Camera.main.GetComponent<underwaterFX> ();
		player = GameObject.FindGameObjectWithTag ("Player").transform;
		maxPulses = sonar.SonarVals.maxPulses;
	}

	void Update ()
    {
		PingCheck ();
		PingUpdate ();
		_origin = transform.position;
	}

	void PingCheck () {
		if (pinged)
			return;
		
		dist = Vector3.Distance (_origin, player.position);
		for (int i = 0; i < maxPulses; i++) {
			if (dist - 1 < sonar.aPulse [i] && dist + 1 > sonar.aPulse[i])
				pinged = true;
		}
	}

	void PingUpdate () {
		if (pinged) {
			ping += Time.deltaTime * speed;
			if (ping > distance) {
				pinged = false;
				ping = 0;
			}
		}
	}

    public bool CheckOnScreen()
    {
        Camera cam = Camera.main;
        Vector3 pos = cam.transform.position;

        Vector3 tl = cam.WorldToViewportPoint(new Vector3(dist, dist, 0) + _origin);
        Vector3 tr = cam.WorldToViewportPoint(new Vector3(-dist, dist, 0) + _origin);
        Vector3 bl = cam.WorldToViewportPoint(new Vector3(-dist, -dist, 0) + _origin);

        if (tl.x <= 1 && tr.x >= 0 && bl.y <= 1 && tl.y >= 0)
            return true;

        return false;
    }

}