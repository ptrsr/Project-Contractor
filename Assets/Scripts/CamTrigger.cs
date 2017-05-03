using UnityEngine;
using System.Collections;
public class CamTrigger : MonoBehaviour {
	private GameObject cam;
	public Vector3 distanceVector = new Vector3 (0, 0, 0);
	public Vector3 targetOffset = new Vector3 (0, 0, 0);
	private GameObject _triggerTarget;
    private Camera_Movement _camScript;
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
		_triggerTarget = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		_triggerTarget.GetComponent<MeshRenderer> ().enabled = false;
		_triggerTarget.GetComponent<SphereCollider> ().enabled = false;
		_triggerTarget.name = "_triggerTarget";
		_triggerTarget.transform.localPosition = targetOffset;
		_triggerTarget.transform.SetParent (transform);
        _camScript = cam.GetComponent<Camera_Movement>();

    }
	void Update () {
		_triggerTarget.transform.localPosition = targetOffset;
	}
	void OnTriggerStay (Collider col) {
		if (col.tag == "Player") {
            _camScript.DistanceOnExit = _camScript.DistanceView;
            _camScript.DistanceView = distanceVector;

		}
	}
	void OnTriggerExit (Collider col) {
		if (col.tag == "Player") {
            _camScript.Target = col.gameObject.transform;
            _camScript.DistanceView = _camScript.DistanceOnExit;
		}
	}
}