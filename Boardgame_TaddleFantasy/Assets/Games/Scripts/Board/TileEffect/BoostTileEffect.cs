using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoostTileEffect : ITileNodeEffect
{
    public int hpBoostForPlayer = 3;
    public int enemySpawnForEnemy=1;
    public BoostTileEffect() : base() { }
    public BoostTileEffect(BaseTileOnBoard node) : base(node)
    {
    }

    public override ITaskSchedule CastEffect()
    {
        if (this._units == null)
            return null;

        if (this.LastOccupator != null && LastOccupator is PlayerUnit player)
        {
            return new DoBoostTileNodeEffectTask_HealPlayer(player, this.hpBoostForPlayer);
        }

        return new DoBoostTileNodeEffectTask_SpawnEnemy(enemySpawnForEnemy, _node);
    }
}
public class DoBoostTileNodeEffectTask_HealPlayer : ITaskSchedule
{
    PlayerUnit playerUnit;
    PlayerProperty PlayerStat => playerUnit.MyStat as PlayerProperty;
    int amountHp;
    public DoBoostTileNodeEffectTask_HealPlayer(PlayerUnit playerUnit, int amountHp)
    {
        this.playerUnit = playerUnit;
        this.amountHp = amountHp;
    }

    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        PlayerStat.UpdateHP(amountHp);
    }
}
public class DoBoostTileNodeEffectTask_SpawnEnemy : ITaskSchedule
{
    int _amountSpawn;
    BaseTileOnBoard _tile;
    public DoBoostTileNodeEffectTask_SpawnEnemy(int amountSpawn, BaseTileOnBoard tile)
    {
        this._amountSpawn = amountSpawn;
        _tile = tile;
    }

    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        EnemyManager.Instance.SpawnEnemies(EnemyType.None, this._amountSpawn, new List<BaseTileOnBoard>() { _tile });
    }
}