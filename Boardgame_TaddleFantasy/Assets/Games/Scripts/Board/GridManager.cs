using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using Random = UnityEngine.Random;

namespace Taddle_Fantasy
{
    public class GridManager : MonoBehaviour
    {
        public static GridManager Instance;
        void Awake() => Instance = this;

        [Header("Grid Infor")]
        [SerializeField] protected SquareBoardScriptableGrid gridConfig;
        [Space(5f)]
        [SerializeField]
        protected Transform panelGrid;

        #region Getter
        public int Row => gridConfig?.Width() ?? 0;
        public int Col => gridConfig?.Height() ?? 0;
        public float CellSize => gridConfig?.CellSize() ?? 1f;
        public float Spacing => gridConfig?.Spacing() ?? 0f;

        protected BaseTileOnBoard tileItemPrefab => gridConfig?.NodePrefab;

        [SerializeField] protected Dictionary<int, BaseTileOnBoard> grids = new Dictionary<int, BaseTileOnBoard>();

        protected List<BaseTileOnBoard> tiles = new List<BaseTileOnBoard>();
        public List<BaseTileOnBoard> Items => this.grids.Values.ToList();
        #endregion

        #region Event
        public static System.Action onGridGeneratedComplete;

        #endregion

        /// <summary>
        /// If maxCellSize < 0 meaning there is no cap
        /// </summary>
        /// <param name="row"></param>
        /// <param name="col"></param>
        /// <param name="_maxCellSize"></param>
        /// <param name="onClickDiggingTile"></param>
        public virtual void InitBoard()
        {
            if (this.Row <= 0 || this.Col <= 0)
            {
                Debug.LogError($"Col or Row not empty");
                return;
            }

            grids.Clear();
            //tiles.ForEach(x => { x.SetHidden(true); x.gameObject.SetActive(false); });

            //parse the cell
            int index = 0; BaseTileOnBoard obj = null;
            var gridTileDataSetup = gridConfig.GenerateGrid();
            Dictionary<TileEffectType, Color> colors = new Dictionary<TileEffectType, Color>();
            for (int i = 0; i < this.Col; i++)
            {
                for (int j = 0; j < this.Row; j++)
                {
                    if(gridTileDataSetup.TryGetValue(new Vector2Int(j, i), out TileData t))
                    {

                        if (index >= tiles.Count)
                        {
                            obj = Instantiate(this.tileItemPrefab, this.panelGrid);
                            tiles.Add(obj);
                        }
                        else
                            obj = tiles[index];
                        obj.gameObject.SetActive(true);

                        index++;
#if UNITY_EDITOR
                        obj.name = $"{j}_{i}";
#endif
                        obj.Init(t, CellSize);
                        t.SetHost(obj);
                        if (!colors.TryGetValue(t.TileEffectType, out Color color))
                        {
                            color = gridConfig.GetNodeColor(t.TileEffectType);
                            colors.Add(t.TileEffectType, color);
                        }
                        obj.Init_EffectColor(color);
                        this.grids.TryAdd(t.Id, obj);
                    }
                }
            }

            foreach (var t in Items)
            {
                List<BaseTileOnBoard> neighBor = new List<BaseTileOnBoard>();
                foreach (var tile in BaseTileOnBoard.Dirs.Select(dir => GetTileByRowCol<BaseTileOnBoard>(t.Row + dir.x, t.Col + dir.y)).Where(tile => tile != null))
                {
                    neighBor.Add(tile);
                }

                t.CacheNeighbors(neighBor);
            }

            onGridGeneratedComplete?.Invoke();
        }


        public bool TryGetTileById<T>(int id, out T tile) where T : BaseTileOnBoard
        {
            bool isContain = this.grids.TryGetValue(id, out BaseTileOnBoard t);
            tile = (T)t;
            return isContain;
        }

        public T GetTileByRowCol<T>(int row, int col) where T : BaseTileOnBoard
        {
            var t = this.grids.Values.FirstOrDefault(x => x.Row == row && x.Col == col);
            return (T)t;
        }
        public bool TryGetTileByRowCol<T>(int row, int col, out T tile) where T : BaseTileOnBoard
        {
            tile = this.GetTileByRowCol<T>(row, col);
            return tile != null;
        }

        public void FlipAllTilesOfType(TileEffectType type)
        {
            var tiles = Items.FindAll(t=>t.EffectType == type);
            if(tiles != null && tiles.Any())
            {
                DoAnimationFlipTilesTask flipTask = new DoAnimationFlipTilesTask(tiles, null);
                InGameTaskManager.Instance.ScheduleNewTask(flipTask);
            }
        }
    }
}
