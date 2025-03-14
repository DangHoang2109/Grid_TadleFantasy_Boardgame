using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UnitScriptable : ScriptableObject
{
    [SerializeField] protected string unitName;
    [SerializeField] public GameObject visual;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int attackDmg;

    [SerializeField] protected int killedReward;
    [SerializeField] protected int attackRange;
    [SerializeField] protected int moveRange;

    public virtual string Name() => unitName;
    public virtual int MaxHP() => maxHp;
    public virtual int AttackDamage() => attackDmg;

    public virtual int KilledReward() => killedReward;
    public virtual int AttackRange() => attackRange;
    public virtual int MoveRange() => moveRange;
    public abstract int PowerPoint();
}
