using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contain the property of unit
/// </summary>

public class UnitProperty : MonoBehaviour
{
    public virtual int MaxHP { get; protected set; }
    public virtual int CurrentHP { get; set; }
    public virtual int AttackDice { get; protected set; }
    public virtual int AttackRange { get; protected set; }
    public virtual int AttackDamage { get; protected set; }

    public virtual void InitStat(UnitScriptable config)
    {
        CurrentHP = MaxHP = config.MaxHP();
        AttackDice = config.AttackDice();
        AttackRange = config.AttackRange();
        AttackDamage = config.AttackDamage();
    }
}
