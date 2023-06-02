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
    Dictionary<EnumDefinition.RewardType, UnityAction<int>> rewardDic = new Dictionary<EnumDefinition.RewardType, UnityAction<int>>();




    void SetRewardDic()
    {
        rewardDic.Add(EnumDefinition.RewardType.gold, GlobalData.instance.player.AddGold);
        rewardDic.Add(EnumDefinition.RewardType.bone, GlobalData.instance.player.AddBone);
        rewardDic.Add(EnumDefinition.RewardType.dice, GlobalData.instance.player.AddDice);
        rewardDic.Add(EnumDefinition.RewardType.coal, GlobalData.instance.player.AddCoal);
        rewardDic.Add(EnumDefinition.RewardType.gem, GlobalData.instance.player.AddGem);
        rewardDic.Add(EnumDefinition.RewardType.clearTicket, GlobalData.instance.player.AddClearTicket);
    }



    // unionRewardQueue 에 인자로 값을 받아서 넣는 함수
    public void AddUnionReward(int unionIndex)
    {
        // 유니온 획득 버튼 활성화
        UtilityMethod.SetBtnInteractableEnable(68, true);
        unionRewardQueue.Enqueue(unionIndex);
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
        RewardUnion(unionIndex);

        GlobalData.instance.globalPopupController.EnableGlobalPopup("유니온 획득", $"유니온 {unionIndex} 획득");
        GlobalData.instance.lotteryManager.TotalDrawCountUiUpdate();

        if (unionRewardQueue.Count <= 0)
        {
            // 유니온 획득 버튼 비활성화
            UtilityMethod.SetBtnInteractableEnable(68, false);
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

    public void RewardByType(EnumDefinition.RewardType rewardType, int rewardValye)
    {
        rewardDic[rewardType].Invoke(rewardValye);

        //TODO: 획득 연출 추가

        // 획득 이벤트 타입과 획득량을 로그로 출력함
        Debug.Log($"획득 이벤트 타입 : {rewardType}, 획득량 : {rewardValye}");
    }


    public void RewardUnion(int unionIndex)
    {
        //TODO: 획득 연출 추가 ( 팝업 )

        // 획득
        GlobalData.instance.unionManager.AddUnion(unionIndex);
        // 획득한 유니온을 로그로 출력함
        Debug.Log($"획득한 유니온 번호 : {unionIndex}");
    }

}
