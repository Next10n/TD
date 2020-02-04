using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class BuildController : MonoBehaviour
{
    [Inject]
    private GameObject _gamePrefab;

    [SerializeField]
    private Transform currentBuilding;

    private RaycastHit hit;
    void Start()
    {
        
    }


    void Update()
    {
        
        if (currentBuilding != null)
        {

            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            Physics.Raycast(ray, out hit);
            currentBuilding.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
        }
    }

    public void SetBulding(Transform build)
    {
        currentBuilding = build;
    }

    public class Factory : PlaceholderFactory<GameObject, BuildController>
    {

    }

}
