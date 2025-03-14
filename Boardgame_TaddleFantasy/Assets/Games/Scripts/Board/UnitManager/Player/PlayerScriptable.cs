using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptable : UnitScriptable
{
    [SerializeField] public PlayerType playerType;
    [SerializeField] protected int startingHP;
    [SerializeField] protected int attackDice;
    public virtual int StartingHP() => startingHP;
    public virtual int AttackDice() => attackDice;

    public override int PowerPoint()
    {
        return AttackDice();
    }
}
public enum PlayerType
{
    //No Skill? May be just for tutorial
    None,

    Balance
}