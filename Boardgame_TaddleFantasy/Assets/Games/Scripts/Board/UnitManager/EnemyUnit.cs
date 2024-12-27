using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyUnit : Unit
{
    //temp
    [SerializeField] Sprite _sprite;
    [SerializeField] Vector3 noisePositionInNode = new Vector3(0.1f, 0.1f);
    public EnemyType EnemyType { get; private set; }
    public EnemyMovement MyMovement => _myMovement as EnemyMovement;

    public override void Init(Sprite sprite)
    {
        base.Init(sprite);
        _renderer.sprite = _sprite;
    }
    public virtual void Init(EnemyScriptable enemyScriptable)
    {
        base.Init(null);
        this.EnemyType = enemyScriptable.enemyType;
        //set visual
        GameObject visualObject = enemyScriptable.enemyVisual;
        if (visualObject != null)
        {
            Instantiate(visualObject, this.transform);
        }
        else
        {
            _renderer.sprite = _sprite;
        }
        //set stat
        Init_EnemyStat(enemyScriptable);
    }
    void Init_EnemyStat(EnemyScriptable enemyScriptable)
    {
        this.MyMovement.SetMovementAllow(enemyScriptable.MoveRange());
    }
    public override void SetStandingNode(BaseTileOnBoard node)
    {
        base.SetStandingNode(node);
        _myMovement.SetMeToNode(node);
    }
    public void SetStandingNodeWithNoise(BaseTileOnBoard node)
    {
        SetStandingNode(node);
        this.transform.position = GetNoisePosition(node);
    }
    public Vector3 GetNoisePosition(BaseTileOnBoard node)
    {
        return node.transform.position + new Vector3(
            x: Random.Range(-noisePositionInNode.x, noisePositionInNode.x),
            y: Random.Range(-noisePositionInNode.y, noisePositionInNode.y)
            );
    }
    public DoEnemyCastSkillTask MainTurnCastEffect()
    {
        return null;
    }

    public virtual void PlanningMovement(Vector2Int dir)
    {
        this.MyMovement.PlanningMovement(dir);
    }
    public override void Disable_RemoveFromBoard()
    {
        base.Disable_RemoveFromBoard();
        EnemyManager.Instance.KillEnemy(this);
    }
}
