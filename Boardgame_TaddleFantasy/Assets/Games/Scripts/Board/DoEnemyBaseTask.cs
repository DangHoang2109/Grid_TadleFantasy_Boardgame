using System.Collections;
using System.Collections.Generic;
using Taddle_Fantasy;
using UnityEngine;

public class DoEnemyBaseTask : ITaskSchedule
{
}
public class DoInviteEnemyTask : DoEnemyBaseTask
{
    private List<BaseTileOnBoard> _nodesToSpawn;
    private int _enemyAmount;
    private EnemyType _enemyTypeToSpawn;
    public DoInviteEnemyTask(List<BaseTileOnBoard> nodesToSpawn, EnemyType enemyTypeToSpawn, int enemyAmount)
    {   
        _nodesToSpawn = nodesToSpawn;
        _enemyTypeToSpawn = enemyTypeToSpawn;
        _enemyAmount = enemyAmount;
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();

        bool isComplete = false;
        //find the gate
   
        if (_nodesToSpawn.Count <= 0) { yield break; }

        _enemyAmount = Mathf.Clamp(_enemyAmount, 1, int.MaxValue);
        EnemyManager.Instance.SpawnEnemies(_enemyTypeToSpawn, _enemyAmount, _nodesToSpawn);

        //do animation if you want
        isComplete = true;
        InGameManager.Instance.ExistState(TurnState.Enemy_Invite_Phase);
        yield return new WaitUntil(() => isComplete);
    }
}

public class DoEnemyCastSkillTask : DoEnemyBaseTask
{
}