using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "GameConfig", menuName = "Create GameConfig")]
public class GameConfig : ScriptableObject 
{
    public float UnitSpeed;
    public int UnitHealth;
    public int UnitDamageOnBase;
    public float BaseStartHealth;
    public float PlayersStartGold;
    public float WinScore;
    public Material Player1Color;
    public Material Player2Color;
    public GameObject UnitPrefab;
    public GameObject TowerPrefab;
    public GameObject BulletPrefab;
    public GameObject TowerBuildPrefab;
}
