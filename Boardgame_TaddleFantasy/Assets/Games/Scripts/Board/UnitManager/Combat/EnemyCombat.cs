using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyCombat : UnitCombat
{
    public EnemyUnit EnemyUnit => this._unit as EnemyUnit;
    public override void Die()
    {
        base.Die();
        //DoEnemyKilledTask task = new DoEnemyKilledTask(EnemyUnit);
        //InGameTaskManager.Instance.ScheduleNewTask(task);
    }
}
