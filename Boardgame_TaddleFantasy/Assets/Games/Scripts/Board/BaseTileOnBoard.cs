using _Scripts.Tiles;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaseTileOnBoard : MonoBehaviour
{
    public static readonly List<Vector2Int> Dirs = new List<Vector2Int>() {
            new Vector2Int(0, 1), new Vector2Int(-1, 0), new Vector2Int(0, -1), new Vector2Int(1, 0),
            new Vector2Int(1, 1), new Vector2Int(1, -1), new Vector2Int(-1, -1), new Vector2Int(-1, 1)
        };

    private bool _selected;
    protected Color _defaultColor;

    [Header("References")]
    [SerializeField]
    protected Color _obstacleColor, _backsideColor;

    [SerializeField] protected Gradient _walkableColor;
    [SerializeField] protected SpriteRenderer _renderer;
    [SerializeField] protected SpriteRenderer _effectRenderer;

    [SerializeField] private TileData _tileData;
    public TileData Data => _tileData;
    public float GetDistance(BaseTileOnBoard other) => Data.GetDistance(other.Data); // Helper to reduce noise in pathfinding
    public List<BaseTileOnBoard> Neighbors { get; protected set; }

    public bool Walkable { get => Data.Walkable;}
    public int GridId => Data?.Id ?? 0;
    public int Row
    {
        get
        {
            if (this.Data == null)
            {
                return -1;
            }

            return this.Data.row;
        }
    }
    public int Col
    {
        get
        {
            if (this.Data == null)
            {
                return -1;
            }

            return this.Data.col;
        }
    }
    public bool IsFaceUp { get; protected set; }

    public TileEffectType EffectType => this.Data.TileEffectType;
    public ITileNodeEffect TileEffect => this.Data.TileNodeEffect;

    public virtual void Init(TileData coords, float cellScale)
    {
        this._tileData = coords;
        _renderer.color = Walkable ? GetWalkableColor() : _obstacleColor;
        _defaultColor = _renderer.color;

        BindStateEvent();
        
        this.transform.localScale = Vector3.one * cellScale;
        transform.position = _tileData.Position;
        FaceDown();

    }
    public virtual void Init_EffectColor(Color c)
    {
        _effectRenderer.color = c;
    }
    public virtual void BindStateEvent()
    {
        OnHoverTile += OnOnHoverTile;

        //Turn State
        PlayerMovingTurnState.OnExitState += EndStateMove;
    }
    public virtual void CacheNeighbors(List<BaseTileOnBoard> n)
    {
        Neighbors = n;
    }
    public virtual Color GetWalkableColor()
    {
        return _walkableColor.Evaluate(Random.Range(0f, 1f));
    }

    public static event Action<BaseTileOnBoard> OnHoverTile;
    protected virtual void OnEnable() => OnHoverTile += OnOnHoverTile;
    protected virtual void OnDisable() => OnHoverTile -= OnOnHoverTile;
    private void OnOnHoverTile(BaseTileOnBoard selected) => _selected = selected == this;

    protected virtual void OnMouseDown()
    {
        if (!Walkable) return;
        OnHoverTile?.Invoke(this);
    }

    public bool IsNeighbor(BaseTileOnBoard other) => Neighbors.Contains(other);

    #region Flip
    public  virtual void FaceDown()
    {
        if (!Walkable)
            return;
        _renderer.color = _backsideColor;
        IsFaceUp = false;
        this._effectRenderer.gameObject.SetActive(IsFaceUp);
    }
    public virtual void Flip()
    {
        if (!Walkable)
            return;

        IsFaceUp = true;
        _renderer.color = GetWalkableColor();
        _defaultColor = _renderer.color;

        this._effectRenderer.gameObject.SetActive(IsFaceUp);

        this.TileEffect.Flip();
    }
    public virtual ITaskSchedule DoWhenFlip()
    {
        return this.TileEffect.CastEffect();
    }
    #endregion

    #region Moving
    public virtual void BeginStateMove()
    {

    }
    public virtual void BeginStateChose()
    {
    }
    public virtual void EndStateMove()
    {

    }
    public virtual void SetOccupation(Unit unit) { TileEffect.SetOccupation(unit); }
    public virtual void UnOccupation(Unit unit) { TileEffect.UnOccupation(unit); }
    public virtual void UnOccupateAllUnits() { TileEffect.UnOccupateAllUnits(); }

    #endregion

}
[System.Serializable]
public class TileData
{
    public int row = -1;
    public int col = -1;
    protected TileEffectType _tileEffectType;
    public TileEffectType TileEffectType => _tileEffectType;

    public int Id
    {
        get
        {
            return row * 1000 + col;
        }
    }
    public virtual bool IsEmpty
    {
        get
        {
            return false;
        }
    }
    public bool Walkable { get; protected set; }
    public Vector3 Position { get; protected set; }

    private ITileNodeEffect _nodeEffect;
    public ITileNodeEffect TileNodeEffect
    {
        get
        {
            if (_nodeEffect == null)
            {
                SetTileEffect(TileEffectType);
                SetHost(Host);
            }
            return _nodeEffect;
        }
        set { _nodeEffect = value; }
    }
    public BaseTileOnBoard Host { get; protected set; }

    public TileData(int _row, int _col, TileEffectType effectType, bool walkable, Vector3 position)
    {
        this.row = _row;
        this.col = _col;
        Walkable = walkable;
        Position = position;
        SetTileEffect(effectType);
    }
    public void SetTileEffect(TileEffectType effectType)
    {
        _tileEffectType = effectType;
        _nodeEffect = Activator.CreateInstance(EnumUtility.GetStringType(_tileEffectType)) as ITileNodeEffect;
    }
    public void SetHost(BaseTileOnBoard h)
    {
        Host = h;
        TileNodeEffect.SetHost(Host);
    }
    public float GetDistance(TileData other)
    {
        return 0f;
    }
}