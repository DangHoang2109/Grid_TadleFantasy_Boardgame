using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class ITileNodeEffect
{
    protected BaseTileOnBoard _node;
    protected List<Unit> _units;

    protected Unit LastOccupator => _units.LastOrDefault();

    public ITileNodeEffect()
    {
    }
    public ITileNodeEffect(BaseTileOnBoard node)
    {
        SetHost(node);
    }
    public virtual void SetHost(BaseTileOnBoard node)
    {
        _node = node;
    }
    public virtual void SetOccupation(Unit unit)
    {
        _units ??= new List<Unit>();
        _units.Add(unit);
    }
    public virtual void UnOccupation(Unit unit)
    {
        _units ??= new List<Unit>();
        _units.Remove(unit);
    }
    public virtual void UnOccupateAllUnits()
    {
        _units ??= new List<Unit>();
        _units.Clear();
    }
    public virtual void Flip() { ShowVFX(); CastEffect(); }
    public virtual void CastEffect() { }
    public virtual void ShowVFX() { }
}
public enum TileEffectType
{
    [Type(typeof(NormalTileEffect))]
    None,
    //Player +3HP, Enemy +1
    [Type(typeof(BoostTileEffect))]
    Boosting,
    //Grant player 1 weapon free
    [Type(typeof(WeaponTileEffect))]
    Weapon,
    //Spawn enemy every turn
    [Type(typeof(CosmicSpawnTileEffect))]
    Cosmic,
    //Spawn enemy in their standy
    [Type(typeof(GateSpawnTileEffect))]
    Gate,
}