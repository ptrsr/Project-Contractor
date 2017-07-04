using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinalCameraPlayer : MonoBehaviour {
    
    [SerializeField]
    private Transform _camPos;
    [SerializeField]
    private Transform _playerPos;

    private Camera _cam;
    private camMove _camMove;
    private SubMovement _subMov;

    private bool _update = false;
    void Start()
    {
        _cam = Camera.main;
        _camMove = _cam.GetComponent<camMove>();
        _subMov = FindObjectOfType<SubMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (!_update) return;
        _camMove.FollowAnimation(true);
        if (MoveCamera()) { }
        if (MovePlayer()) { _subMov.Freeze(true); }
       

    }
    private bool MovePlayer()
    {
        _subMov.transform.position = Vector3.Lerp(_subMov.transform.position, _playerPos.position, Time.deltaTime * 2);
        if (Vector3.Distance(_subMov.transform.position, _playerPos.position) < 1)
        {
            return true;
        }
        else { return false; }
    }

    private bool MoveCamera()
    {
        _cam.transform.position = Vector3.Lerp(_cam.transform.position, _camPos.position, Time.deltaTime * 2);
        Quaternion quat = new Quaternion();
        quat.eulerAngles = _camPos.rotation.eulerAngles;
        _cam.transform.rotation = Quaternion.Slerp(_cam.transform.rotation, quat, 0.05f);
        if (Vector3.Distance(_cam.transform.position, _camPos.position) < 1)
        {
            return true;
        }
        else { return false; }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            _update = true;
        }
    }
}
