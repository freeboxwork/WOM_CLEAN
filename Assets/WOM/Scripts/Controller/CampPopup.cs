using UnityEngine.UI;
using TMPro;

public class CampPopup : CastlePopupBase
{

    public Image imgSummonCountProgress;
    public TextMeshProUGUI txtSummonCount;
    public TextMeshProUGUI txtGradeLevel;

    public Button btnGetReward;



    void Start()
    {
        SetBtnEvent();
    }

    public void SetSummonCountProgress(int totalValue, int curValue)
    {
        imgSummonCountProgress.fillAmount = (float)curValue / (float)totalValue;
        
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
    }

}
