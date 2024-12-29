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
        EnemyManager.Instance.KillEnemy(EnemyUnit);
    }
}
