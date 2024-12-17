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
    [SerializeField] NodeBase _standingNode;

    [SerializeField] int playerMoveAllow;
    [SerializeField] List<NodeBase> _planningNode = new List<NodeBase>();

    public static System.Action<NodeBase> OnNodeAddedToPlan;
    public static System.Action<List<NodeBase>> OnPlayerHoverNode;
    public static System.Action OnPlayerMove;

    private void OnEnable()
    {
        NodeBase.OnHoverTile -= OnNodeClicked;
        NodeBase.OnHoverTile += OnNodeClicked;
        _playerUnit = GetComponent<PlayerUnit>();
    }
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            Move();
    }

    void OnNodeClicked(NodeBase nodeClicked)
    {
        //check turn, check số lượt move
        if (!IsMoveable(nodeClicked))
            return;

        _planningNode.Add(nodeClicked);
        OnNodeAddedToPlan?.Invoke(nodeClicked);
    }
    bool IsMoveable(NodeBase nodeClicked)
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
    NodeBase LastChoosingNode() => _planningNode.Count > 0 ? _planningNode.LastOrDefault() : _standingNode;
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
    public void SetMeToNode(NodeBase node)
    {
        this._standingNode = node;
        Debug.Log("Set me to node");
    }
}
