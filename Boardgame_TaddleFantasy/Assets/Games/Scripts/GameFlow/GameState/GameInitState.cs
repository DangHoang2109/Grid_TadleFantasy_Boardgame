using System.Collections;
using System.Collections.Generic;
using Taddle_Fantasy;
using UnityEngine;

public class GameInitState : IGameState
{

    public override void Enter()
    {
        base.Enter();
        OnEnterState?.Invoke();
        //show UI turn depend on the player
        Debug.Log("Enter GameInitState");

        GridManager.onGridGeneratedComplete -= OnInitComplete;
        GridManager.onGridGeneratedComplete += OnInitComplete;

        UnitManager.Instance.Init();
        GridManager.Instance.InitBoard();
        FloatBubbleManager.Instance.Init();
    }
    void OnInitComplete()
    {
        InGameManager.Instance.ChangeGameState(GameState.Start);
    }
    public override void Exit()
    {
        base.Exit();
        OnExitState?.Invoke();

        Debug.Log("Exit GameInitState");
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
