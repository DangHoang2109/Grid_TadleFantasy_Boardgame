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

    public static System.Action<BaseTileOnBoard> OnNodeAddedToPlan;
    public static System.Action<List<BaseTileOnBoard>> OnPlayerHoverNode;
    public static System.Action OnPlayerMove;

    private void OnEnable()
    {
        BaseTileOnBoard.OnHoverTile -= OnNodeClicked;
        BaseTileOnBoard.OnHoverTile += OnNodeClicked;
        _playerUnit = GetComponent<PlayerUnit>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Move();
    }

    void OnNodeClicked(BaseTileOnBoard nodeClicked)
    {
        //check turn, check số lượt move
        if (!IsMoveable(nodeClicked))
            return;

        _planningNode.Add(nodeClicked);
        OnNodeAddedToPlan?.Invoke(nodeClicked);
    }
    bool IsMoveable(BaseTileOnBoard nodeClicked)
    {
        if(MovementAllowLeft() < 0)
            return false;
        if(!IsInTurn())
            return false;
        //ô liền kề ô vừa chọn hoặc đang đứng (ô chọn đầu tiên)
        if (!LastChoosingNode().IsNeighbor(nodeClicked))
            return false;
        return true;
    }
    bool IsInTurn() => true;
    public int MovementAllowLeft() => Mathf.Clamp(playerMoveAllow - _planningNode.Count, 0, int.MaxValue);
    BaseTileOnBoard LastChoosingNode() => _planningNode.Count > 0 ? _planningNode.LastOrDefault() : _standingNode;
    void Move()
    {
        if(_planningNode.Count == 0)
            return;

        DOTween.Kill(this);
        Sequence seq = DOTween.Sequence().SetId(this);
        foreach (var node in _planningNode)
        {
            seq.Append(_playerUnit.MoveToNode(node));
            seq.AppendCallback(() => { SetMeToNode(node); OnPlayerMove?.Invoke(); });
            seq.AppendInterval(DELAY_TIME_MOVE_BETWEEN_NODE);
        }

        seq.OnComplete(() =>
        {
            _planningNode.Clear();
        });
    }
    public void SetMeToNode(BaseTileOnBoard node)
    {
        this._standingNode = node;
        Debug.Log("Set me to node");
    }
}
