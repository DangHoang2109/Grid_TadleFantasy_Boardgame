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
    public override void CastEffect()
    {
        if (this.indexGate < 0)
            return;
    }
    public override void Flip()
    {
        ShowVFX();
    }

}
