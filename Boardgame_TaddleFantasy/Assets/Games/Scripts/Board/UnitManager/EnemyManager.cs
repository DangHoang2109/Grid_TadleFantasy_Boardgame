using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Taddle_Fantasy;
using UnityEngine;
using UnityEngine.Pool;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance => UnitManager.Instance.enemyManager;

    [SerializeField] private Transform _tfUnit;

    [Header("Unit")]
    [SerializeField] private Unit _unitPrefab;
    [SerializeField] private List<EnemyScriptable> _enemyConfigs;
    [SerializeField] List<EnemyUnit> enemies = new List<EnemyUnit>();
    Dictionary<EnemyType, ObjectPool<EnemyUnit>> dictPoolByType = new Dictionary<EnemyType, ObjectPool<EnemyUnit>>();

    public List<EnemyUnit> ActiveEnemies => enemies;
    public EnemyScriptable GetEnemyConfig(EnemyType type) => _enemyConfigs.Find(e => e.enemyType == type);
    public void Init(Transform tfUnit)
    {
        this._tfUnit = tfUnit;
        PreparePool();
    }
    void PreparePool()
    {
        var allType = Enum.GetValues(typeof(EnemyType)).Cast<EnemyType>().ToList();
        foreach (var type in allType)
        {
            dictPoolByType.TryAdd(type, new ObjectPool<EnemyUnit>(
                createFunc: OnCreate,
                actionOnGet: OnGet,
                actionOnRelease: OnRelease,
                defaultCapacity: 3,
                maxSize: 100
                ));
        }
    }

    public void StartGame()
    {
        //Spawn enemy into all the revealed gate node, each node one enemy
        EnemyType startingEnemy = EnemyType.None;
        var tiles = GridManager.Instance.GetTilesOfType(TileEffectType.Gate);
        SpawnEnemies(startingEnemy, tiles);
    }

    public void SpawnAnEnemy(EnemyType type, BaseTileOnBoard tile)
    {
        if (dictPoolByType.TryGetValue(type, out ObjectPool<EnemyUnit> ePool))
        {
            EnemyUnit enemyUnit = ePool.Get();

            var eConfig = GetEnemyConfig(type);
            enemyUnit.Init(eConfig);
            enemyUnit.SetStandingNodeWithNoise(tile);
        }
    }
    public void SpawnEnemies(EnemyType type, List<BaseTileOnBoard> tiles)
    {
        if (dictPoolByType.TryGetValue(type, out ObjectPool<EnemyUnit> ePool))
        {
            var eConfig = GetEnemyConfig(type);
            foreach (var tile in tiles)
            {
                EnemyUnit enemyUnit = ePool.Get();
                enemyUnit.Init(eConfig);
                enemyUnit.SetStandingNodeWithNoise(tile);
            }
        }
    }
    public void SpawnEnemies(EnemyType type, int amountEachNode, List<BaseTileOnBoard> tiles)
    {
        if (dictPoolByType.TryGetValue(type, out ObjectPool<EnemyUnit> ePool))
        {
            var eConfig = GetEnemyConfig(type);
            foreach (var tile in tiles)
            {
                for (int i = 0; i < amountEachNode; i++)
                {
                    EnemyUnit enemyUnit = ePool.Get();
                    enemyUnit.Init(eConfig);
                    enemyUnit.SetStandingNodeWithNoise(tile);
                }
            }
        }
    }
    public void KillEnemy(EnemyUnit e)
    {
        if (dictPoolByType.TryGetValue(e.EnemyType, out ObjectPool<EnemyUnit> ePool))
        {
            ePool.Release(e);
        }
    }
    public void KillEnemies(List<EnemyUnit> es)
    {
        foreach (var e in es)
        {
            KillEnemy(e);
        }
    }
    public (List<BaseTileOnBoard>, EnemyType, int) GenerateInviteResult()
    {
        ///ID position of the Gate to spawn, -1 mean spawn all the gate tile
        List<int> craftGateDice = new List<int>(GridManager.Instance.GetTilesOfType(TileEffectType.Gate).Select(t => t.GridId));
        craftGateDice.Add(EnemyInviteConstant.ALL_GATE_DEFINE);

        //Amount creep enemy to spawn, -1 mean spawn Elite creep
        List<int> craftEnemyDice = new List<int>() { EnemyInviteConstant.ELITE_ENEMY_DEFINE, 1, 1, 1, 2, 2 };

        var _gateID = craftGateDice.Random();
        int _enemyAmount = craftEnemyDice.Random();

        List<BaseTileOnBoard> nodesToSpawn = new List<BaseTileOnBoard>();
        if (_gateID == EnemyInviteConstant.ALL_GATE_DEFINE)
            nodesToSpawn = GridManager.Instance.GetTilesOfType(TileEffectType.Gate);
        else
        {
            if (GridManager.Instance.TryGetTileById(_gateID, out BaseTileOnBoard t))
                nodesToSpawn.Add(t);
        }
        EnemyType typeToSpawn = _enemyAmount == EnemyInviteConstant.ELITE_ENEMY_DEFINE ? EnemyType.Elite : EnemyType.None;

        return new(nodesToSpawn, typeToSpawn, Mathf.Clamp(_enemyAmount, 1, int.MaxValue));
    }
    #region Pool
    EnemyUnit OnCreate()
    {
        // Create a new instance of the base enemy prefab
        var newItem = Instantiate(this._unitPrefab, this._tfUnit);
        newItem.gameObject.SetActive(false); // Initially inactive
        return newItem as EnemyUnit;
    }
    void OnGet(EnemyUnit newItem)
    {
        newItem.gameObject.SetActive(true); 
        this.enemies.Add(newItem);
    }
    void OnRelease(EnemyUnit newItem)
    {
        newItem.gameObject.SetActive(false); // Initially inactive
        this.enemies.Remove(newItem);
    }
    #endregion

}
public enum EnemyType
{
    //Normal = Creep enemy
    None,

    Elite,

    Boss,
}
public static class EnemyInviteConstant
{
    public const int ALL_GATE_DEFINE = -1;
    public const int ELITE_ENEMY_DEFINE = -1;
}