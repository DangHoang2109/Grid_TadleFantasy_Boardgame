using _Scripts.Tiles;
using UnityEngine;


public class Unit : MonoBehaviour {
    [SerializeField] protected SpriteRenderer _renderer;

    public virtual void Init(Sprite sprite, BaseTileOnBoard node) {
        _renderer.sprite = sprite;
        SetStandingNode(node);
    }
    public virtual void SetStandingNode(BaseTileOnBoard node)
    {

    }
}
