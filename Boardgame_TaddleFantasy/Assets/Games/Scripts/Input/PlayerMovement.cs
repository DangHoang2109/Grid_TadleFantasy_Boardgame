using _Scripts.Tiles;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public const float DELAY_TIME_MOVE_BETWEEN_NODE = 0.1f;
    [SerializeField] PlayerUnit _playerUnit;
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

    public void Move()
    {
        if(_planningNode.Count == 0)
            return;

        DOTween.Kill(this);
        Sequence seq = DOTween.Sequence().SetId(this);
        foreach (var node in _planningNode)
        {
            seq.Append(_playerUnit.MoveToNode(node));
            seq.AppendCallback(() => { SetMeToNode(node); node.EndStateMove() ; OnPlayerMove?.Invoke(); });
            seq.AppendInterval(DELAY_TIME_MOVE_BETWEEN_NODE);
        }

        seq.OnComplete(() =>
        {
            _planningNode.Clear();
            InGameManager.Instance.ChangeTurnState(TurnState.End_Turn);
        });
    }
    public void SetMeToNode(BaseTileOnBoard node)
    {
        this._standingNode = node;
        Debug.Log("Set me to node");
    }
}
