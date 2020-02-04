using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private Camera _camera;
    [SerializeField]
    private float _cameraMaxFieldOfView = 130;
    [SerializeField]
    private float _cameraMinFieldOfView = 50;
    [SerializeField]
    private float _scale = 3;
    [SerializeField]
    private float _cameraSpeed = 15;
    private float _currentFieldOfView;

    private void Awake()
    {
        _camera = Camera.main;
        _currentFieldOfView = _camera.fieldOfView;
    }

    private void Update()
    {
        _currentFieldOfView -= Input.mouseScrollDelta.y * _scale;
        if (_currentFieldOfView > _cameraMaxFieldOfView)
            _currentFieldOfView = _cameraMaxFieldOfView;
        if (_currentFieldOfView < _cameraMinFieldOfView)
            _currentFieldOfView = _cameraMinFieldOfView;

        if(_camera.fieldOfView > _currentFieldOfView)
        {
            _camera.fieldOfView--;
        }
        else if(_camera.fieldOfView < _currentFieldOfView)
        {
            _camera.fieldOfView++;
        }
        
        if(Input.GetAxis("Horizontal") < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x + _cameraSpeed, transform.position.y, transform.position.z),
                Time.deltaTime * _cameraSpeed);
        }
        if (Input.GetAxis("Horizontal") > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x - _cameraSpeed, transform.position.y, transform.position.z),
                Time.deltaTime * _cameraSpeed);
        }

        if (Input.GetAxis("Vertical") < 0)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, transform.position.y, transform.position.z + _cameraSpeed),
                Time.deltaTime * _cameraSpeed);
        }
        if (Input.GetAxis("Vertical") > 0)
        {
            transform.position = Vector3.MoveTowards(transform.position,
                new Vector3(transform.position.x, transform.position.y, transform.position.z - _cameraSpeed),
                Time.deltaTime * _cameraSpeed);
        }


    }
}
