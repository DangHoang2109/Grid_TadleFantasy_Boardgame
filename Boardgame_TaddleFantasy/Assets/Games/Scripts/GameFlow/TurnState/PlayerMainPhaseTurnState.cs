using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMainPhaseTurnState : ITurnState
{
    public PlayerMovement PMovement => this.PlayerTurn.MyMovement;
    public int MaxMoveAllow => this.PMovement.MaxMoveAllow;

    public override void Enter()
    {
        base.Enter();
        OnEnterState?.Invoke();
        //show UI turn depend on the player
        Debug.Log("Enter PlayerMainPhaseTurnState");
    }

    public override void Exit()
    {
        base.Exit(); 
        OnExitState?.Invoke();

        Debug.Log("Exit PlayerMainPhaseTurnState");
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
