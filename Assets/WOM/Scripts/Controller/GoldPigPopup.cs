using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GoldPigPopup : MonoBehaviour
{
    public Button btnCommonReward;
    public Button btnADReward;

    public int normalRewardValue = 50;
    public int adRewardValue = 500;

    public TextMeshProUGUI txtRewardAD_Value;
    public TextMeshProUGUI txtRewardNormal_Value;
    public EnumDefinition.RewardTypeAD adRewardType;


    void Start()
    {
        SetBtnEvents();
    }

    void OnEnable()
    {
        UpdateUI();
    }


    public void SetBtnEvents()
    {
        btnCommonReward.onClick.AddListener(() => OnClickNormalReward());
        btnADReward.onClick.AddListener(() => OnClickADReward());
    }


    void OnClickNormalReward()
    {
        var rewardValue = GetRewardValue(normalRewardValue);
        PopupController.instance.InitPopup(EnumDefinition.RewardType.gold, rewardValue);
        GlobalData.instance.goldPigController.EnableGoldPig();
        ClosePopup();
    }

    void OnClickADReward()
    {
        Admob.instance.ShowRewardedAdByType(adRewardType);
    }

    public void RewardAD()
    {
        var rewardValue = GetRewardValue(adRewardValue);
        PopupController.instance.InitPopup(EnumDefinition.RewardType.gold, rewardValue);
        GlobalData.instance.goldPigController.EnableGoldPig();
        ClosePopup();
    }

    void UpdateUI()
    {
        txtRewardAD_Value.text = GetRewardValue(adRewardValue).ToString();
        txtRewardNormal_Value.text = GetRewardValue(normalRewardValue).ToString();
    }

    void ClosePopup()
    {
        gameObject.SetActive(false);
    }

    int GetRewardValue(int rewardValue)
    {
        var normalMonsterData = GlobalData.instance.monsterManager.GetMonsterData(EnumDefinition.MonsterType.normal);
        return normalMonsterData.gold * rewardValue;
    }


}
