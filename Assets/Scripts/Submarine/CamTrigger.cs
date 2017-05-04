using UnityEngine;
using System.Collections;
public class CamTrigger : MonoBehaviour {

	private GameObject cam;
	public Vector3 distanceVector = new Vector3 (0, 0, 0);
    private Camera_Movement _camScript;
	void Start () {
		cam = GameObject.FindGameObjectWithTag("MainCamera");
        _camScript = cam.GetComponent<Camera_Movement>();

    }
	void OnTriggerStay (Collider col) {
		if (col.tag == "Player") {
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