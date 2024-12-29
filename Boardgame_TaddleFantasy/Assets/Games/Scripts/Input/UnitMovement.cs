using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class UnitMovement : MonoBehaviour
{
    public virtual float DelayTimeMoveBetweenNode => 0.1f;
    [SerializeField] protected Unit _unit;
    [SerializeField] protected BaseTileOnBoard _standingNode;

    [SerializeField] protected List<BaseTileOnBoard> _planningNode = new List<BaseTileOnBoard>();
    [SerializeField] protected int _moveAllow;

    public BaseTileOnBoard StandingNode => _standingNode;

    protected virtual void OnEnable()
    {
        BaseTileOnBoard.OnHoverTile -= OnNodeClicked;
        BaseTileOnBoard.OnHoverTile += OnNodeClicked;

        _unit = GetComponent<Unit>();
    }
    protected virtual void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Move();
    }

    protected virtual void OnNodeClicked(BaseTileOnBoard nodeClicked)
    {
        if (nodeClicked == LastChoosingNode())
        {
            OnNodeClicked_RemoveNode(nodeClicked);
            return;
        }

        //check turn, check số lượt move
        if (!IsMoveable(nodeClicked))
            return;
        OnNodeClicked_AddNode(nodeClicked);
    }
    protected virtual void OnNodeClicked_RemoveNode(BaseTileOnBoard nodeClicked)
    {
        _planningNode.Remove(nodeClicked);
    }
    protected virtual void OnNodeClicked_AddNode(BaseTileOnBoard nodeClicked)
    {
        foreach (SquareTileOnBoardNode node in _planningNode)
        {
            node.CleanChosableMyNeighbor();
        }
        _planningNode.Add(nodeClicked);
    }
    bool IsMoveable(BaseTileOnBoard nodeClicked)
    {
        if (MovementAllowLeft() <= 0)
            return false;
        if (!IsInTurn())
            return false;

        if (_planningNode.Find(x => x.GridId == nodeClicked.GridId) != null)
            return false;
        //ô liền kề ô vừa chọn hoặc đang đứng (ô chọn đầu tiên)
        if (!LastChoosingNode().IsNeighbor(nodeClicked))
            return false;
        return true;
    }
    protected bool IsInTurn() => true;

    public virtual int MovementAllowLeft() => Mathf.Clamp(_moveAllow - _planningNode.Count, 0, int.MaxValue);
    public virtual void SetMovementAllow(int v) => _moveAllow = v;


    protected BaseTileOnBoard LastChoosingNode() => _planningNode.Count > 0 ? _planningNode.LastOrDefault() : _standingNode;

    public virtual Tween MoveToNode(BaseTileOnBoard nodeDestination, System.Action onComplete = null)
    {
        if (nodeDestination is SquareTileOnBoardNode tileNode)
            tileNode.Flip();

        return this._unit.transform.DOJump(nodeDestination.transform.position, 0.5f, 1, 0.2f).SetEase(Ease.InQuad).OnComplete(() => { onComplete?.Invoke(); });
    }

    protected virtual void OnCompleteMoveToNode(BaseTileOnBoard node)
    {
        node.UnOccupation(this._unit);
        node.EndStateMove();
    }
    protected virtual void OnCompleteMoveAllNodes()
    {
        this._unit.SetStandingNode(_planningNode.Last());
        _planningNode.Clear();
    }
    public virtual void Move()
    {
        if (_planningNode.Count == 0)
            return;

        foreach (var node in _planningNode)
        {
            node.SetOccupation(this._unit);
            DoMoveToNodeTask moveTask = new DoMoveToNodeTask(this, node, onCompleteJump: () =>
            {
                OnCompleteMoveToNode(node);
            }, onCompletelyDoneTask: null);
            InGameTaskManager.Instance.ScheduleNewTask(moveTask, autoRun: false);
            var tileTask = node.DoWhenFlip();
            if (tileTask != null)
                InGameTaskManager.Instance.ScheduleNewTask(tileTask, autoRun: false);
        }

        DoCallbackTask endTask = new DoCallbackTask(callback: () =>
        {
            OnCompleteMoveAllNodes();
        });
        InGameTaskManager.Instance.ScheduleNewTask(endTask);
    }
    public void SetMeToNode(BaseTileOnBoard node)
    {
        if (_standingNode != null)
            _standingNode.UnOccupation(this._unit);

        this._standingNode = node;
        _standingNode.SetOccupation(this._unit);
    }
}
