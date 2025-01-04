using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LoseGamePopup : BasePopup
{
    public static LoseGamePopup Show() => PopupManager.Instance.ShowPopup<LoseGamePopup>("Popup/LoseGamePopup");
    public void ClickReplay()
    {
        InGameManager.Instance.ResetTheGame();
        ClickClose();
    }
}
