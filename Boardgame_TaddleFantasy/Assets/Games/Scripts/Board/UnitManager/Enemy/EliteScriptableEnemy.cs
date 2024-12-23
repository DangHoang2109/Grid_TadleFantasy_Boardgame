using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Scriptable Enemy", menuName = "Game/Enemy/EliteEnemy")]
public class EliteScriptableEnemy : EnemyScriptable
{
    public override void Attack()
    {
        throw new System.NotImplementedException();
    }

    public override void CastEffect()
    {
        throw new System.NotImplementedException();
    }

    public override bool VerifySpawn(BaseTileOnBoard tileToSpawn)
    {
        throw new System.NotImplementedException();
    }
}
