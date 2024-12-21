using _Scripts.Tiles;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const float DELAY_TIME_MOVE_BETWEEN_NODE = 0.1f;
    [SerializeField] PlayerUnit _playerUnit;
    public PlayerUnit PlayerUnit => _playerUnit;

    [SerializeField] BaseTileOnBoard _standingNode;

    [SerializeField] int playerMoveAllow;
    [SerializeField] List<BaseTileOnBoard> _planningNode = new List<BaseTileOnBoard>();

    public static System.Action<BaseTileOnBoard> OnNodeAddedToPlan, OnNodeRemovedFromPlan;
    public static System.Action<string> OnMovementPlan;
    public static System.Action OnPlayerMove;

    public int MaxMoveAllow => playerMoveAllow;

    private void OnEnable()
    {
        BaseTileOnBoard.OnHoverTile -= OnNodeClicked;
        BaseTileOnBoard.OnHoverTile += OnNodeClicked;

        _playerUnit = GetComponent<PlayerUnit>();
    }
    private void Update()
    {
        //if (Input.GetKeyDown(KeyCode.Space))
        //    Move();
    }

    void OnNodeClicked(BaseTileOnBoard nodeClicked)
    {
        if (nodeClicked == LastChoosingNode())
        {
            _planningNode.Remove(nodeClicked);
            OnNodeRemovedFromPlan?.Invoke(nodeClicked);
            OnMovementPlan?.Invoke(StringMovementStatus());
            return;
        }

        //check turn, check số lượt move
        if (!IsMoveable(nodeClicked))
            return;

        foreach (SquareTileOnBoardNode node in _planningNode)
        {
            node.CleanChosableMyNeighbor();
        }
        _planningNode.Add(nodeClicked);
        OnNodeAddedToPlan?.Invoke(nodeClicked);
        OnMovementPlan?.Invoke(StringMovementStatus());
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
    bool IsInTurn() => true;
    public int MovementAllowLeft() => Mathf.Clamp(playerMoveAllow - _planningNode.Count, 0, int.MaxValue);
    public string StringMovementStatus() => $"{MovementAllowLeft()}/{playerMoveAllow}";
    BaseTileOnBoard LastChoosingNode() => _planningNode.Count > 0 ? _planningNode.LastOrDefault() : _standingNode;
    public virtual Tween MoveToNode(BaseTileOnBoard nodeDestination, System.Action onComplete = null)
    {
        if (nodeDestination is SquareTileOnBoardNode tileNode)
            tileNode.Flip();

        return this._playerUnit.transform.DOJump(nodeDestination.transform.position, 0.5f, 1, 0.2f).SetEase(Ease.InQuad).OnComplete(() => { onComplete?.Invoke(); });
    }
    public void Move()
    {
        if(_planningNode.Count == 0)
            return;

        foreach (var node in _planningNode)
        {
            node.SetOccupation(this._playerUnit);
            DoMoveToNodeTask moveTask = new DoMoveToNodeTask(this, node, onCompleteJump: () =>
            {
                node.UnOccupation(this._playerUnit); node.EndStateMove(); OnPlayerMove?.Invoke();
            }, onCompletelyDoneTask: null);
            InGameTaskManager.Instance.ScheduleNewTask(moveTask, autoRun: false);
            var tileTask = node.DoWhenFlip();
            if(tileTask != null)
                InGameTaskManager.Instance.ScheduleNewTask(tileTask, autoRun: false);
        }

        DoCallbackTask endTask = new DoCallbackTask(callback: () =>
        {
            this._playerUnit.SetStandingNode(_planningNode.Last());
            _planningNode.Clear();
            InGameManager.Instance.ChangeTurnState(TurnState.End_Turn);
        });
        InGameTaskManager.Instance.ScheduleNewTask(endTask);
    }
    public void SetMeToNode(BaseTileOnBoard node)
    {
        if (_standingNode != null)
            _standingNode.UnOccupation(this._playerUnit);

        this._standingNode = node;
        _standingNode.SetOccupation(this._playerUnit);
        Debug.Log("Set me to node");
    }
}
