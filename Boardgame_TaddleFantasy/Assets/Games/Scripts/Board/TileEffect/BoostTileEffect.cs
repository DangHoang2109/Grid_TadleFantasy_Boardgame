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

        return new DoBoostTileNodeEffectTask_SpawnEnemy(enemySpawnForEnemy);
    }
}
public class DoBoostTileNodeEffectTask_HealPlayer : ITaskSchedule
{
    PlayerUnit playerUnit;
    int amountHp;
    public DoBoostTileNodeEffectTask_HealPlayer(PlayerUnit playerUnit, int amountHp)
    {
        this.playerUnit = playerUnit;
        this.amountHp = amountHp;
    }

    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"DoBoostTileNodeEffectTask, add him {amountHp} HP");
    }
}
public class DoBoostTileNodeEffectTask_SpawnEnemy : ITaskSchedule
{
    int _amountSpawn;
    public DoBoostTileNodeEffectTask_SpawnEnemy(int amountSpawn)
    {
        this._amountSpawn = amountSpawn;
    }

    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(2f);
        Debug.Log($"DoBoostTileNodeEffectTask_SpawnEnemy, spawn {_amountSpawn} enemy more");
    }
}