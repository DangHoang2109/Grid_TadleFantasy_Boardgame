using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingButton : MonoBehaviour
{
    public void ClickTesting()
    {
        InGameManager.Instance.ChangeGameState(GameState.Enemy_Turn);
    }
}
