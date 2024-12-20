using _Scripts.Tiles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SquareTileOnBoardNode : BaseTileOnBoard
{
    [SerializeField] protected Color _backsideColor, _choosingColor, _canChoseColor;
    public bool IsFaceUp { get; protected set; }
    public bool IsChosingToBeMoved { get; protected set; }
    [SerializeField] SpriteRenderer _movingPlanRenderer;

    protected override void OnEnable()
    {
        base.OnEnable();

        PlayerMovement.OnNodeAddedToPlan -= OnChooseToPlan;
        PlayerMovement.OnNodeAddedToPlan += OnChooseToPlan;

        PlayerMovement.OnNodeRemovedFromPlan -= OnRemoveFromPlan;
        PlayerMovement.OnNodeRemovedFromPlan += OnRemoveFromPlan;

        ShowChosableToMve(false);
    }
    public void ShowChosableToMve(bool isShow)
    {
        _movingPlanRenderer.gameObject.SetActive(isShow);
        _movingPlanRenderer.color = _canChoseColor;
    }
    void OnChooseToPlan(BaseTileOnBoard node)
    {
        if(node == this)
        {
            IsChosingToBeMoved = true;
            _movingPlanRenderer.gameObject.SetActive(true);
            _movingPlanRenderer.color = _choosingColor;
            //foreach (var neighbor in this.Neighbors)
            //{
            //    if(neighbor is SquareTileOnBoardNode gameTile && !gameTile.IsChosingToBeMoved)
            //    {
            //        gameTile.ShowChosableToMve(isShow: true);
            //    }
            //}
        }
    }
    public void CleanChosableMyNeighbor()
    {
        foreach (var neighbor in this.Neighbors)
        {
            if (neighbor is SquareTileOnBoardNode gameTile && !gameTile.IsChosingToBeMoved)
            {
                gameTile.ShowChosableToMve(isShow: false);
            }
        }
    }
    void OnRemoveFromPlan(BaseTileOnBoard node)
    {
        if (node.GridId == this.GridId)
        {
            IsChosingToBeMoved = false;
            _movingPlanRenderer.gameObject.SetActive(false);
            CleanChosableMyNeighbor();
        }
    }
    public override void Init(TileData coords, float cellScale)
    {
        base.Init(coords, cellScale);
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

    public override void BeginStateChose()
    {
        base.BeginStateChose(); OnRemoveFromPlan(this);
    }
    public override void EndStateMove()
    {
        base.EndStateMove(); OnRemoveFromPlan(this);
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
