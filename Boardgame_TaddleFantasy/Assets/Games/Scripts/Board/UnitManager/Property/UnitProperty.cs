using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Contain the property of unit
/// </summary>

public class UnitProperty : MonoBehaviour
{
    [SerializeField] protected Unit _unit;

    public virtual int MaxHP { get; protected set; }
    public virtual int CurrentHP { get; set; }
    public virtual int AttackDice { get; protected set; }
    public virtual int AttackRange { get; protected set; }
    public virtual int AttackDamage { get; protected set; }

    public virtual string HPString => $"{CurrentHP}/{MaxHP}";

    //Change -- current -- max
    public System.Action<int, int, int> onHPChange;
    protected virtual void OnEnable()
    {
        _unit ??= GetComponent<Unit>();
    }
    public virtual void InitStat(UnitScriptable config)
    {
        CurrentHP = MaxHP = config.MaxHP();
        AttackDice = config.AttackDice();
        AttackRange = config.AttackRange();
        AttackDamage = config.AttackDamage();
    }
    public virtual void UpdateHP(int change)
    {
        CurrentHP = Mathf.Clamp(CurrentHP + change, 0, MaxHP);
        onHPChange?.Invoke(change, CurrentHP, MaxHP);
        SpawnFloatBuble(change);

        if (IsDie()) { Die(); }
    } 
    protected virtual void SpawnFloatBuble(int change)
    {
        FloatBubbleManager.Instance.SpawnBubble(text: change.ToString(), color: change < 0 ? Color.red : Color.green,
            position: InGameCamera.GameCamera.WorldToScreenPoint(this.transform.position));
    }

    public virtual bool IsDie() => CurrentHP <= 0;
    public virtual void Die()
    {

    }
}
