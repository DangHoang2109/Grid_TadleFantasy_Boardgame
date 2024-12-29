using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyScriptable : UnitScriptable
{
    [SerializeField] public EnemyType enemyType;

    public abstract void CastEffect();
    public abstract bool VerifySpawn(BaseTileOnBoard tileToSpawn);
    public abstract void Attack();
}
