using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Square Grid", menuName = "Game/SquareGrid")]
public class SquareBoardScriptableGrid : ScriptableGrid
{
    [SerializeField, Range(3, 50)] private int _gridWidth = 16;
    [SerializeField, Range(3, 50)] private int _gridHeight = 9;
    [SerializeField] private float cellSize = 1f;
    [SerializeField] private float spacing = 0f;
    public override int Height() => _gridHeight;
    public override int Width() => _gridWidth;
    public float CellSize() => cellSize;
    public float Spacing() => spacing;  

    public Vector3 CellPosition(int rowID, int colID)
    {
        return new Vector3(
            x: rowID * cellSize + spacing * (rowID - 1),
            y: colID * cellSize + spacing * (colID - 1)
            );
    }
}
