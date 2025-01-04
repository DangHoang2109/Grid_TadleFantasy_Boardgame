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

    private PlayerProperty PlayerProperty => UnitManager.Instance.MainPlayer.MyProperty;

    private void OnEnable()
    {
        GameStartState.OnEnterState -= OnGameEnterStart;
        GameStartState.OnEnterState += OnGameEnterStart;

        GameResetState.OnEnterState -= OnGameEnterReset;
        GameResetState.OnEnterState += OnGameEnterReset;
    }

    void OnGameEnterStart()
    {
        OnGameStartComplete();
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
            charStat.onHPChange -= OnChangeHPPts;
            charStat.onHPChange += OnChangeHPPts;
        }
    }
    void OnChangeAPPts(int change, int currentValue)
    {
        FloatBubbleManager.Instance.SpawnBubble_HelperValueColor(change, _tmpAP.transform.position);
        _tmpAP.text = $"AP: {currentValue}";
    }
    void OnChangeHPPts(int change, int currentValue, int maxValue)
    {
        _progressHPPts.UpdateFillAndProgress(currentValue, maxValue, this.PlayerProperty.HPString);
    }

    void OnGameEnterReset()
    {
        OnGameResetComplete();
    }
    void OnGameResetComplete()
    {
        _tmpCharacterName.text = "--";
        _tmpAP.text = $"AP: 0";
        _progressHPPts.UpdateFillAndProgress(0, 50, "--");
    }
}
