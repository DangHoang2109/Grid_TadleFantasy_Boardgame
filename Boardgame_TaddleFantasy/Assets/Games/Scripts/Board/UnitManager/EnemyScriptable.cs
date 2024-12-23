using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class EnemyScriptable : ScriptableObject
{
    [SerializeField] public EnemyType enemyType;
    [SerializeField] public GameObject enemyVisual;
    [SerializeField] protected int maxHp;
    [SerializeField] protected int attackDmg;
    [SerializeField] protected int killedReward;
    [SerializeField] protected int attackRange;
    [SerializeField] protected int moveRange;

    public abstract void CastEffect();
    public abstract bool VerifySpawn(BaseTileOnBoard tileToSpawn);
    public abstract void Attack();
}
