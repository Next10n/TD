using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameInstaller : MonoInstaller
{
    [Inject] private GameConfig _gameConfig;

    public override void InstallBindings()
    {
        SignalBusInstaller.Install(Container);
        Container.DeclareSignal<HitBaseSignal>();
        Container.DeclareSignal<UnitDestroySignal>();
        Container.Bind<AIController>().AsSingle();
        Container.BindFactory<float, int, int, PlayerSelector, UnitController, UnitController.Factory>().
            FromComponentInNewPrefab(_gameConfig.UnitPrefab).WithGameObjectName("Unit");
        Container.BindFactory<float, int, int, float, PlayerSelector, TowerController, TowerController.Factory>().
            FromComponentInNewPrefab(_gameConfig.TowerPrefab).WithGameObjectName("Tower");
        Container.BindFactory<float, int, Transform, Transform, BulletController, BulletController.Factory>().
            FromComponentInNewPrefab(_gameConfig.BulletPrefab).WithGameObjectName("TowerBullet");
        Container.BindFactory<GameObject, BuildController, BuildController.Factory>().FromComponentInNewPrefab(_gameConfig.TowerBuildPrefab);




    }


}
