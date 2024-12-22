using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    //temp
    [SerializeField] Sprite _sprite;

    public override void Init(Sprite sprite)
    {
        base.Init(sprite);
        _renderer.sprite = _sprite;
    }
    public override void SetStandingNode(BaseTileOnBoard node)
    {
        base.SetStandingNode(node);
        this.transform.position = node.transform.position;
    }
}
