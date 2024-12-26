using System.Collections;
using System.Collections.Generic;
using Taddle_Fantasy;
using UnityEngine;

public class EnemyInviteTurnState : ITurnState
{
    void SpawnEnemy()
    {
        (List<BaseTileOnBoard> gateResult, EnemyType typeToSpawn, int enemyResult) inviteResult = EnemyManager.Instance.GenerateInviteResult();

        DoInviteEnemyTask task = new DoInviteEnemyTask(inviteResult.gateResult, inviteResult.typeToSpawn, inviteResult.enemyResult);
        InGameTaskManager.Instance.ScheduleNewTask(task);

        //spawn into Cosmic Node
        var cosmicRevealedNode = GridManager.Instance.GetTilesOfType(TileEffectType.Cosmic);
        if(cosmicRevealedNode != null && cosmicRevealedNode.Count > 0)
        {

            DoInviteEnemyTask cosmicTask = new DoInviteEnemyTask(cosmicRevealedNode, EnemyType.None, 1);
            InGameTaskManager.Instance.ScheduleNewTask(cosmicTask);
        }
    }
    public override void Enter()
    {
        base.Enter(); Debug.Log($"Enter state EnemyInviteTurnState");
        OnEnterState?.Invoke();

        SpawnEnemy();
    }

    public override void Exit()
    {
        base.Exit();
        OnExitState?.Invoke();
        Debug.Log($"Exit state EnemyInviteTurnState");
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
