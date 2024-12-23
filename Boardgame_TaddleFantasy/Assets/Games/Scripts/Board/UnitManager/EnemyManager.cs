using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Taddle_Fantasy;
using UnityEditorInternal.Profiling.Memory.Experimental;
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
                defaultCapacity: 3
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
            enemyUnit.Init(eConfig.enemyVisual);
            enemyUnit.SetStandingNode(tile);
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
                enemyUnit.Init(eConfig.enemyVisual);
                enemyUnit.SetStandingNode(tile);
            }
        }
    }

    public void GenerateInviteResult()
    {

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