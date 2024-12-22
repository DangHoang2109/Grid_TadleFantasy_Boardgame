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
    public virtual Tween MoveToNode(BaseTileOnBoard nodeDestination, System.Action onComplete = null)
    {
        if (nodeDestination is SquareTileOnBoardNode tileNode)
            tileNode.Flip();

        return this.transform.DOJump(nodeDestination.transform.position, 0.5f, 1, 0.2f).SetEase(Ease.InQuad).OnComplete(()=> { onComplete?.Invoke(); });

    }
}
