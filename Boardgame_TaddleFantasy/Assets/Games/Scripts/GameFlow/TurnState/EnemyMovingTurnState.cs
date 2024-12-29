using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovingTurnState : ITurnState
{
    public override void Enter()
    {
        base.Enter(); Debug.Log($"Enter state EnemyMovingTurnState");
        OnEnterState?.Invoke();

        var allEnemies = EnemyManager.Instance.ActiveEnemies;
        if( allEnemies != null)
        {
            allEnemies.ForEach(en => { en.Move(); });
        }

        DoCallbackTask endMovingTurnTask = new DoCallbackTask(OnAllMovedComplete);
        InGameTaskManager.Instance.ScheduleNewTask(endMovingTurnTask);
    }
    void OnAllMovedComplete()
    {
        Exit();
    }
    public override void Exit()
    {
        base.Exit();
        OnExitState?.Invoke();
        Debug.Log($"Exit state EnemyMovingTurnState");
        //change state
        InGameManager.Instance.ChangeTurnState(TurnState.Enemy_Battle_Phase);
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
