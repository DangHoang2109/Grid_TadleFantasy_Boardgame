using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScriptable : UnitScriptable
{
    [SerializeField] public PlayerType playerType;
    [SerializeField] protected int startingHP;
    public virtual int StartingHP() => startingHP;

}
public enum PlayerType
{
    //No Skill? May be just for tutorial
    None,

    Balance
}