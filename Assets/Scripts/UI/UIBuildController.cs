using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UIBuildController : MonoBehaviour
{
    [Inject]
    private BuildController.Factory _buildFactory;

    [Inject]
    private TowerController.Factory _towerControllerFactory;

    [Inject]
    private GameConfig _gameConfig;

    public void OnCreateTowerBtnClick()
    {
        _towerControllerFactory.Create(3,1, 1, 1, PlayerSelector.Player1);
    }
}
