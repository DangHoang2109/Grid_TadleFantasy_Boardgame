using _Scripts.Tiles;
using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerUnit : Unit
{
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
        this.transform.DOJump(nodeClicked.transform.position, 0.5f, 1, 0.2f).SetEase(Ease.InQuad);
        if (nodeClicked is SquareTileOnBoardNode tileNode)
            tileNode.Flip();
    }
}
