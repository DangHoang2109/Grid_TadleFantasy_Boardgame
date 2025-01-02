using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;

public class PlayerCharacterInfoUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI _tmpCharacterName;
    [SerializeField] TextMeshProUGUI _tmpAP;
    [SerializeField] ProgressSliderUI _progressHPPts;


    private void OnEnable()
    {
        InGameManager.OnGameStateChanged -= OnGameStateChange;
        InGameManager.OnGameStateChanged += OnGameStateChange;
    }

    void OnGameStateChange(GameState state)
    {
        if(state == GameState.Start)
        {
            OnGameStartComplete();
            InGameManager.OnGameStateChanged -= OnGameStateChange;
        }
    }
    void OnGameStartComplete()
    {
        //get main user character info
        var mainPlayer = UnitManager.Instance.MainPlayer;
        if (mainPlayer != null) {
            var charConfig = mainPlayer.MyCharacterConfig;
            _tmpCharacterName.text = charConfig.Name();

            var charStat = mainPlayer.MyProperty;
            _tmpAP.text = charStat.CurrentAP.ToString("##");

            PlayerProperty.onAPChange -= OnChangeAPPts;
            PlayerProperty.onAPChange += OnChangeAPPts;

            _progressHPPts.UpdateFillAndProgress()
        }
    }
    void OnChangeAPPts(int change, int currentValue)
    {
        _tmpAP.text = currentValue.ToString("##");
    }
}
