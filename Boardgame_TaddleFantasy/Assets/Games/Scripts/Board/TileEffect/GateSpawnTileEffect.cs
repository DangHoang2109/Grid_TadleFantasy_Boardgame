using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GateSpawnTileEffect : ITileNodeEffect
{
    public int indexGate;
    public GateSpawnTileEffect() : base() { }
    public GateSpawnTileEffect(BaseTileOnBoard node) : base(node)
    {
    }
    public void Init_SetIndexGate(int index)
    {
        indexGate = index;
    }
    public override ITaskSchedule CastEffect()
    {
        if (this.indexGate < 0)
            return null;
        return new DoGateTileNodeEffectTask();
    }
    public override void Flip()
    {
        ShowVFX();
    }

}
public class DoGateTileNodeEffectTask : ITaskSchedule
{
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("DoGateTileNodeEffectTask");
    }
}