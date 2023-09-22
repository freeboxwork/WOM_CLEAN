using System.Collections.Generic;
using UnityEngine;
using System;




public class RewardInfoData
{
    public float amount;
    public EnumDefinition.RewardType type;

    public Sprite icon;

    public RewardInfoData(EnumDefinition.RewardType type, float amount, Sprite sp)
    {
        this.type = type;
        this.amount = amount;
        this.icon = sp;
    }
}

public enum EEfectGameObjectTYPE
{
    None,
    BossReward,
    Max
}


public class PopupRewardInfoData
{
    private List<RewardInfoData> rewardInfoList;
    private string popupTitleName;
    private EEfectGameObjectTYPE effectType;
    private List<Action> callBackList = new List<Action>();

    //TitleName, RewardData
    public void SetPopupData(string title, List<RewardInfoData> list)
    {
        this.popupTitleName = title;
        this.rewardInfoList = list;
    }
    //TitleName, RewardData, CallbackAction
    public void SetPopupData(string title, List<RewardInfoData> list, Action cb)
    {
        this.popupTitleName = title;
        this.rewardInfoList = list;
        SetCallBackAction(cb);
    }
    //TitleName, RewardData, CallbackAction, EffectGameObject
    public void SetPopupData(string title, List<RewardInfoData> list, Action cb, EEfectGameObjectTYPE type)
    {
        this.popupTitleName = title;
        this.rewardInfoList = list;
        SetCallBackAction(cb);
        this.effectType = type;
    }

    //GET//
    public string GetTitle()
    {
        return popupTitleName;
    }
    public List<RewardInfoData> GetRewardInfoDataList()
    {
        return rewardInfoList;
    }
    public EEfectGameObjectTYPE GetEffectType()
    {
        return effectType;
    }
    public void ClearActionList()
    {
        callBackList.Clear();
    }

    public void SetCallBackAction(Action ac)
    {
        callBackList.Add(ac);
    }
    public List<Action> GetCallBackList()
    {
        return callBackList;
    }

}

public class PopupController : MonoBehaviour
{
    public static PopupController instance;

    [Header("Union Sprite SO")]
    [SerializeField] SpriteFileData spriteFileData; //GetIconData()
    PopupRewardInfoData popupRewardInfoData = null;
    public Transform popupParent;
    public PoolManager poolManager;
    public RewardManager rewardManager;


    private void Awake()
    {
        instance = this;
    }

    public void SetupPopupInfo(PopupRewardInfoData data)
    {
        popupRewardInfoData = data;

        popupRewardInfoData.ClearActionList();

        //rewardEffect.SetActive(true);

        popupRewardInfoData.SetCallBackAction(Reward);

        PopupBuilder popupBuilder = new PopupBuilder(popupParent);
        popupBuilder.SetTitle(popupRewardInfoData.GetTitle());
        popupBuilder.SetButton("GET", popupRewardInfoData.GetCallBackList());
        var rewards = popupRewardInfoData.GetRewardInfoDataList();

        for (int i = 0; i < rewards.Count; i++)
        {
            popupBuilder.SetRewardInfo(rewards[i].type, rewards[i].amount, rewards[i].icon);
        }

        popupBuilder.Build(poolManager);

    }

    void Reward()
    {
        //Insert Audio Effect Play Code 
        var rewards = popupRewardInfoData.GetRewardInfoDataList();

        for (int i = 0; i < rewards.Count; i++)
        {
            GlobalData.instance.rewardManager.RewardByType(rewards[i].type, rewards[i].amount);
        }
        //Player Data Save
    }

    public void InitPopup(EnumDefinition.RewardType rewardType, float amount)
    {
        PopupRewardInfoData data = new PopupRewardInfoData();
        List<RewardInfoData> rewards = new List<RewardInfoData>();


        //var sprite = GlobalData.instance.spriteDataManager.GetRewardIcon(rewardType);
        var sprite = GetRewardIcon(rewardType, amount);
        rewards.Add(new RewardInfoData(rewardType, amount, sprite));

        data.SetPopupData("REWARD", rewards);

        SetupPopupInfo(data);
    }

    public void InitPopups(EnumDefinition.RewardType[] rewardTypes, int[] amounts)
    {
        PopupRewardInfoData data = new PopupRewardInfoData();
        List<RewardInfoData> rewards = new List<RewardInfoData>();

        for (int i = 0; i < rewardTypes.Length; i++)
        {
            //var sprite = GlobalData.instance.spriteDataManager.GetRewardIcon(rewardTypes[i]);
            var sprite = GetRewardIcon(rewardTypes[i], amounts[i]);
            rewards.Add(new RewardInfoData(rewardTypes[i], amounts[i], sprite));
        }
        data.SetPopupData("REWARD", rewards);

        SetupPopupInfo(data);
    }


    public void InitPopup(EnumDefinition.RewardType rewardType, int amount)
    {
        PopupRewardInfoData data = new PopupRewardInfoData();
        List<RewardInfoData> rewards = new List<RewardInfoData>();
        //var sprite = GlobalData.instance.spriteDataManager.GetRewardIcon(rewardType);
        var sprite = GetRewardIcon(rewardType, amount);
        rewards.Add(new RewardInfoData(rewardType, amount, sprite));

        data.SetPopupData("REWARD", rewards);

        SetupPopupInfo(data);
    }

    public void InitPopups(EnumDefinition.RewardType[] rewardTypes, float[] amounts)
    {
        PopupRewardInfoData data = new PopupRewardInfoData();
        List<RewardInfoData> rewards = new List<RewardInfoData>();

        for (int i = 0; i < rewardTypes.Length; i++)
        {
            //var sprite = GlobalData.instance.spriteDataManager.GetRewardIcon(rewardTypes[i]);
            var sprite = GetRewardIcon(rewardTypes[i], amounts[i]);
            rewards.Add(new RewardInfoData(rewardTypes[i], amounts[i], sprite));
        }
        data.SetPopupData("REWARD", rewards);

        SetupPopupInfo(data);
    }


    Sprite GetRewardIcon(EnumDefinition.RewardType rewardType, float value)
    {
        if (rewardType == EnumDefinition.RewardType.union)
        {
            return spriteFileData.GetIconData(value);
        }
        else
        {
            return GlobalData.instance.spriteDataManager.GetRewardIcon(rewardType);
        }
    }


}
