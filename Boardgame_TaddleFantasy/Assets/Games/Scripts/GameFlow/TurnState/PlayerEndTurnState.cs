using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEndTurnState : ITurnState
{
    public override void Enter()
    {
        base.Enter();

        Debug.Log($"Enter state PlayerEndTurnState");
        OnEnterState?.Invoke();

        InGameManager.Instance.ChangeGameState(GameState.Player_Turn);
    }
    public override void Exit()
    {
        base.Exit(); Debug.Log($"exit state PlayerEndTurnState");
        OnExitState?.Invoke();
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
