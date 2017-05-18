using UnityEngine;
using System.Collections;
public class Camera_Movement : MonoBehaviour {

//	public Vector3 _topView = new Vector3 (0, 0, 0);

	public float smoothness = 0.1f;
	public Vector3 perspective = new Vector3 (0, 0, 0);
	public Vector3 offset = new Vector3 (0, 0, 0);
	public Vector3 newPerspective = new Vector3 (0, 0, 0);
	public Transform newTarget;

	private GameObject _dynamicTarget;
	private GameObject player;
	private GameObject _camTarget;
	private Vector3 wantedDistance;
	private Vector3 currentDistance;
	private Vector3 wantedPosition;

	void Start () {
		
		setupMultiple ("camtarget", 0.5f, new Color (0, 1, 0));
		_camTarget = GameObject.Find ("camtarget");
		setupMultiple ("dynamictarget", 0.5f, new Color (1, 0, 0));
		_dynamicTarget = GameObject.Find ("dynamictarget");

		player = GameObject.FindGameObjectWithTag ("Player");
	}

	void FixedUpdate () {
		_camTarget.transform.position = new Vector3 (player.transform.position.x + offset.x, 
			player.transform.position.y + offset.y, player.transform.position.z + offset.z);
		
		if (newTarget != null)
			wantedDistance = newTarget.position + newPerspective;
		else
			wantedDistance = _camTarget.transform.position + perspective;
		currentDistance = Vector3.Lerp (currentDistance, wantedDistance, smoothness);
		transform.position = currentDistance;

		if (newTarget != null)
			wantedPosition = newTarget.position;
		else
			wantedPosition = _camTarget.transform.position;
		_dynamicTarget.transform.position = Vector3.Lerp (_dynamicTarget.transform.position, 
			wantedPosition, smoothness);
		
		transform.LookAt (_dynamicTarget.transform);
	}	
		
	public void setupMultiple (string name, float size, Color color) {
		GameObject o = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		o.GetComponent<SphereCollider> ().enabled = false;
		o.transform.localScale = new Vector3 (size, size, size);
		o.name = name;
		o.GetComponent<MeshRenderer> ().sharedMaterial.color = color;
	}

}