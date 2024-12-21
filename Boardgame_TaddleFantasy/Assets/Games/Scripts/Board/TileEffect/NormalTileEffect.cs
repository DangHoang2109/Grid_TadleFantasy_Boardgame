using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NormalTileEffect : ITileNodeEffect
{
    public NormalTileEffect() : base() { }
    public NormalTileEffect(BaseTileOnBoard node) : base(node)
    {
    }

    public override void CastEffect()
    {
        Debug.Log("Normal tile, no effect");
    }

    public override void Flip()
    {
        CastEffect();
    }
}
