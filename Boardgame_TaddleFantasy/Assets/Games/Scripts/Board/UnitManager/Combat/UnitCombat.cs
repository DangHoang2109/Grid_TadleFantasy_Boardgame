using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class UnitCombat : MonoBehaviour
{
    [SerializeField] protected Unit _unit;
    [SerializeField] protected UnitProperty _unitProperty => _unit?.MyStat;

    public int AttackDamage => _unitProperty.AttackDamage;
    /// <summary>
    /// int change, int currentHP, int maxHP
    /// </summary>
    public System.Action<int, int, int> OnHPChange;

    public virtual int GenerateRollingAttackDiceResult()
    {
        return CombatManager.GenerateRollingAttackDiceResult(this._unitProperty.AttackDice);
    }

    private void OnEnable()
    {
        _unit ??= GetComponent<Unit>();
    }


    public virtual void TakeDamage(int damage)
    {
        _unitProperty.UpdateHP(-damage);
        AnimationTakingDamager(damage);
        OnHPChange?.Invoke(-damage, _unitProperty.CurrentHP, _unitProperty.MaxHP);
    }
    public virtual void AnimationTakingDamager(int damage)
    {
        this.transform.DOPunchPosition(new Vector3(0, 0.25f), 0.2f).SetDelay(0.25f).OnComplete(()=> OnCompleteAnimationTakingDamage(damage));
    }
    public virtual void OnCompleteAnimationTakingDamage(int damage)
    {
        if (_unitProperty.CurrentHP <= 0)
            Die();
    }
    public virtual void Die()
    {

    }
    public virtual void Attack(Unit target, int damageMultiplier)
    {
        if(target == null) return;
        target.MyCombat?.TakeDamage(_unitProperty.AttackDamage * damageMultiplier);
        AnimationAttack(target);
    }
    public virtual void Attacks(List<Unit> targets, int damageMultiplier)
    {
        if (targets == null || targets.Count == 0) return;

        Sequence seq = DOTween.Sequence().SetId(this);
        foreach (var t in targets)
        {
            t.MyCombat?.TakeDamage(_unitProperty.AttackDamage * damageMultiplier);
            seq.Append(AnimationAttack(t));
        }
    }
    public virtual Tween AnimationAttack(Unit target)
    {
        return this.transform.DOMove(target.transform.position, 0.2f).SetEase(Ease.InBack).SetLoops(2, LoopType.Yoyo);
    }
}
