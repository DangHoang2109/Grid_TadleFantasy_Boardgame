using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Taddle_Fantasy;
using UnityEngine;
using static UnityEditor.Progress;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance;
    void Awake() => Instance = this;

    [Header("Unit Player")]
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] List<PlayerUnit> players = new List<PlayerUnit>();

    [Header("Enemy")]
    public EnemyManager enemyManager;

    public PlayerUnit GetPlayerByTurnIndex(int index) => players[index];
    public PlayerUnit MainPlayer => GetPlayerByTurnIndex(0);

    public void Init()
    {
        SpawnUnits();
        enemyManager.Init();
    }
    void SpawnUnits()
    {
        var _spawnedPlayer = Instantiate(_unitPrefab); //_playerNodeBase.transform.position, Quaternion.identity
        players.Add(_spawnedPlayer as PlayerUnit);

        _spawnedPlayer.Init(null);
    }
    public void StartGame_PlayerPickNode(PlayerUnit p, BaseTileOnBoard node) 
    {
        p.SetStandingNode(node);
    }
    public void StartGame_MainPlayerPickNode(BaseTileOnBoard node)
    {
        MainPlayer.SetStandingNode(node);
    }
}
