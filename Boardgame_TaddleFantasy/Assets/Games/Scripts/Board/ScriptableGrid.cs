using _Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ScriptableGrid : ScriptableObject
{
    [SerializeField] protected BaseTileOnBoard nodeBasePrefab;
    [SerializeField, Range(0, 6)] private int _obstacleWeight = 3;
    public abstract int Width();
    public abstract int Height();
    //Dont need to use this now
    public bool DecideIfObstacle() => true; //Random.Range(1, 20) > _obstacleWeight;

    public BaseTileOnBoard NodePrefab => nodeBasePrefab;
}
