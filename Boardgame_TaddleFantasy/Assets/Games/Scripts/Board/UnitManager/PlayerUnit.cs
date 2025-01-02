using _Scripts.Tiles;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] PlayerType PlayerType;
    private PlayerScriptable characterConfig;

    public PlayerScriptable MyCharacterConfig => characterConfig;
    public PlayerMovement MyMovement => _myMovement as PlayerMovement;
    public PlayerProperty MyProperty => _myStat as PlayerProperty;
    
    public override void Init(Sprite sprite)
    {
        base.Init(sprite);
    }
    public void Init(PlayerScriptable playerScriptable)
    {
        characterConfig = playerScriptable;
        this.PlayerType = playerScriptable.playerType;
        //set visual
        GameObject visualObject = playerScriptable.visual;
        if (visualObject != null)
        {
            Instantiate(visualObject, this.transform);
        }
        else
        {
        }
        //set stat
        Init_PlayerStat(playerScriptable);
    }
    public override void SetStandingNode(BaseTileOnBoard node)
    {
        base.SetStandingNode(node);
        this.transform.position = node.transform.position;
        _myMovement.SetMeToNode(node);
    }
    void Init_PlayerStat(PlayerScriptable playerScriptable)
    {
        this.MyProperty.InitStat(playerScriptable);
        this.MyMovement.SetMovementAllow(playerScriptable.MoveRange());
    }
}
