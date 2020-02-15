using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class UnitController : MonoBehaviour, ISetColor, ISetDamage
{
    
    [Inject]
    private GameConfig _gameConfig;
    [Inject]
    protected int _id;
    [Inject]
    protected float _speed;
    [Inject]
    protected int _health;
    [Inject]
    protected int _damage;
    [Inject]
    [SerializeField]
    protected PlayerSelector _playerID;
    [Inject]
    private SignalBus _signalBus;

    public int Id => _id;

    private List<int> _lastHitTowersId = new List<int>(); //список таверов, которые хитали
    private Transform _target;
    private NavMeshAgent agent;

    public PlayerSelector PlayerId => _playerID;

    private void Awake()
    {
        OnCreate();
    }


    public void SetTarget() => _target = (PlayerSelector.Player1 == _playerID) ? GameObject.Find("Base2").transform : GameObject.Find("Base1").transform;

    public void SetColor()
    {
        MeshRenderer mr = GetComponent<MeshRenderer>();
        if (_playerID == PlayerSelector.Player1)
        {
            mr.material = _gameConfig.Player1Color;
        }
        if (_playerID == PlayerSelector.Player2)
        {
            mr.material = _gameConfig.Player2Color;
        }
    }

    public void SetDamage(int damage, int towerId)
    {
        _lastHitTowersId.Add(towerId);
        _health -= damage;
        if (_health <= 0)
        {            
            //GetComponent<MeshRenderer>().enabled = false;
            Destroy(this.gameObject); //это костыль пока что
            // TODO далее пулинг
        }
    }


    public class Factory : PlaceholderFactory<int, float, int, int, PlayerSelector, UnitController>
    {

    }

    private void OnCreate()
    {
        TagUnit();
        SetColor();
        SetStartPosition();
        SetTarget();
        agent = GetComponent<NavMeshAgent>();
        agent.SetDestination(_target.position);        
    }

    private void SetStartPosition()
    {
        if (_playerID == PlayerSelector.Player1)
        {
            transform.position = GameObject.Find("Base1").transform.position;
        }
        if (_playerID == PlayerSelector.Player2)
        {
            transform.position = GameObject.Find("Base2").transform.position;
        }
    }

    private void TagUnit()
    {
        if (_playerID == PlayerSelector.Player1)
        {
            gameObject.tag = "Player1";
        }
        if (_playerID == PlayerSelector.Player2)
        {
            gameObject.tag = "Player2";
        }
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.transform.gameObject.name.Contains("Base"))
        {
            if (other.transform.gameObject.tag != tag)
            {
                _signalBus.Fire(new HitBaseSignal() { HittedBase =  _playerID , Damage = _damage});
                Destroy(this.gameObject);                
                //TODO пулинг объектов
            }
        }

        if (other.gameObject.name.Contains("Tower"))
        {
            other.gameObject.GetComponent<TowerController>().SetTarget(this.transform, this._playerID);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.name.Contains("Tower"))
        {
            TowerController towerController = other.gameObject.GetComponent<TowerController>();
            towerController.UpdateTarget(Id, towerController.GetTowerID());
        }
    }

    private void OnDestroy()
    {
        _signalBus.Fire(new UnitDestroySignal() { unitId = _id, towerIds = _lastHitTowersId });
    }



}

