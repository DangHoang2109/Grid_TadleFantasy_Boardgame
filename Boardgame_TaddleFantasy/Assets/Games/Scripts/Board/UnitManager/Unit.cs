using _Scripts.Tiles;
using UnityEngine;


public class Unit : MonoBehaviour {

    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected UnitMovement _myMovement;
    [SerializeField] protected UnitProperty _myStat;
    [SerializeField] protected UnitCombat _myCombat;

    public UnitProperty MyStat => _myStat;
    public UnitCombat MyCombat => _myCombat;
    public virtual BaseTileOnBoard StandingNode => _myMovement.StandingNode;
    private void OnEnable()
    {
        _myMovement ??= GetComponent<UnitMovement>();
        _myStat ??= GetComponent<UnitProperty>();
        _myCombat ??= GetComponent<UnitCombat>();
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
