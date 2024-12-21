using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTileEffect : ITileNodeEffect
{
    public NormalTileEffect() : base() { }
    public NormalTileEffect(BaseTileOnBoard node) : base(node)
    {
    }

    public override ITaskSchedule CastEffect()
    {
        Debug.Log("Normal tile, no effect");
        return new DoNormalTileNodeEffectTask();
    }

    public override void Flip()
    {
        CastEffect();
    }
}
public class DoNormalTileNodeEffectTask : ITaskSchedule
{
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log("DoNormalTileNodeEffectTask");
    }
}
