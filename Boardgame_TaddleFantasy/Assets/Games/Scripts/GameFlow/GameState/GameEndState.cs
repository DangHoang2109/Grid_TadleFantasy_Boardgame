using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEndState : IGameState
{
    public override void Enter()
    {
        base.Enter();
        OnEnterState?.Invoke();
        //show UI turn depend on the player
        Debug.Log("Enter GameEndState");

        //Show EndGame Popup
        LoseGamePopup.Show();
    }
    public override void Exit()
    {
        base.Exit();
        OnExitState?.Invoke();

        Debug.Log("Exit GameEndState");
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

