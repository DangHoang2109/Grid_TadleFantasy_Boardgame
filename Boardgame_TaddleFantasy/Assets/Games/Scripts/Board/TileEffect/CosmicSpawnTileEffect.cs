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
        return new DoCosmicTileNodeEffectTask();
    }
}
public class DoCosmicTileNodeEffectTask : ITaskSchedule
{
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame(); 
        
        yield return new WaitForSeconds(1f);

        Debug.Log("DoCosmicTileNodeEffectTask");
    }
}