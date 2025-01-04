using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyProperty : UnitProperty
{
    public override void InitStat(UnitScriptable config)
    {
        base.InitStat(config);
        EnemyScriptable enemyScriptable = config as EnemyScriptable;

    }
    public override void Die()
    {
        base.Die();
        DoEnemyKilledTask task = new DoEnemyKilledTask(this._unit);
        InGameTaskManager.Instance.ScheduleNewTask(task);
    }
}
