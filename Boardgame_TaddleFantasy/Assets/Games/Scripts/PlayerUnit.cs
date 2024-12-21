using _Scripts.Tiles;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    [SerializeField] Sprite _sprite;
    [SerializeField] PlayerMovement _myMovement;

    public PlayerMovement MyMovement => _myMovement;
    private void OnEnable()
    {
        _myMovement = GetComponent<PlayerMovement>();
    }
    public override void Init(Sprite sprite, BaseTileOnBoard node)
    {
        base.Init(sprite, node);
        _renderer.sprite = _sprite;
    }
    public override void SetStandingNode(BaseTileOnBoard node)
    {
        base.SetStandingNode(node);
        _myMovement.SetMeToNode(node);
    }

}
