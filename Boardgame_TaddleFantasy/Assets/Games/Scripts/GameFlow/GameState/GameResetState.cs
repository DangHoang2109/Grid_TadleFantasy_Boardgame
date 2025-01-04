using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Taddle_Fantasy;

public class GameResetState : IGameState
{
    public override void Enter()
    {
        base.Enter();
        OnEnterState?.Invoke();
        //show UI turn depend on the player
        Debug.Log("Enter GameResetState");

        DoResetGameTask task = new DoResetGameTask();
        InGameTaskManager.Instance.ScheduleNewTask(task);
    }
    public override void Exit()
    {
        base.Exit();
        OnExitState?.Invoke();

        Debug.Log("Exit GameResetState");
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

