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

    public override void CastEffect()
    {
        if (this._units == null)
            return;

        if (this.LastOccupator != null && LastOccupator is PlayerUnit player)
        {
            Debug.Log($"BoostTileEffect tile, Player reveal, add hime {hpBoostForPlayer} HP");
            return;
        }

        Debug.Log($"BoostTileEffect tile, Enemy reveal, spawn {enemySpawnForEnemy} more");
        return;
    }
}
