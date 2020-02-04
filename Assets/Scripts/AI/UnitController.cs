using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

public class UnitController : MonoBehaviour, ISetColor
{
    
    [Inject]
    private GameConfig _gameConfig;
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

    private Transform _target;
    private NavMeshAgent agent;

    private void Awake()
    {
        OnCreate();
    }


    public void SetTarget()
    {
        if(_playerID == PlayerSelector.Player1)
        {
            _target = GameObject.Find("Base2").transform;
        }
        if(_playerID == PlayerSelector.Player2)
        {
            _target = GameObject.Find("Base1").transform;
        }
    }

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
                //_signalBus.Fire<HitBaseSignal>();
                _signalBus.Fire(new HitBaseSignal() { HittedBase =  _playerID , Damage = _damage});
                Destroy(this.gameObject);
                
                //fire destroy
                //пулинг объектов
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
            other.gameObject.GetComponent<TowerController>().ResetTarget(this.transform, this._playerID);
        }
    }

    public void TakeDamage(int damage)
    {
        _health -= damage;
        if(_health <= 0)
        {
            _signalBus.Fire<UnitDestroySignal>();
            //GetComponent<MeshRenderer>().enabled = false;
            Destroy(this.gameObject); //это костыль пока что

            //далее пулинг

        }
    }


    public class Factory : PlaceholderFactory<float, int, int, PlayerSelector, UnitController>
    {

    }
}

