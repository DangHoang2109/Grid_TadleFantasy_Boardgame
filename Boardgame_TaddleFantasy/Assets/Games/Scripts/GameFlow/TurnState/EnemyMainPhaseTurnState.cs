using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Taddle_Fantasy;
using UnityEngine;

public class EnemyMainPhaseTurnState : ITurnState
{
    void UpgradeCreepToElite()
    {
        var allGrid = GridManager.Instance.GetAllTiles();
        foreach (var tile in allGrid)
        {
            var unitOnTile = tile.UnitsOnTiles();
            if(unitOnTile != null && unitOnTile.Count > 0)
            {
                var enemiesCreep = unitOnTile.FindAll(e => e is EnemyUnit enemyUnit && enemyUnit.EnemyType == EnemyType.None);
                if(enemiesCreep != null && enemiesCreep.Count >= EliteScriptableEnemy.AmountCreepToUpgrade)
                {
                    var creepToUpgrade = enemiesCreep.Take(EliteScriptableEnemy.AmountCreepToUpgrade).ToList();
                    tile.UnOccupateUnits(creepToUpgrade);
                    creepToUpgrade.ForEach(e => e.Disable_RemoveFromBoard());
                    EnemyManager.Instance.SpawnAnEnemy(EnemyType.Elite, tile);
                }
            }
        }
    }
    void AllEnemyPlayTheirMainTurn()
    {
        //upgrade first
        UpgradeCreepToElite();

        //Now we do their behavior
        var allEnemies = EnemyManager.Instance.ActiveEnemies;
        if(allEnemies != null && allEnemies.Count > 0)
        {
            foreach (var enemy in allEnemies)
            {
                DoEnemyCastSkillTask turnTask = enemy.MainTurnCastEffect();
                if (turnTask != null)
                {
                    InGameTaskManager.Instance.ScheduleNewTask(turnTask, autoRun: false);
                }
            }

            var callbackEndMainTurnTask = new DoCallbackTask(AllEnemiesCompletePlayTurn);
            InGameTaskManager.Instance.ScheduleNewTask(callbackEndMainTurnTask);
        }
    }
    void AllEnemiesCompletePlayTurn()
    {
        AllEnemiesPlanningMoving();
        Exit();
    }
    void AllEnemiesPlanningMoving()
    {
        //decide a direction
        var Dirs = BaseTileOnBoard.Dirs;
        Vector2Int moveDir = Dirs.Random();

        //Foreach enemy, let's they decide their moving behavior
        var allEnemies = EnemyManager.Instance.ActiveEnemies;
        if (allEnemies != null && allEnemies.Count > 0)
        {
            foreach (var enemy in allEnemies)
            {
                enemy.PlanningMovement(moveDir);
            }
        }
    }
    public override void Enter()
    {
        base.Enter(); Debug.Log($"Enter state EnemyMainPhaseTurnState");
        OnEnterState?.Invoke();
        AllEnemyPlayTheirMainTurn();
    }

    public override void Exit()
    {
        base.Exit();
        OnExitState?.Invoke();
        Debug.Log($"Exit state EnemyMainPhaseTurnState");
        //change state
        InGameManager.Instance.ChangeTurnState(TurnState.Enemy_Moving_Phase);
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

