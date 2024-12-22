using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Taddle_Fantasy;
using UnityEngine;

public class GameStartState : IGameState
{
    public override void Enter()
    {
        base.Enter();
        OnEnterState?.Invoke();
        //show UI turn depend on the player
        Debug.Log("Enter GameStartState");

        Debug.Log("Let's main player choose the starting tile");
        var node = GridManager.Instance.Items.Where(t => t.Walkable && t.EffectType != TileEffectType.Gate).OrderBy(t => UnityEngine.Random.value).First();
        UnitManager.Instance.StartGame_MainPlayerPickNode(node);
        Debug.Log("Let's reveal gate node");

        GridManager.Instance.FlipAllTilesOfType(TileEffectType.Gate);

        Debug.Log("Let's spawn starting enemy");
        DoCallbackTask spawnEnemiesTask = new DoCallbackTask(() => { EnemyManager.Instance.StartGame(); });
        InGameTaskManager.Instance.ScheduleNewTask(spawnEnemiesTask);

        OnStartComplete();
    }
    void OnStartComplete()
    {
        InGameManager.Instance.ChangeGameState(GameState.Player_Turn);
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

