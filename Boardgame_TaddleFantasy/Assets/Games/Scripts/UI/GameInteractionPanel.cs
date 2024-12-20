using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

/// <summary>
/// Right Panel
/// </summary>
public class GameInteractionPanel : MonoBehaviour
{
    public Transform mainTransform;
    [Header("Move")]
    [SerializeField] TextMeshProUGUI txtMoveLeft;
    [SerializeField] Button btnMove;


    private void Start()
    {
        PlayerMainPhaseTurnState.RegisterEnterStateCallback(EnterMainPhase);
        PlayerMainPhaseTurnState.RegisterExitStateCallback(ExitMainPhase);

        PlayerMovement.OnMovementPlan += OnMovementPlanned;
    }

    void EnterMainPhase()
    {
        Debug.Log("GameInteractionPanel EnterMainPhase");
        mainTransform.gameObject.SetActive(true);

        this.txtMoveLeft.text = InGameManager.Instance.CurrentPlayerTurn.MyMovement.StringMovementStatus();

    }
    void ExitMainPhase()
    {
        Debug.Log("GameInteractionPanel ExitMainPhase");
        mainTransform.gameObject.SetActive(false);
    }
    public void OnClickMove()
    {
        InGameManager.Instance.ChangeTurnState(TurnState.Moving_Phase);
    }

    void OnMovementPlanned(string strMoveLeft)
    {
        this.txtMoveLeft.text = strMoveLeft;
    }
}
