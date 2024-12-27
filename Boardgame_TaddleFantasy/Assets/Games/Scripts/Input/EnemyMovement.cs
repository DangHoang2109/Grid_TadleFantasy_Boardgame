using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Taddle_Fantasy;
using UnityEngine;

public class EnemyMovement : UnitMovement
{
    public override float DelayTimeMoveBetweenNode => 0f; 
    EnemyUnit EnemyUnit => this._unit as EnemyUnit;
    public override Tween MoveToNode(BaseTileOnBoard nodeDestination, System.Action onComplete = null)
    {
        if (nodeDestination is SquareTileOnBoardNode tileNode)
            tileNode.Flip();

        return this._unit.transform.DOJump(EnemyUnit.GetNoisePosition(nodeDestination), 0.5f, 1, 0.2f).SetEase(Ease.InQuad).OnComplete(() => { onComplete?.Invoke(); });
    }
    public virtual void PlanningMovement(Vector2Int dir)
    {
        //move all the range allow, if you have other movement behavior, inherit this script
        for (int i = 0; i < this._moveAllow; i++)
        {
            var lastChoseNodeOrStandingNdoe = LastChoosingNode();

            var tile = GridManager.Instance.GetTileByRowCol<BaseTileOnBoard>(lastChoseNodeOrStandingNdoe.Row + dir.x, lastChoseNodeOrStandingNdoe.Col + dir.y);
            if(tile == null) {
                break;
            }
            OnNodeClicked(tile);
        }
    }
}
