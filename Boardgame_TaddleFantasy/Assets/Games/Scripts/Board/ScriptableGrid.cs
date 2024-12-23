using _Scripts.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public abstract class ScriptableGrid : ScriptableObject
{
    [SerializeField] protected BaseTileOnBoard nodeBasePrefab;
    [SerializeField, Range(0, 6)] protected int _obstacleWeight = 3;

    [SerializeField] protected List<GridNodesEffectConfig> nodeEffects;
    public abstract int Width();
    public abstract int Height();
    public abstract Vector3 CellPosition(int rowID, int colID);
    //Dont need to use this now
    public bool DecideIfObstacle() => true; //Random.Range(1, 20) > _obstacleWeight;

    public BaseTileOnBoard NodePrefab => nodeBasePrefab;


    public virtual Dictionary<Vector2Int, TileData> GenerateGrid()
    {
        //All posible tile effect in the grid
        List<TileEffectType> tileEffectTypes = GenerateListTileEffect();
        int gridSize = Height() * Width();
        if (tileEffectTypes.Count == 0)
        {
            Debug.LogAssertion("There is no tile effects configed, I will make all of them normal");
            for (int i = 0; i < gridSize; i++)
            {
                tileEffectTypes.Add(TileEffectType.None);
            }
        }
        if (tileEffectTypes.Count < gridSize) 
        {
            Debug.LogAssertion("The config not match, I will make normal tile for missing tile");
            for (int i = tileEffectTypes.Count; i < gridSize; i++)
            {
                tileEffectTypes.Add(TileEffectType.None);
            }
        }
        if (tileEffectTypes.Count > gridSize)
        {
            Debug.LogAssertion("The config not match, Some last node effect config will be discard");
        }


        var grid = new Dictionary<Vector2Int,TileData>();

        //form a grid with all normal tile -> assert the grid is follow Width x Height format first
        for (int i = 0; i < this.Height(); i++)
        {
            for (int j = 0; j < this.Width(); j++)
            {
                var t = new TileData(j, i, effectType: TileEffectType.None,walkable: DecideIfObstacle(), position: CellPosition(j, i));
                grid.TryAdd(new Vector2Int(j,i),t);
            }
        }

        //fill node in persistent position 
        foreach (var c in nodeEffects)
        {
            foreach (var pos in c.PersistentPositions)
            {
                if (tileEffectTypes.Contains(c.Type) && grid.TryGetValue(pos, out TileData t))
                {
                    t.SetTileEffect(c.Type);
                    tileEffectTypes.Remove(c.Type);
                }
            }
        }
        //random from list and fill other random nodes
        List<TileData> availableForRandom = grid.Values.ToList().FindAll(t => t.TileEffectType == TileEffectType.None);
        var randomizeNOde = availableForRandom.OrderBy(x => UnityEngine.Random.value).ToList();
        int index = 0;
        foreach (var c in nodeEffects)
        {
            int amountRandom = c.AmountRandomNodes();
            for (int i = 0; i < amountRandom; i++)
            {
                randomizeNOde[index++].SetTileEffect(c.Type);
                if(index >=  randomizeNOde.Count)
                    break;
            }

            if (index >= randomizeNOde.Count)
                break;
        }

        return grid;
    }
    public virtual List<TileEffectType> GenerateListTileEffect()
    {
        List<TileEffectType> tileEffectTypes = new List<TileEffectType>();
        foreach (var c in this.nodeEffects)
        {
            for (int i = 0; i < c.Amount; i++)
            {
                tileEffectTypes.Add(c.Type);
            }
        }
        return tileEffectTypes;
    }
    public Color GetNodeColor(TileEffectType type) => nodeEffects.Find(c => c.Type == type).temp_tileColor;
}
[Serializable]
public struct GridNodesEffectConfig
{
    public TileEffectType Type;
    public int Amount;
    /// <summary>
    /// Các vị trí bắt buộc trên grid
    /// (_row, _col)
    /// </summary>
    public List<Vector2Int> PersistentPositions;
    public Color temp_tileColor;
    public int AmountRandomNodes() => UnityEngine.Mathf.Clamp(Amount - PersistentPositions.Count, 0, int.MaxValue); 
}