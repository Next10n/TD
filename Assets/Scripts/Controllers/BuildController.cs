using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildController : MonoBehaviour
{
    [Inject]
    private GameObject _gamePrefab;

    [SerializeField]
    private Transform _currentBuilding;

    private RaycastHit _hit;


    private void Update()
    {
        
        if (_currentBuilding != null)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out _hit);
            _currentBuilding.position = new Vector3(_hit.point.x, _hit.point.y + 1, _hit.point.z);
        }
    }

    public void SetBulding(Transform build)
    {
        _currentBuilding = build;
    }

    public class Factory : PlaceholderFactory<GameObject, BuildController>
    {

    }

}
