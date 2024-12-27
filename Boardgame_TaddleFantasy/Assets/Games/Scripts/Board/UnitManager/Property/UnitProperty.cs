using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contain the property of unit
/// </summary>

public class UnitProperty : MonoBehaviour
{
    [SerializeField] protected int _currentHP;
    [SerializeField] protected int _maxHP;

    [SerializeField] protected int _attackDice;
    [SerializeField] protected int _attackRange;

    public virtual void InitStat(UnitScriptable config)
    {
        _currentHP = _maxHP = config.MaxHP();
        _attackDice = config.AttackDice();
        _attackRange = config.AttackRange();
    }
}
