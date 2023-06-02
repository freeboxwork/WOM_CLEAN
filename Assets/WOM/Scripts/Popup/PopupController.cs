using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;




public class RewardInfoData
{
    public float amount;
    public EnumDefinition.RewardType type;

    public Sprite icon;

    public RewardInfoData(EnumDefinition.RewardType type, float amount,Sprite sp)
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
    //∫∏ªÛ ¿Á»≠ ¡§∫∏
    private List<RewardInfoData> rewardInfoList;
    //∫∏ªÛ ∆Àæ˜ ¿Ã∏ß
    private string popupTitleName;
    //ø¨√‚ ¿Ã∆Â∆Æ ≈∏¿‘
    private EEfectGameObjectTYPE effectType;
    //ƒ›πÈ æ◊º«
    private Action callBack;

    public void SetPopupData(string title)
    {
        this.popupTitleName = title;
    }
    public void SetPopupData(string title, List<RewardInfoData> list)
    {
        this.popupTitleName = title;
        this.rewardInfoList = list;
    }
    public void SetPopupData(string title, List<RewardInfoData> list, Action cb)
    {
        this.popupTitleName = title;
        this.rewardInfoList = list;
        this.callBack = cb;
    }
    public void SetPopupData(string title , List<RewardInfoData> list, Action cb,EEfectGameObjectTYPE type)
    {
        this.popupTitleName = title;
        this.rewardInfoList = list;
        this.callBack = cb;
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
    public Action GetCallBack()
    {
        return callBack;
    }
    

}

public class PopupController : MonoBehaviour
{
    public static PopupController instance;

    [Header("¿Ø¥œø¬ Sprite SO")]
    [SerializeField] SpriteFileData spriteFileData; //GetIconData()

    //Î¶¨Ïõå?ìú ?†ïÎ≥¥Í?? ?ã¥Í∏? ?Å¥?ûò?ä§
    PopupRewardInfoData popupRewardInfoData = null;
    //ÏΩúÎ∞± ?ù¥Î≤§Ìä∏ ?ï°?Öò
    List<Action> callbacks = new List<Action>();
    [Header("¿Ã∆Â∆Æ ø¨√‚ ∞‘¿”ø¿∫Í¡ß∆Æ")]
    public GameObject rewardEffect;
    [Header("∆Àæ˜¿« ∫Œ∏ ø¿∫Í¡ß∆Æ")]
    public Transform popupParent;

    private void Awake()
    {
        instance = this;
    }


    public void SetupPopupInfo(PopupRewardInfoData data)
    {
        popupRewardInfoData = data;

        callbacks.Clear();

        var cb = popupRewardInfoData.GetCallBack();

        //rewardEffect.SetActive(true);

        if (cb != null)
        {
            callbacks.Add(cb);
        }

        callbacks.Add(Reward);

        PopupBuilder popupBuilder = new PopupBuilder(popupParent);
        popupBuilder.SetTitle(popupRewardInfoData.GetTitle());
        //popupBuilder.SetButton(I2.Loc.LocalizationManager.GetTranslation("?É≠?ïò?ó¨?ã´Í∏? Î≤ÑÌäº") , callbacks);
        popupBuilder.SetButton("?ã´Í∏?", callbacks);
        var rewards = popupRewardInfoData.GetRewardInfoDataList();

        for (int i = 0; i < rewards.Count; i++)
        {
            popupBuilder.SetRewardInfo(rewards[i].type, rewards[i].amount);

        }

        popupBuilder.Build();

    }

    void Reward()
    {
        //Insert Audio Effect Play Code 

        var rewards = popupRewardInfoData.GetRewardInfoDataList();

        for (int i = 0; i < rewards.Count; i++)
        {
            switch(rewards[i].type)
            {
                case EnumDefinition.RewardType.gold:

                    break;

                case EnumDefinition.RewardType.bone:

                    break;

                case EnumDefinition.RewardType.gem:

                    break;

                case EnumDefinition.RewardType.dice:

                    break;

                case EnumDefinition.RewardType.coal:
                    break;

                case EnumDefinition.RewardType.clearTicket:
                    break;

                case EnumDefinition.RewardType.goldKey:
                    break;

                case EnumDefinition.RewardType.boneKey:
                    break;

                case EnumDefinition.RewardType.diceKey:
                    break;

                case EnumDefinition.RewardType.coalKey:
                    break;

                case EnumDefinition.RewardType.union:
                    break;

                case EnumDefinition.RewardType.dna:
                    break;
                case EnumDefinition.RewardType.none:
                    break;
                    
            } 
        }

        //rewardEffect.SetActive(false);


        //Player Data Save
    }


    [Sirenix.OdinInspector.Button]
    public void TestPopup()
    {
        PopupRewardInfoData data = new PopupRewardInfoData();
        List<RewardInfoData> rewards = new List<RewardInfoData>();

        rewards.Add(new RewardInfoData(EnumDefinition.RewardType.gold, 10000, null));
        rewards.Add(new RewardInfoData(EnumDefinition.RewardType.gem, 20000, null));
 

        data.SetPopupData("REWARD",rewards);

        SetupPopupInfo(data);




    }




}
