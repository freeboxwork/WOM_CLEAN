using UnityEngine;
using UnityEngine.UI;

public class GoldPigPopup : MonoBehaviour
{
    public Button btnCommonReward;
    public Button btnADReward;

    public int commonRewardValue = 50;
    public int adRewardValue = 500;


    void Start()
    {
        SetBtnEvents();
    }


    public void SetBtnEvents()
    {
        btnCommonReward.onClick.AddListener(() => OnClickCommonReward());
        btnADReward.onClick.AddListener(() => OnClickADReward());
    }


    void OnClickCommonReward()
    {
        var rewardValue = GetRewardValue(commonRewardValue);
        PopupController.instance.InitPopup(EnumDefinition.RewardType.gold, rewardValue);
        GlobalData.instance.goldPigController.EnableGoldPig();
        ClosePopup();
    }

    void OnClickADReward()
    {
        var rewardValue = GetRewardValue(adRewardValue);
        PopupController.instance.InitPopup(EnumDefinition.RewardType.gold, rewardValue);
        GlobalData.instance.goldPigController.EnableGoldPig();
        ClosePopup();
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
