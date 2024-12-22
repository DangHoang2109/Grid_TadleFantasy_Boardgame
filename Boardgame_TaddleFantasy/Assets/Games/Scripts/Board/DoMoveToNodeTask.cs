using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoMoveToNodeTask : ITaskSchedule
{
    private PlayerMovement _unit;
    private BaseTileOnBoard _tileOnBoard;
    private System.Action _onCompleteJump, _onCompletelyDoneTask;
    public DoMoveToNodeTask(PlayerMovement unit, BaseTileOnBoard tileOnBoard,
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
        yield return new WaitForSeconds(PlayerMovement.DELAY_TIME_MOVE_BETWEEN_NODE);
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
