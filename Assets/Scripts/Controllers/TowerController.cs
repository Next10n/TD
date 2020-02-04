using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class TowerController : MonoBehaviour
{
    [Inject]
    protected float _radius;
    [Inject]
    protected int _towerDamage;
    [Inject]
    protected int _towerID;
    [Inject]
    protected float _hitCouldown;
    [Inject]
    protected PlayerSelector _playerID;

    [Inject]
    private BulletController.Factory _bulletFactory;
    [Inject]
    private SignalBus _signalBus;

    [SerializeField]
    private Transform _bulletStartPosition;
    protected BuildMaterial _buildMaterial;
    private bool _canShoot = true;
    private int _towerLvl;
    private Transform _target;
    private bool _isTarget = false;
    private bool _builded = false;
    private bool _canBuild = true;
    private RaycastHit hit;
    private Material _redMaterial;
    private Material _greenMaterial;
    private Material _playerMaterial;
    private BoxCollider _boxCollider;
    private Renderer render;

    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        render = GetComponent<Renderer>();
        _redMaterial = Resources.Load("Materials/Red") as Material;
        _greenMaterial = Resources.Load("Materials/Green") as Material;
        if (_playerID == PlayerSelector.Player1)
            _playerMaterial = Resources.Load("Materials/Player1Color") as Material;

        if (_playerID == PlayerSelector.Player2)
            _playerMaterial = Resources.Load("Materials/Player2Color") as Material;

        render.material = _greenMaterial;

        _signalBus.Subscribe<UnitDestroySignal>(ResetTarget);

    }

    private void FixedUpdate()
    {
        if (!_builded)
        {
            Build();
            return;
        }
        if(_isTarget)
        {
            Shoot();
        }
         
    }

    private void SetColliderRaduis()
    {
        GetComponent<SphereCollider>().radius = _radius;
    }

    private void Build()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(ray, out hit);
        transform.position = new Vector3(hit.point.x, hit.point.y + 1, hit.point.z);
        if (_canBuild)
        {
            render.material = _greenMaterial;
            if (Input.GetMouseButtonDown(0))
            {
                gameObject.AddComponent<SphereCollider>().isTrigger = true;
                SetColliderRaduis();
                GetComponent<NavMeshObstacle>().enabled = true;
                GetComponent<BoxCollider>().isTrigger = true;

                render.material = _playerMaterial;
                _builded = true;
            }
            if (Input.GetMouseButtonDown(1))
            {
                Destroy(this.gameObject);
            }

        }
        else
        {
            render.material = _redMaterial;
        }

    }   

    private bool CheckBuild()
    {

        return true;
    }

    private void Shoot()
    {
        if (_canShoot)
        {
            _bulletFactory.Create(10, 1, _target, _bulletStartPosition);
            _canShoot = false;
            Invoke(nameof(CanShoot), _hitCouldown);
        }
    }

    private void CanShoot()
    {
        _canShoot = true;
    }

    public class Factory : PlaceholderFactory<float, int, int, float, PlayerSelector, TowerController>
    {

    }

    public void SetTarget(Transform target, PlayerSelector playerID)
    {
        if(playerID != _playerID)
        {
            if(_target == null)
            {
                _isTarget = true;
                _target = target;
            }  
        }
          
    }

    public void ResetTarget(Transform losingTarget, PlayerSelector playerID)
    {
        if (playerID != _playerID)
        {
            if (_target == losingTarget)
            {
                _isTarget = false;
                _target = null;
            }
        }
    }

    public void ResetTarget()
    {
        _isTarget = false;
        _target = null;
    }




    private void OnTriggerEnter(Collider other)
    {
        if (other.name.Contains("Unit"))
        {
            _canBuild = false;
        }

        if (other is BoxCollider)
        {
            _canBuild = false;
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.name.Contains("Unit"))
        {
            _canBuild = true;
        }

        if (other is BoxCollider)
        {
            _canBuild = true;
        }
    }



    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<UnitDestroySignal>(ResetTarget);
    }
   


    public int GetTowerID()
    {
        return _towerID;
    }

}
