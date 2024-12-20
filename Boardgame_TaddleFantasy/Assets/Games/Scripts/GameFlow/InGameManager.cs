using System;
using System.Collections;
using System.Collections.Generic;
using Taddle_Fantasy;
using UnityEngine;

public class InGameManager : MonoBehaviour
{
    public static InGameManager Instance;
    public int currentPlayerIndex;
    //move to Unit Manager
    [SerializeField] List<PlayerUnit> players;
    public PlayerUnit CurrentPlayerTurn => players[currentPlayerIndex];
    void Awake() => Instance = this;

    [SerializeField] private GameState _state;
    public GameState CurrentGameState => _state;

    [SerializeField] private TurnState _turnState;
    public TurnState CurrentTurnState => _turnState;
    protected ITurnState currentTurnState;

    public static System.Action<GameState> OnGameStateChanged;
    //State, Player Index who taking turn
    public static System.Action<TurnState, int> OnTurnStateChanged;


    public void ChangeGameState(GameState state)
    {
        _state = state;
        OnGameStateChanged?.Invoke(state);

        ///Change to the IGameState system like TurnState
        switch (state)
        {
            case GameState.None:
                break;
            case GameState.Init:
                GridManager.Instance.InitBoard();
                players = new List<PlayerUnit>( FindObjectsOfType<PlayerUnit>());
                ChangeGameState(GameState.Start);
                break;
            case GameState.Start:
                ChangeGameState(GameState.Player_Turn);
                break;
            case GameState.Enemy_Turn:
                break;
            case GameState.Player_Turn:
                ChangeTurnState(TurnState.Main_Phase);
                break;
            case GameState.EndGame:
                break;
            default:
                break;
        }
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
    Init = 0,
    //Begin the game
    Start = 1,

    //Begin turn of Enemy
    Enemy_Turn = 2,
    //Begin turn of main player and other player, check the Turn Index
    Player_Turn = 3,

    EndGame = 4,
}
public enum TurnState
{
    //Player: Add Action Pts, Enemy: Spawn new Enemy
    StandBy_Phase = 0,
    //Play their turn: Planning Move (Chose or Auto), Purchase skill, Purchase Weapon,...
    [Type(typeof(PlayerMainPhaseTurnState))]
    Main_Phase = 1,
    //Moving after planning
    [Type(typeof(PlayerMovingTurnState))]
    Moving_Phase = 2,
    //Battle after moving
    Battle_Phase = 3,
    [Type(typeof(PlayerEndTurnState))]
    End_Turn = 4,
}