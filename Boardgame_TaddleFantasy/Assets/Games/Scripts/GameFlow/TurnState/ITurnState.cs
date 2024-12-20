using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ITurnState : IState
{
    public PlayerUnit PlayerTurn {  get; private set; }
    public int PlayerIndex { get; private set; }

    public virtual void Init(PlayerUnit playerTurn, int playerIndex)
    {
        PlayerIndex = playerIndex;
        PlayerTurn = playerTurn;
    }
}
