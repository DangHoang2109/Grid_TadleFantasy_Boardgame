using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTileEffect : ITileNodeEffect
{
    public WeaponTileEffect() : base() { }
    public WeaponTileEffect(BaseTileOnBoard node) : base(node)
    {
    }

    public override void CastEffect()
    {
        base.CastEffect();

        if (this._units == null)
            return;

        if (this.LastOccupator != null && LastOccupator is PlayerUnit player)
        {
            Debug.Log($"WeaponTileEffect tile, Player reveal, add him a free weapon");
            return;
        }
    }
}
