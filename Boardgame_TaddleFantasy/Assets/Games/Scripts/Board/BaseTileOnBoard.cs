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
    protected Color _obstacleColor;

    [SerializeField] protected Gradient _walkableColor;
    [SerializeField] protected SpriteRenderer _renderer;

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

    public virtual void Init(TileData coords, float cellScale)
    {
        this._tileData = coords;
        _renderer.color = Walkable ? GetWalkableColor() : _obstacleColor;
        _defaultColor = _renderer.color;

        OnHoverTile += OnOnHoverTile;
        this.transform.localScale = Vector3.one * cellScale;
        transform.position = _tileData.Position;
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
    #endregion

}
[System.Serializable]
public class TileData
{
    public int row = -1;
    public int col = -1;
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

    public TileData(int _row, int _col, bool walkable, Vector3 position)
    {
        this.row = _row;
        this.col = _col;
        Walkable = walkable;
        Position = position;
    }
    public float GetDistance(TileData other)
    {
        return 0f;
    }
}