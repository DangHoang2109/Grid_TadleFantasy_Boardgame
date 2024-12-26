using System;
using System.Collections;
using System.Collections.Generic;
using Taddle_Fantasy;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    void Awake() => Instance = this;

    public int currentPlayerIndex;
    //move to Unit Manager
    public PlayerUnit CurrentPlayerTurn => UnitManager.Instance.GetPlayerByTurnIndex(currentPlayerIndex);

    [SerializeField] private GameState _state;
    public GameState CurrentGameState => _state;
    protected IGameState currentGameState;

    [SerializeField] private TurnState _turnState;
    public TurnState CurrentTurnState => _turnState;
    protected ITurnState currentTurnState;

    public static System.Action<GameState> OnGameStateChanged;
    //State, Player Index who taking turn
    public static System.Action<TurnState, int> OnTurnStateChanged;


    public void ChangeGameState(GameState state)
    {
        if (state != GameState.Player_Turn && _state == state)
            return;
        _state = state;
            
        if (currentGameState != null)
            currentGameState.Exit();
        currentGameState = Activator.CreateInstance(EnumUtility.GetStringType(state)) as IGameState;
        currentGameState.Init();
        currentGameState.Enter();

        OnGameStateChanged?.Invoke(state);
    }
    public void ChangeTurnState(TurnState state)
    {
        if (_turnState == state)
            return;

        _turnState = state;

        if(currentTurnState != null)
            currentTurnState.Exit();
        currentTurnState = Activator.CreateInstance(EnumUtility.GetStringType(state)) as ITurnState;
        currentTurnState.Init(CurrentPlayerTurn, this.currentPlayerIndex);
        currentTurnState.Enter();

        OnTurnStateChanged?.Invoke(_turnState, currentPlayerIndex);
    }
    public void ExistState(TurnState state)
    {
        if (_turnState != state)
            return;

        if (currentTurnState != null)
            currentTurnState.Exit();
    }

    public void FinishTurnState(TurnState state)
    {
        if (this.CurrentTurnState != state)
        {
            Debug.Log($"Want to finish unmatch state {CurrentTurnState} -- {state}");
            return;
        }
    }

    #region Test
    private void Start()
    {
        ChangeGameState(GameState.Init);
    }
    #endregion
}
public enum GameState
{
    None = -1,
    //Prepare the game: Board, Unit,...
    [Type(typeof(GameInitState))]
    Init = 0,
    //Begin the game
    [Type(typeof(GameStartState))]
    Start = 1,

    [Type(typeof(GameEnemyTurnState))]
    //Begin turn of Enemy
    Enemy_Turn = 2,
    //Begin turn of main player and other player, check the Turn Index
    [Type(typeof(GamePlayerTurnState))]
    Player_Turn = 3,

    EndGame = 4,
}
public enum TurnState
{
    //Player: Add Action Pts, Enemy: Spawn new Enemy
    Player_StandBy_Phase = 0,
    [Type(typeof(EnemyInviteTurnState))]
    Enemy_Invite_Phase,

    //Play their turn: Planning Move (Chose or Auto), Purchase skill, Purchase Weapon,...
    [Type(typeof(PlayerMainPhaseTurnState))]
    Main_Phase,
    //Moving after planning
    [Type(typeof(PlayerMovingTurnState))]
    Moving_Phase,
    //Battle after moving
    Battle_Phase,
    [Type(typeof(PlayerEndTurnState))]
    End_Turn,
}