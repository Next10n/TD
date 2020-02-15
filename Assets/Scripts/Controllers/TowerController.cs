using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class TowerController : MonoBehaviour, ISetColor
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
    private bool _isTarget = false; //чтобы не проверять Transform на null
    private bool _builded = false;
    private bool _canBuild = true;
    private RaycastHit hit;
    private Material _redMaterial;
    private Material _greenMaterial;
    private Material _playerMaterial;
    private BoxCollider _boxCollider;
    private Renderer render;
    private Dictionary<int, Transform> _targets = new Dictionary<int, Transform>();

    #region unity
    private void Awake()
    {
        _boxCollider = GetComponent<BoxCollider>();
        render = GetComponent<Renderer>();
        _redMaterial = Resources.Load("Materials/Red") as Material;
        _greenMaterial = Resources.Load("Materials/Green") as Material;

        _signalBus.Subscribe<UnitDestroySignal>(UpdateTarget);
        SetColor();
        render.material = _greenMaterial;

    }

    private void FixedUpdate()
    {
        if (!_builded)
        {
            Build();
            return;
        }
        if (_isTarget)
        {
            Shoot();
        }

    }
    #endregion

    #region public metods

    public void UpdateTarget(UnitDestroySignal args)
    {
        _targets.Remove(args.unitId);
        if (args.towerIds.Contains(_towerID))
        {
            _target = GetNearestTarget();
        }
        if (_target == null)
        {
            _isTarget = false;
        }
    }

    public void UpdateTarget(int targetId, int towerId)
    {
        _targets.Remove(targetId);
        if (_towerID == towerId)
        {
            _target = GetNearestTarget();
        }
        if (_target == null)
        {
            _isTarget = false;
        }
    }

    public class Factory : PlaceholderFactory<float, int, int, float, PlayerSelector, TowerController>
    {

    }

    public void SetTarget(Transform target, PlayerSelector playerID)
    {
        if (playerID != _playerID)
        {
            if (_target == null)
            {
                _isTarget = true;
                _target = target;
            }
        }

    }

    public int GetTowerID()
    {
        return _towerID;
    }

    public void SetColor()
    {
        if (_playerID == PlayerSelector.Player1)
        {
            _playerMaterial = Resources.Load("Materials/Player1Color") as Material;
        }

        if (_playerID == PlayerSelector.Player2)
        {
            _playerMaterial = Resources.Load("Materials/Player2Color") as Material;
        }
    }

    #endregion

    #region private metods

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

    private Transform GetNearestTarget()
    {
        float minDistance = _radius * 100; // костыль
        float distance = 0;
        Transform nearestTarget = null;
        Vector3 position = new Vector3(transform.position.x, transform.position.y, transform.position.z);

        foreach (KeyValuePair<int, Transform> target in _targets)
        {
            distance = Vector3.Distance(position, new Vector3(target.Value.position.x, target.Value.position.y, target.Value.position.z));
            if (distance < minDistance)
            {
                nearestTarget = target.Value;
            }
        }

        return nearestTarget;
    }

    private bool CheckBuild()
    {
        return true;
    }

    private void Shoot()
    {
        if (_canShoot)
        {
            _bulletFactory.Create(_towerID, 10, 1, _target, _bulletStartPosition); //тут надо бы в конфиге задать параметры для пули
            _canShoot = false;
            Invoke(nameof(CanShoot), _hitCouldown);
        }
    }

    private void CanShoot()
    {
        _canShoot = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (_builded == true)
        {
            if (other is SphereCollider)
            {
                if (other.name.Contains("Unit"))
                {
                    UnitController unitController = other.GetComponent<UnitController>();
                    if (unitController.PlayerId != _playerID)
                    {
                        _targets.Add(unitController.Id, unitController.transform);
                    }
                }
            }
        }
        else
        {
            if (other.name.Contains("Unit")) //можно удалить наверное
            {
                _canBuild = false;
            }

            if (other is BoxCollider)
            {
                _canBuild = false;
            }
        }


    }


    private void OnTriggerExit(Collider other)
    {
        if (_builded == true)
        {
            if (other is SphereCollider)
            {
                if (other.name.Contains("Unit"))
                {
                    UnitController unitController = other.GetComponent<UnitController>();
                    if (unitController.PlayerId != _playerID)
                    {
                        _targets.Remove(unitController.Id);
                    }


                }
            }
        }
        else
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

    }

    private void OnDestroy()
    {
        _signalBus.TryUnsubscribe<UnitDestroySignal>(UpdateTarget);
    }

    #endregion

}
