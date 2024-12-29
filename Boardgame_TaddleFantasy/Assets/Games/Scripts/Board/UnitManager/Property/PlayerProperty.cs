using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProperty : UnitProperty
{
    public override int AttackDice { get => base.AttackDice; protected set { base.AttackDice = value; onAttackDicesChange?.Invoke(value); } }
    public static System.Action<int> onAttackDicesChange;
    public override void InitStat(UnitScriptable config)
    {
        base.InitStat(config);
        PlayerScriptable playerScriptable = config as PlayerScriptable;
        CurrentHP = playerScriptable.StartingHP();
    }
}
