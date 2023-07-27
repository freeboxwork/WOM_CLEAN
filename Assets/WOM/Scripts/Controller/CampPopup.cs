using UnityEngine.UI;
using TMPro;
using UnityEngine;
using Sirenix.Utilities;

public class CampPopup : CastlePopupBase
{

    public Image imgSummonCountProgress;
    public TextMeshProUGUI txtSummonCount;
    public TextMeshProUGUI txtGradeLevel;

    public Button btnGetReward;

    public Button btnClose;
    public Toggle[] togglesUnion;
    public Toggle[] togglesDNA;

    void Start()
    {
        SetBtnEvent();
        foreach(var to in togglesDNA) to.isOn = false; 
        foreach(var to in togglesUnion) to.isOn = false;
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
}
