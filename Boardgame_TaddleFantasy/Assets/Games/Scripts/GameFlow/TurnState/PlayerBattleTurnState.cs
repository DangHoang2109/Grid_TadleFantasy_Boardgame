using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBattleTurnState : ITurnState
{
    public PlayerCombat PlayerCombat => this.PlayerTurn.MyCombat as PlayerCombat;
    void AttackSameNodeEnemy()
    {
        var standingNode = this.PlayerTurn.StandingNode;
        if (standingNode == null)
            return;

        var enemiesInSameNode = standingNode.UnitsOnTiles();
        if (enemiesInSameNode != null && enemiesInSameNode.Count > 0)
        {
            enemiesInSameNode = enemiesInSameNode.FindAll(u => u is EnemyUnit);
            if (enemiesInSameNode.Count == 0) { Exit(); return; }
        }
        else
        { Exit(); return; }

        int playerResult = PlayerCombat.GenerateRollingAttackDiceResult();
        int enemiesResult = CombatManager.GenerateRollingAttackDiceResult(enemiesInSameNode.Count);

        bool isPlayerWin = playerResult > enemiesResult;

        DoAttackTask task = new DoAttackTask(
            attacker: this.PlayerTurn,
            defenders: enemiesInSameNode,
            callback: null,
            isWin : isPlayerWin,
            attackerDiceResult: playerResult,
            defenderDiceResult: enemiesResult            
            );
        InGameTaskManager.Instance.ScheduleNewTask(task, autoRun: false);

        DoCallbackTask completeAttackPhase = new DoCallbackTask(Exit);
        InGameTaskManager.Instance.ScheduleNewTask(completeAttackPhase);
    }

    public override void Enter()
    {
        base.Enter(); Debug.Log($"Enter state PlayerBattleTurnState");
        OnEnterState?.Invoke();
        AttackSameNodeEnemy();
    }

    public override void Exit()
    {
        base.Exit();
        OnExitState?.Invoke();
        Debug.Log($"Exit state PlayerBattleTurnState");
        //change state
        InGameManager.Instance.ChangeTurnState(TurnState.Main_Phase);
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
