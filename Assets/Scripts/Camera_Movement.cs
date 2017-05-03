using UnityEngine;
using System.Collections;
public class Camera_Movement : MonoBehaviour {
    [SerializeField]
	private Transform _target;
    [SerializeField]
    private Vector3 _distanceVector = new Vector3 (0, 0, 0);
    [SerializeField]
    private Vector3 _distanceView = new Vector3 (0, 0, 0);
    [SerializeField]
    private float _smoothness = 0.1f;
    private Vector3 _distanceOnExit = new Vector3(0, 0, 0);
	private GameObject _dynamicTarget;
	private GameObject _player;
	private GameObject _camTarget;
	private Vector3 _wantedDistance;
	private Vector3 _currentDistance;
	private Vector3 _wantedPosition;

    public Transform Target {
        get { return _target; }
        set { _target = value; }
    }
    public Vector3 DistanceView
    {
        get { return _distanceView; }
        set { _distanceView = value; }
    }
    public Vector3 DistanceOnExit
    {
        get { return _distanceOnExit; }
        set { _distanceOnExit = value; }
    }


    void Start () {
        _distanceOnExit = _distanceView;
		_player = GameObject.FindGameObjectWithTag ("Player");
		_camTarget = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		_camTarget.GetComponent<MeshRenderer> ().enabled = false;
		_camTarget.GetComponent<SphereCollider> ().enabled = false;
		_camTarget.name = "_camTarget";
		_camTarget.transform.SetParent (transform);
		_camTarget.transform.position = _player.transform.position;
		_dynamicTarget = GameObject.CreatePrimitive (PrimitiveType.Sphere);
		_dynamicTarget.GetComponent<MeshRenderer> ().enabled = false;
		_dynamicTarget.GetComponent<SphereCollider> ().enabled = false;
		_dynamicTarget.name = "_dynamicTarget";
		_dynamicTarget.transform.position = _camTarget.transform.position;
	}
	void FixedUpdate () {
		_camTarget.transform.position =_player.transform.position;
		if (_target != null)
			_wantedDistance = _target.position + _distanceView;
		else
			_wantedDistance = _camTarget.transform.position + _distanceVector;
		_currentDistance = Vector3.Lerp (_currentDistance, _wantedDistance, _smoothness);
		transform.position = _currentDistance;
		if (_target != null)
			_wantedPosition = _target.position;
		else
			_wantedPosition = _target.position + _distanceView;
		_dynamicTarget.transform.position = Vector3.Lerp (_dynamicTarget.transform.position, 
			_wantedPosition, _smoothness);
	}	
}