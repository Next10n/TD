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

    private int unitId = 0; // временная хуцйня

    void Start()
    {        
        InvokeRepeating(nameof(Spawn1v1), 1, 5);
        _unitsFactory.Create(unitId, _gameConfig.UnitSpeed, _gameConfig.UnitHealth, _gameConfig.UnitDamageOnBase, PlayerSelector.Player1);
        unitId++;
        _unitsFactory.Create(unitId, _gameConfig.UnitSpeed, _gameConfig.UnitHealth, _gameConfig.UnitDamageOnBase, PlayerSelector.Player1);
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
        _unitsFactory.Create(unitId, _gameConfig.UnitSpeed, _gameConfig.UnitHealth, _gameConfig.UnitDamageOnBase, PlayerSelector.Player1);
        unitId++;
        _unitsFactory.Create(unitId, _gameConfig.UnitSpeed, _gameConfig.UnitHealth, _gameConfig.UnitDamageOnBase, PlayerSelector.Player2);
        unitId++;

    }

    private void OnApplicationQuit()
    {
        //_signalBus.TryUnsubscribe<HitBaseSignal>(prin123t);
    }


}
