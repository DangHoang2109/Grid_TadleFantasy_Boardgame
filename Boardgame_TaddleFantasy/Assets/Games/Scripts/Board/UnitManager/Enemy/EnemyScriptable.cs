using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyScriptable : UnitScriptable
{
    [SerializeField] public EnemyType enemyType;
    [SerializeField] public int attackPower;
    public abstract void CastEffect();
    public abstract bool VerifySpawn(BaseTileOnBoard tileToSpawn);
    public abstract void Attack();

    public override int PowerPoint()
    {
        return attackPower;
    }
}
