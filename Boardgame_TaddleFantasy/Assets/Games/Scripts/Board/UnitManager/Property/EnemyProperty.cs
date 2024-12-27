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
}
