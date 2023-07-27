using UnityEngine.UI;
using TMPro;
using UnityEngine;

public class CampPopup : CastlePopupBase
{

    public Image imgSummonCountProgress;
    public TextMeshProUGUI txtSummonCount;
    public TextMeshProUGUI txtGradeLevel;

    public Button btnGetReward;

    public Button btnClose;

    public ProjectGraphics.LotteryAnimationController lotteryAnim;

    void Start()
    {
        SetBtnEvent();
        lotteryAnim = FindObjectOfType<ProjectGraphics.LotteryAnimationController>();
    }

    public void SetSummonCountProgress(int curValue, int totalValue)
    {
        var value = (float)curValue / (float)totalValue;

        Debug.Log($"value : {value}" + $"curValue : {curValue}" + $"totalValue : {totalValue}");

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

    public void OnCheckSkipUnion(UnityEngine.UI.Toggle isOn)
    {
        lotteryAnim.OnClickSkipUnion(isOn.isOn);
    }

    public void OnCheckSkipDNA(UnityEngine.UI.Toggle isOn)
    {
        lotteryAnim.OnClickSkipDNA(isOn.isOn);
    }

}
