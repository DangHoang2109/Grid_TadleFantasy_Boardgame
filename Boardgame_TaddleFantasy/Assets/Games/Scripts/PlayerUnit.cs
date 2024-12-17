using _Scripts.Tiles;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
    public static System.Action OnPlayerMove;
    // Start is called before the first frame update
    void Start()
    {
        NodeBase.OnHoverTile -= OnNodeClicked;
        NodeBase.OnHoverTile += OnNodeClicked;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnNodeClicked(NodeBase nodeClicked)
    {
        MoveToNode(nodeClicked);
    }
    public virtual void MoveToNode(NodeBase nodeDestination)
    {
        this.transform.DOJump(nodeDestination.transform.position, 0.5f, 1, 0.2f).SetEase(Ease.InQuad);
        if (nodeDestination is SquareTileOnBoardNode tileNode)
            tileNode.Flip();

        OnPlayerMove?.Invoke();
    }
}
