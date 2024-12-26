using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    //temp
    [SerializeField] Sprite _sprite;
    [SerializeField] Vector3 noisePositionInNode = new Vector3(0.5f, 0.5f);
    public override void Init(Sprite sprite)
    {
        base.Init(sprite);
        _renderer.sprite = _sprite;
    }
    public virtual void Init(GameObject visualObject)
    {
        base.Init(null);
        if(visualObject != null)
        {
            Instantiate(visualObject, this.transform);
        }
        else
        {
            _renderer.sprite = _sprite;
        }
    }
    public override void SetStandingNode(BaseTileOnBoard node)
    {
        base.SetStandingNode(node);
        this.transform.position = node.transform.position + new Vector3(
            x: Random.Range(-noisePositionInNode.x, noisePositionInNode.x),
            y: Random.Range(-noisePositionInNode.y, noisePositionInNode.y)
            );
    }
}
