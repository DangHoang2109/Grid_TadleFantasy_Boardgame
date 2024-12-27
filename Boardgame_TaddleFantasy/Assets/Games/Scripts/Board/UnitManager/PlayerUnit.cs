using _Scripts.Tiles;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] Sprite _sprite;

    public PlayerMovement MyMovement => _myMovement as PlayerMovement;

    public override void Init(Sprite sprite)
    {
        base.Init(sprite);
        _renderer.sprite = _sprite;
    }
    public override void SetStandingNode(BaseTileOnBoard node)
    {
        base.SetStandingNode(node);
        this.transform.position = node.transform.position;
        _myMovement.SetMeToNode(node);
    }
    void Init_PlayerStat(EnemyScriptable enemyScriptable)
    {
        this.MyMovement.SetMovementAllow(enemyScriptable.MoveRange());
    }
}
