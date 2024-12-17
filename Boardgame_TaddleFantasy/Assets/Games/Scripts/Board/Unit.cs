using _Scripts.Tiles;
using UnityEngine;


public class Unit : MonoBehaviour {
    [SerializeField] private SpriteRenderer _renderer;

    public virtual void Init(Sprite sprite, NodeBase node) {
        _renderer.sprite = sprite;
        SetStandingNode(node);
    }
    public virtual void SetStandingNode(NodeBase node)
    {

    }
}
