using UnityEngine;
using System.Collections;
public class CamTrigger : MonoBehaviour {
	
	private Camera cam;
	public Vector3 perspective = new Vector3 (0, 0, 0);
	public Vector3 offset = new Vector3 (0, 0, 0);
	private GameObject _triggerTarget;

	void Start () {
		cam = GameObject.FindObjectOfType<Camera>();
		Targetsetup ("triggertarget", new Color (0, 0, 1));
	}

	void Update () {
		_triggerTarget.transform.localPosition = offset;
	}

	void OnTriggerStay (Collider col) {
		if (col.tag == "Player") {
			cam.GetComponent<Camera_Movement> ().newTarget = _triggerTarget.transform;
			cam.GetComponent<Camera_Movement> ().newPerspective = perspective;
		}
	}

	void OnTriggerExit (Collider col) {
		if (col.tag == "Player") {
			cam.GetComponent<Camera_Movement> ().newTarget = null;
			cam.GetComponent<Camera_Movement> ().newPerspective = new Vector3 (0, 0, 0);
		}
	}

	void Targetsetup (string name, Color color) {
		_triggerTarget = GameObject.CreatePrimitive(PrimitiveType.Sphere);
		_triggerTarget.GetComponent<MeshRenderer> ().enabled = true;
		_triggerTarget.GetComponent<MeshRenderer> ().material.color = color;
		_triggerTarget.transform.localScale = new Vector3 (0.75f, 0.75f, 0.75f);
		_triggerTarget.GetComponent<SphereCollider> ().enabled = false;
		_triggerTarget.name = name;
		_triggerTarget.transform.SetParent (transform);
		_triggerTarget.transform.localPosition = offset;
	}

}