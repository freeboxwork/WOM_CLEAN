using UnityEngine.UI;
using TMPro;
using UnityEngine;
using ProjectGraphics;

public class CampPopup : CastlePopupBase
{

    public Image imgSummonCountProgress;
    public TextMeshProUGUI txtSummonCount;
    public TextMeshProUGUI txtGradeLevel;

    public Button btnGetReward;

    public Button btnClose;
    public Toggle[] togglesUnion;
    public Toggle[] togglesDNA;

    public Toggle toggleUnionEffSkip;
    public Toggle toggleUnionRepeatGame;

    public Toggle toggleDnaEffSkip;
    public Toggle toggleDnaRepeatGame;

    public LotteryAnimationController lotteryAnimationController;

    void Start()
    {
        SetBtnEvent();
        SetToggleEvent();
        foreach (var to in togglesDNA) to.isOn = false;
        foreach (var to in togglesUnion) to.isOn = false;
    }

    void SetToggleEvent()
    {
        // Union
        toggleUnionEffSkip.onValueChanged.AddListener((isOn) =>
        {
            lotteryAnimationController.toggleEffSkip.isOn = isOn;
        });
        toggleUnionRepeatGame.onValueChanged.AddListener((isOn) =>
        {
            lotteryAnimationController.toggleRepeatGame.isOn = isOn;
        });

        // DNA
        toggleDnaEffSkip.onValueChanged.AddListener((isOn) =>
        {
            lotteryAnimationController.toggleEffSkip.isOn = isOn;
        });
        toggleDnaRepeatGame.onValueChanged.AddListener((isOn) =>
        {
            lotteryAnimationController.toggleRepeatGame.isOn = isOn;
        });

    }

    public void ToggleReset()
    {
        toggleUnionEffSkip.isOn = false;
        toggleUnionRepeatGame.isOn = false;
        toggleDnaEffSkip.isOn = false;
        toggleDnaRepeatGame.isOn = false;
    }

    public void SetSummonCountProgress(int curValue, int totalValue)
    {
        var value = (float)curValue / (float)totalValue;

//        Debug.Log($"value : {value}" + $"curValue : {curValue}" + $"totalValue : {totalValue}");

        imgSummonCountProgress.fillAmount = value;

    }

    public void SetTxtSummonCount(int curValue, int totalValue)
    {
        txtSummonCount.text = $"{curValue}/{totalValue}";
    }

    public void SetTxtGradeLevel(int level)
    {
        txtGradeLevel.text = $"{level}";
    }

    void SetBtnEvent()
    {
        btnGetReward.onClick.AddListener(() =>
        {
            GlobalData.instance.rewardManager.UnionReward();

        });

        btnClose.onClick.AddListener(() =>
        {
            gameObject.SetActive(false);
        });
    }
}
