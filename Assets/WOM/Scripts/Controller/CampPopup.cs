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
    public Toggle toggleUnionEffSkip;
    public Toggle toggleUnionRepeatGame;
    public Toggle toggleDnaRepeatGame;

    bool isToggleEffSkipUnion;
    bool isToggleRepeatUnion;
    bool isToggleRepeatDNA;
    public LotteryAnimationController lotteryAnimationController;

    protected override void Awake() {
        base.Awake();
    }
    void Start()
    {
        SetBtnEvent();
        ToggleReset();
    }

    public override void ShowPopup()
    {
        base.ShowPopup();
    }
    public override void HidePopup()
    {
        base.HidePopup();
    }
    public bool GetIsOnToggleSkipUnionIsOn()
    {
        return toggleUnionEffSkip.isOn;
    }
    public bool GetIsOnToggleRepeatUnion()
    {
        return toggleUnionRepeatGame.isOn;
    }
    public bool GetIsOnToggleRepeatDNA()
    {
        return toggleDnaRepeatGame.isOn;
    }

    public void ToggleReset()
    {
        toggleUnionEffSkip.isOn = false;
        toggleUnionRepeatGame.isOn = false;
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
            HidePopup();
        });
    }
}
