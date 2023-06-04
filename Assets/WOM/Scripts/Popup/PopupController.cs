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
    private List<RewardInfoData> rewardInfoList;
    private string popupTitleName;
    private EEfectGameObjectTYPE effectType;
    private Action callBack;

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
        this.callBack = cb;
    }
    //TitleName, RewardData, CallbackAction, EffectGameObject
     public void SetPopupData(string title , List<RewardInfoData> list, Action cb, EEfectGameObjectTYPE type)
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

    [Header("Union Sprite SO")]
    [SerializeField] SpriteFileData spriteFileData; //GetIconData()
    PopupRewardInfoData popupRewardInfoData = null;
    List<Action> callbacks = new List<Action>();
    public GameObject rewardEffect;
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
        popupBuilder.SetButton("GET", callbacks);
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
                    //보상 rewards[i].amount Data/UI text Update
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

        //실제 보상받을 데이터 세팅 RewardType으로 각종 재화+유니온+DNA 및 앞으로 추가 될 재화도 고려해야 함
        //매개변수로 1.보상타입 2.보상량 3.보상아이콘이 세팅되며 보상아이콘은 스크립터블 오브젝트에서 불러올 예정
        rewards.Add(new RewardInfoData(EnumDefinition.RewardType.gold, 10000, null));
        rewards.Add(new RewardInfoData(EnumDefinition.RewardType.gem, 20000, null));
 
        //오버로딩 되어 있음. 매개변수로 CallBack 이벤트와 연출 이펙트로 사용 될 게임오브젝트 타입을 넘길 수 있음(연출 타입에 따른 게임오브젝트 로드가 필요) 
        //*보상은 따로 콜백구현이 이미 되어있음* line115
        data.SetPopupData("REWARD", rewards, CallBackTest);

        SetupPopupInfo(data);

    }

    void CallBackTest()
    {
        Debug.Log("CALL");
    }



}
