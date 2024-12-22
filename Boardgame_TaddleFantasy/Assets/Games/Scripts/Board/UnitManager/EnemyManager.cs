using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Taddle_Fantasy;
using UnityEngine;
using UnityEngine.Pool;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EnemyManager : MonoBehaviour
{
    public static EnemyManager Instance => UnitManager.Instance.enemyManager;

    [SerializeField] private Unit _unitPrefab;
    [SerializeField] List<EnemyUnit> enemies = new List<EnemyUnit>();
    Dictionary<EnemyType, ObjectPool<EnemyUnit>> dictPoolByType = new Dictionary<EnemyType, ObjectPool<EnemyUnit>>();
    public void Init()
    {
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
        if(dictPoolByType.TryGetValue(startingEnemy, out ObjectPool<EnemyUnit> ePool))
        {
            foreach (var tile in tiles)
            {
                EnemyUnit enemyUnit = ePool.Get();
                enemyUnit.SetStandingNode(tile);
            }
        }
    }

    #region Pool
    EnemyUnit OnCreate()
    {
        // Create a new instance of the base enemy prefab
        var newItem = Instantiate(this._unitPrefab);
        newItem.gameObject.SetActive(false); // Initially inactive
        return newItem as EnemyUnit;
    }
    void OnGet(EnemyUnit newItem)
    {
        newItem.gameObject.SetActive(true); 
        this.enemies.Add(newItem);
        newItem.Init(null);
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