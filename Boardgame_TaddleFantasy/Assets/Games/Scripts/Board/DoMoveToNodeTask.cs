using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Taddle_Fantasy;
using UnityEngine;

public class DoMoveToNodeTask : ITaskSchedule
{
    private UnitMovement _unit;
    private BaseTileOnBoard _tileOnBoard;
    private System.Action _onCompleteJump, _onCompletelyDoneTask;
    public DoMoveToNodeTask(UnitMovement unit, BaseTileOnBoard tileOnBoard,
        System.Action onCompleteJump, System.Action onCompletelyDoneTask)
    {
        _unit = unit;
        _tileOnBoard = tileOnBoard;
        _onCompleteJump = onCompleteJump;
        _onCompletelyDoneTask = onCompletelyDoneTask;
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();

        bool isCompleteTweenMove = false;
        _unit.MoveToNode(_tileOnBoard, onComplete: () =>
        {
            isCompleteTweenMove = true;
            //_tileOnBoard.UnOccupation(this._unit.PlayerUnit);
            //_tileOnBoard.EndStateMove();
            _onCompleteJump?.Invoke();
        });

        yield return new WaitUntil( ()=> isCompleteTweenMove);
        yield return new WaitForSeconds(_unit.DelayTimeMoveBetweenNode);
        _onCompletelyDoneTask?.Invoke();
    }
}
public class DoCallbackTask : ITaskSchedule
{
    private System.Action _callback;
    public DoCallbackTask(System.Action callback)
    {
        _callback = callback;
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        _callback?.Invoke();
    }
}

public class DoResetGameTask : ITaskSchedule
{
    public DoResetGameTask()
    {
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        UnitManager.Instance.Clear();
        GridManager.Instance.Clear();

        yield return new WaitForSeconds(0.5f);
        InGameManager.Instance.ChangeGameState(GameState.Init);
    }
}
public class DoAnimationFlipTilesTask : ITaskSchedule
{
    private System.Action _callback;
    private List<BaseTileOnBoard> _tilesToFlip;
    public DoAnimationFlipTilesTask(List<BaseTileOnBoard> tileOnBoards, System.Action callback)
    {
        _tilesToFlip = tileOnBoards;
        _callback = callback;
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        //Do your animation flip
        foreach (var tile in _tilesToFlip) {
            tile.Flip();
        }

        _callback?.Invoke();
    }
}

public class DoAttackTask : ITaskSchedule
{
    private System.Action<bool> _callback;
    private bool isWin;
    private int attackerResult = 0, defenderResult = 0;
    private Unit attacker;
    private List<Unit> defenders;

    public DoAttackTask(Unit attacker, List<Unit> defenders, System.Action<bool> callback, bool isWin, int attackerDiceResult, int defenderDiceResult )
    {
        _callback = callback;
        this.attacker = attacker;
        this.defenders = defenders;
        attackerResult = attackerDiceResult;
        defenderResult = defenderDiceResult;
        this.isWin = isWin;
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"Dice result --atk {attackerResult} --defend total {defenderResult}");
        yield return new WaitForSeconds(0.2f);

        if(isWin)
            attacker.MyCombat.Attacks(defenders, 1);

        _callback?.Invoke(isWin);
    }
}
public class DoAttacksTask : ITaskSchedule
{
    private System.Action<bool> _callback;
    private bool isWin;
    private int attackerResult = 0, defenderResult = 0;
    private List<Unit> attacker;
    private Unit defender;

    public DoAttacksTask(Unit defender, List<Unit> attackers, System.Action<bool> callback, bool isWin, int attackerDiceResult, int defenderDiceResult)
    {
        _callback = callback;
        this.attacker = attackers;
        this.defender = defender;
        attackerResult = attackerDiceResult;
        defenderResult = defenderDiceResult;
        this.isWin = isWin;
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        Debug.Log($"Dice result --atk total {attackerResult} --defend {defenderResult}");
        yield return new WaitForSeconds(0.2f);

        if (isWin && attacker.Count > 0)
        {
            Sequence seq = DOTween.Sequence().SetId(this);

            defender.MyCombat?.TakeDamage(attacker.Max(a=>a.MyCombat.AttackDamage) * attacker.Count);

            for (int i = 0; i < attacker.Count; i++)
            {
                var a = attacker[i];
                seq.Append(a.MyCombat.AnimationAttack(defender));
            }
        }

        _callback?.Invoke(isWin);
    }
}

public class DoMainPlayerKilledTask : ITaskSchedule
{
    private PlayerUnit _unit;
    public DoMainPlayerKilledTask(Unit unit)
    {
        _unit = unit as PlayerUnit;
    }
    public override IEnumerator DoTask()
    {
        yield return new WaitForEndOfFrame();
        Debug.LogError("Main Player Die, Game End");
        InGameManager.Instance.ChangeGameState(GameState.EndGame);
    }
}