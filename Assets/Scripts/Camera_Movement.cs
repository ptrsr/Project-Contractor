using UnityEngine;
using System.Collections;
public class Camera_Movement : MonoBehaviour {
	private Transform _target;
    [SerializeField]
    private Vector3 _distanceOnExit = new Vector3 (0, 0, 0);
    [SerializeField]
    private Vector3 _distanceView = new Vector3 (0, 0, 0);
    [SerializeField]
    private float _smoothness = 0.1f;
	private GameObject _player;
	private Vector3 _wantedDistance;
	private Vector3 _currentDistance;

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
		_player = GameObject.FindGameObjectWithTag ("Player");
        _target = _player.transform;
    }
	void FixedUpdate () {
		if (_target != null)
        {
            _wantedDistance = _target.position + _distanceView;
            _currentDistance = Vector3.Lerp(_currentDistance, _wantedDistance, _smoothness);
            transform.position = _currentDistance;
        }
			
	}	
}