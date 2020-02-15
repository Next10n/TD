using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameController : MonoBehaviour
{
    [Inject]
    private GameConfig _gameConfig;
    [Inject]
    private UnitController.Factory _unitsFactory;
    [Inject]
    private SignalBus _signalBus;
    [Inject]
    private TowerController.Factory _towerFactory;

    private GameObject _player1Units;
    private GameObject _player2Units;

    private int unitId = 0; // TODO добавить менеджер айдишников

    void Start()
    {
        _player1Units = new GameObject("Player1Units");
        _player2Units = new GameObject("Player2Units");
        InvokeRepeating(nameof(Spawn1v1), 1, 5);
        _unitsFactory.Create(unitId, _gameConfig.UnitSpeed, _gameConfig.UnitHealth, _gameConfig.UnitDamageOnBase, PlayerSelector.Player1).transform.SetParent(_player1Units.transform);
        unitId++;
        _unitsFactory.Create(unitId, _gameConfig.UnitSpeed, _gameConfig.UnitHealth, _gameConfig.UnitDamageOnBase, PlayerSelector.Player2).transform.SetParent(_player2Units.transform);
        unitId++;
        StartCoroutine(croutine());
    }



    public void Play()
    {
        
    }

    void Update()
    {

    }

    IEnumerator croutine()
    {
        while (true)
        {
            InvokeRepeating(nameof(Spawn1v1), 1, 5);
            yield return new WaitForSeconds(10f);
        }
    }


    private void Spawn1v1()
    {
        _unitsFactory.Create(unitId, _gameConfig.UnitSpeed, _gameConfig.UnitHealth, _gameConfig.UnitDamageOnBase, PlayerSelector.Player1).transform.SetParent(_player1Units.transform);
        unitId++;
        _unitsFactory.Create(unitId, _gameConfig.UnitSpeed, _gameConfig.UnitHealth, _gameConfig.UnitDamageOnBase, PlayerSelector.Player2).transform.SetParent(_player2Units.transform);
        unitId++;

    }

    private void OnApplicationQuit()
    {
        //_signalBus.TryUnsubscribe<HitBaseSignal>(prin123t);
    }


}
