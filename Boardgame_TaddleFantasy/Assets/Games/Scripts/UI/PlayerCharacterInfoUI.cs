using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerCharacterInfoUI : MonoBehaviour
{
    [SerializeField] Image _imgCharAvatar;
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
            _tmpAP.text = $"AP: {charStat.CurrentAP}";

            PlayerProperty.onAPChange -= OnChangeAPPts;
            PlayerProperty.onAPChange += OnChangeAPPts;

            _progressHPPts.UpdateFillAndProgress(charStat.CurrentHP, charStat.MaxHP, charStat.HPString);
        }
    }
    void OnChangeAPPts(int change, int currentValue)
    {
        FloatBubbleManager.Instance.SpawnBubble_HelperValueColor(change, _tmpAP.transform.position);
        _tmpAP.text = $"AP: {currentValue}";
    }
}
