using _Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTileOnBoardNode : SquareNode
{
    [SerializeField] protected Color _backsideColor, _choosingColor;
    public bool IsFaceUp { get; protected set; }

    protected override void OnEnable()
    {
        base.OnEnable();

        PlayerMovement.OnNodeAddedToPlan -= OnChooseToPlan;
        PlayerMovement.OnNodeAddedToPlan += OnChooseToPlan;
    }
    void OnChooseToPlan(NodeBase node)
    {
        if(node == this)
        {
            _renderer.color = _choosingColor;
        }
        
    }
    public override void Init(bool walkable, ICoords coords)
    {
        base.Init(walkable, coords);
        FaceDown();
    }
    public void FaceDown()
    {
        if (!Walkable)
            return;
        _renderer.color = _backsideColor;
        IsFaceUp = false;
    }
    public void Flip()
    {
        if (!Walkable)
            return;

        IsFaceUp = true;
        _renderer.color = GetWalkableColor();
        _defaultColor = _renderer.color;

        DoWhenFlip();
    }
    public override Color GetWalkableColor()
    {
        return _walkableColor.Evaluate(0f);
    }
    private void Update()
    {
        if (Input.GetMouseButtonDown(1)) 
            FaceDown();
    }
    public virtual void DoWhenFlip()
    {
        Debug.Log("Call Activation Instance to do effect");
    }
}
