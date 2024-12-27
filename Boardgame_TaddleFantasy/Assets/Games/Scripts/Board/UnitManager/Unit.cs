using _Scripts.Tiles;
using UnityEngine;


public class Unit : MonoBehaviour {

    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected UnitMovement _myMovement;

    private void OnEnable()
    {
        _myMovement = GetComponent<UnitMovement>();
    }
    public virtual void Init(Sprite sprite) {
        _renderer.sprite = sprite;
    }
    public virtual void SetStandingNode(BaseTileOnBoard node)
    {

    }

    public virtual void Move()
    {
        this._myMovement?.Move();
    }

    public virtual void Disable_RemoveFromBoard()
    {
        Debug.Log("Disable_Remove from Board");
    }
}
