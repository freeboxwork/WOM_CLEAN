using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class RewardManager : MonoBehaviour
{

    void Start()
    {
        SetRewardDic();
    }

    // 유니온 보상을 받을때 레벨이 오를때 마다 하나씩 넣고 리워드를 획득 할때마다 하나씩 빼서 리워드를 획득한다.
    public Queue<int> unionRewardQueue = new Queue<int>();
    Dictionary<EnumDefinition.RewardType, UnityAction<long>> rewardDic = new Dictionary<EnumDefinition.RewardType, UnityAction<long>>();





    void SetRewardDic()
    {
        rewardDic.Add(EnumDefinition.RewardType.gold, GlobalData.instance.player.AddGold);
        rewardDic.Add(EnumDefinition.RewardType.bone, GlobalData.instance.player.AddBone);
        rewardDic.Add(EnumDefinition.RewardType.dice, GlobalData.instance.player.AddDice);
        rewardDic.Add(EnumDefinition.RewardType.coal, GlobalData.instance.player.AddCoal);
        rewardDic.Add(EnumDefinition.RewardType.gem, GlobalData.instance.player.AddGem);
        rewardDic.Add(EnumDefinition.RewardType.clearTicket, GlobalData.instance.player.AddClearTicket);
        rewardDic.Add(EnumDefinition.RewardType.union, GlobalData.instance.unionManager.AddUnion);

        rewardDic.Add(EnumDefinition.RewardType.goldKey, AddDungeonKeyGold);
        rewardDic.Add(EnumDefinition.RewardType.boneKey, AddDungeonKeyBone);
        rewardDic.Add(EnumDefinition.RewardType.diceKey, AddDungeonKeyDice);
        rewardDic.Add(EnumDefinition.RewardType.coalKey, AddDungeonKeyCoal);

        rewardDic.Add(EnumDefinition.RewardType.unionTicket, GlobalData.instance.player.AddUnionTicket);
        rewardDic.Add(EnumDefinition.RewardType.dnaTicket, GlobalData.instance.player.AddDnaTicket);

        // 보상이 정해지지 않았을때 호출되는 함수
        rewardDic.Add(EnumDefinition.RewardType.dungeonPass, RewardNull);

    }

    // 보상이 정해지지 않았을때 호출되는 함수
    void RewardNull(long value)
    {
        Debug.Log("보상이 정해지지 않았습니다.");
    }

    void AddDungeonKeyGold(long value)
    {
        GlobalData.instance.player.AddDungeonKey(EnumDefinition.GoodsType.gold, value);
    }
    void AddDungeonKeyBone(long value)
    {
        GlobalData.instance.player.AddDungeonKey(EnumDefinition.GoodsType.bone, value);
    }
    void AddDungeonKeyDice(long value)
    {
        GlobalData.instance.player.AddDungeonKey(EnumDefinition.GoodsType.dice, value);
    }
    void AddDungeonKeyCoal(long value)
    {
        GlobalData.instance.player.AddDungeonKey(EnumDefinition.GoodsType.coal, value);
    }


    // unionRewardQueue 에 인자로 값을 받아서 넣는 함수
    public void AddUnionReward(int unionIndex)
    {
        // 유니온 획득 버튼 활성화
        UtilityMethod.SetBtnInteractableEnable(68, true);
        // 유니온 획득 버튼 이펙트 효과 활성화
        UtilityMethod.GetCustomTypeGMById(14).gameObject.SetActive(true);

        // save data 추가 
        GlobalData.instance.saveDataManager.SaveDataUnionAddRewardId(unionIndex);

        unionRewardQueue.Enqueue(unionIndex);

        // Enqueue 로그 출력
        Debug.Log($"유니온 {unionIndex} 보상 저장");
    }

    // unionRewardQueue 에서 값을 추출 하고 예외처리를 한다.
    public void UnionReward()
    {
        if (unionRewardQueue.Count == 0)
        {
            // 팝업
            Debug.Log("획득할 유니온이 없습니다.");
            GlobalData.instance.globalPopupController.EnableGlobalPopup("유니온 획득", "획득할 유니온이 없습니다.");
            return;
        }

        // 팝업
        int unionIndex = unionRewardQueue.Dequeue();

        // save data
        GlobalData.instance.saveDataManager.SaveDataRemoveUnionRewardId(unionIndex);
        PopupController.instance.InitPopup(EnumDefinition.RewardType.union, unionIndex);

        if (unionRewardQueue.Count <= 0)
        {
            // 유니온 획득 버튼 비활성화
            UtilityMethod.SetBtnInteractableEnable(68, false);
            // 유니온 획득 버튼 이펙트 효과 비활성화
            UtilityMethod.GetCustomTypeGMById(14).gameObject.SetActive(false);
        }
    }


    void SetunionRewardQueue()
    {
        var datas = GlobalData.instance.dataManager.summonGradeDatas.data;
        for (int i = 0; i < datas.Count; i++)
        {
            if (datas[i].level != 0)
            {
                unionRewardQueue.Enqueue(datas[i].rewardUnionIndex);
            }
        }
    }

    public void RewardByType(EnumDefinition.RewardType rewardType, long rewardValye)
    {
        rewardDic[rewardType].Invoke(rewardValye);

        //TODO: 획득 연출 추가

        // 획득 이벤트 타입과 획득량을 로그로 출력함
        //Debug.Log($"획득 이벤트 타입 : {rewardType}, 획득량 : {rewardValye}");
    }


    public void RewardUnion(int unionIndex)
    {
        //TODO: 획득 연출 추가 ( 팝업 )

        // 획득
        GlobalData.instance.unionManager.AddUnion(unionIndex);
        // 획득한 유니온을 로그로 출력함
        //Debug.Log($"획득한 유니온 번호 : {unionIndex}");
    }



}
