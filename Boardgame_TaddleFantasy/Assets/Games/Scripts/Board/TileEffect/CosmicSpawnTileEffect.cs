using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CosmicSpawnTileEffect : ITileNodeEffect
{
    public CosmicSpawnTileEffect() : base() { }
    public CosmicSpawnTileEffect(BaseTileOnBoard node) : base(node)
    {
    }
    public override ITaskSchedule CastEffect()
    {
        return new DoCosmicTileNodeEffectTask(this._node, EnemyType.None, 1);
    }
}
public class DoCosmicTileNodeEffectTask : ITaskSchedule
{
    protected BaseTileOnBoard _node;
    protected EnemyType _enemyType;
    protected int _amount;
    public DoCosmicTileNodeEffectTask(BaseTileOnBoard node, EnemyType enemyType, int amount)
    {
        this._node = node;
        this._enemyType = enemyType;
        this._amount = amount;
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        EnemyManager.Instance.SpawnEnemies(this._enemyType, this._amount, new List<BaseTileOnBoard>() { this._node});
        Debug.Log("DoCosmicTileNodeEffectTask --Fly into progress");
    }
}