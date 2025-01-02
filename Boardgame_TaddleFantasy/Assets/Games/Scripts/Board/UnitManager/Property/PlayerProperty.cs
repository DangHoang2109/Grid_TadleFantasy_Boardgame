using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : UnitProperty
{
    public override int AttackDice { get => base.AttackDice; protected set { base.AttackDice = value; onAttackDicesChange?.Invoke(value); } }
    public virtual int CurrentAP { get; protected set; }


    public static System.Action<int> onAttackDicesChange;
    //Change, Current value
    public static System.Action<int,int> onAPChange;

    public override void InitStat(UnitScriptable config)
    {
        base.InitStat(config);
        PlayerScriptable playerScriptable = config as PlayerScriptable;
        CurrentHP = playerScriptable.StartingHP();
        CurrentAP = 0;
    }
    public virtual void UpdateAP(int change)
    {
        CurrentAP = Mathf.Clamp(CurrentAP + change, 0, int.MaxValue);
        onAPChange?.Invoke(change,CurrentAP);
    }

}
