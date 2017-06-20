using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camMove : MonoBehaviour {

	public float smoothness = 0.1f;
	public Vector3 perspective = new Vector3 (0, 0, 0);

	private GameObject _dynaTarget;
	private GameObject _lazyTarget;
	private GameObject player;

    private bool _followingAnimation = false;

	void Start () {
		setupMultiple ("lazyTarget", 0.5f, new Color (0, 0, 1));
		_lazyTarget = GameObject.Find ("lazyTarget");

		setupMultiple ("dynaTarget", 0.5f, new Color (1, 1, 0));
		_dynaTarget = GameObject.Find ("dynaTarget");

		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate () {
		dynaTarget ();
		lazyTarget ();
        if (_followingAnimation) return;
        dynaCam ();
	}


    public void FollowAnimation(bool value)
    {
        _followingAnimation = value;
    }
	private void lazyTarget() {
		Vector3 dir = player.transform.position - _lazyTarget.transform.position;
		float dis = dir.magnitude;
		if (dis > 1)
			_lazyTarget.transform.position += dir.normalized * (dis/5);
	}
		
	private void dynaTarget() {
		_dynaTarget.transform.position = Vector3.Lerp 
			(_dynaTarget.transform.position, _lazyTarget.transform.position, smoothness);
	}

	private void dynaCam() {
		transform.position = _dynaTarget.transform.position + perspective;
		transform.LookAt (player.transform);
	}

	public void setupMultiple (string name, float size, Color color) {
		GameObject o = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		o.GetComponent<SphereCollider> ().enabled = false;
		o.transform.localScale = new Vector3 (size, size, size);
		o.name = name;
		o.GetComponent<MeshRenderer> ().enabled = false;
//		o.GetComponent<MeshRenderer> ().enabled = false;
	}
}