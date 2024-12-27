using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponTileEffect : ITileNodeEffect
{
    public WeaponTileEffect() : base() { }
    public WeaponTileEffect(BaseTileOnBoard node) : base(node)
    {
    }

    public override ITaskSchedule CastEffect()
    {
        base.CastEffect();

        if (this._units == null)
            return null;

        if (this.LastOccupator != null && LastOccupator is PlayerUnit player)
        {
            return new DoWeaponTileNodeEffectTask(player, this);
        }
        return null;
    }
}
public class DoWeaponTileNodeEffectTask : ITaskSchedule
{
    PlayerUnit playerUnit;
    WeaponTileEffect weaponTileEffect;
    public DoWeaponTileNodeEffectTask(PlayerUnit playerUnit, WeaponTileEffect weaponTileEffect)
    {
        this.playerUnit = playerUnit;
        this.weaponTileEffect = weaponTileEffect;
    }

    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        yield return new WaitForSeconds(0.5f);
        Debug.Log($"WeaponTileEffect tile, Player reveal, add him a free weapon");
    }
}