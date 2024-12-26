using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMainPhaseTurnState : ITurnState
{
    public override void Enter()
    {
        base.Enter(); Debug.Log($"Enter state EnemyMainPhaseTurnState");
        OnEnterState?.Invoke();

    }

    public override void Exit()
    {
        base.Exit();
        OnExitState?.Invoke();
        Debug.Log($"Exit state EnemyMainPhaseTurnState");
        //change state
        InGameManager.Instance.ChangeTurnState(TurnState.Moving_Phase);
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

