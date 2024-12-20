using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PlayerMovingTurnState : ITurnState
{
    public PlayerMovement PMovement => this.PlayerTurn.MyMovement;


    public override void Enter()
    {
        base.Enter(); Debug.Log($"Enter state PlayerMovingTurnState");
        OnEnterState?.Invoke();
        PMovement.Move();
    }

    public override void Exit()
    {
        base.Exit(); 
        OnExitState?.Invoke();
        Debug.Log($"Exit state PlayerMovingTurnState");
        //change state
        InGameManager.Instance.ChangeTurnState(TurnState.End_Turn);
    }

    public static System.Action OnEnterState, OnExitState;
    public static void RegisterEnterStateCallback(System.Action cb)
    {
        UnRegisterEnterStateCallback(cb);
        OnEnterState += cb;
    }
    public static void RegisterExitStateCallback(System.Action cb)
    {
        UnRegisterExistStateCallback(cb);
        OnExitState += cb;
    }
    public static void UnRegisterEnterStateCallback(System.Action cb)
    {
        OnEnterState -= cb;
    }
    public static void UnRegisterExistStateCallback(System.Action cb)
    {
        OnExitState -= cb;
    }
}
