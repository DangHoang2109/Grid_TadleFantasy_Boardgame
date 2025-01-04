using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingButton : MonoBehaviour
{
    public void ClickTesting()
    {
        UnitManager.Instance.MainPlayer.MyCombat.TakeDamage(100);
    }
    public void ClickTest2()
    {
        InGameManager.Instance.ChangeGameState(GameState.Init);
    }
}
