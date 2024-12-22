using _Scripts.Tiles;
using UnityEngine;


public class Unit : MonoBehaviour {
    [SerializeField] protected SpriteRenderer _renderer;

    public virtual void Init(Sprite sprite) {
        _renderer.sprite = sprite;
    }
    public virtual void SetStandingNode(BaseTileOnBoard node)
    {

    }
}
