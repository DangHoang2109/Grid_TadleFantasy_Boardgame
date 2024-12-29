using _Scripts.Tiles;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : UnitMovement
{    public PlayerUnit PlayerUnit => _unit as PlayerUnit;


    public static System.Action<BaseTileOnBoard> OnNodeAddedToPlan, OnNodeRemovedFromPlan;
    public static System.Action<string> OnMovementPlan;
    public static System.Action OnPlayerMove;

    public int MaxMoveAllow => _moveAllow;


    protected override void OnNodeClicked_RemoveNode(BaseTileOnBoard nodeClicked)
    {
        base.OnNodeClicked_RemoveNode(nodeClicked);
        OnNodeRemovedFromPlan?.Invoke(nodeClicked);
        OnMovementPlan?.Invoke(StringMovementStatus());
    }
    protected override void OnNodeClicked_AddNode(BaseTileOnBoard nodeClicked)
    {
        base.OnNodeClicked_AddNode(nodeClicked);
        OnNodeAddedToPlan?.Invoke(nodeClicked);
        OnMovementPlan?.Invoke(StringMovementStatus());
    }


    public string StringMovementStatus() => $"{MovementAllowLeft()}/{MaxMoveAllow}";

    protected override void OnCompleteMoveToNode(BaseTileOnBoard node)
    {
        base.OnCompleteMoveToNode(node);
        OnPlayerMove?.Invoke();
    }
    protected override void OnCompleteMoveAllNodes()
    {
        base.OnCompleteMoveAllNodes();
        InGameManager.Instance.ExistState(TurnState.Moving_Phase);
    }
}
