using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumDefinition;

public class SaleManager : MonoBehaviour
{
    Player player;
    TraningManager traningManager;
    Queue<SaleStatMsgData> saleStatMsgs = new Queue<SaleStatMsgData>();

    void Start()
    {
        GetPlayer();
        StartCoroutine( SandMessage());
    }
   
    void GetPlayer()
    {
        player = GlobalData.instance.player;
        traningManager = GlobalData.instance.traningManager;
    }

    public void AddData(SaleStatMsgData data)
    {
        saleStatMsgs.Enqueue(data);
    }
        

    IEnumerator SandMessage()
    {
        while (true)
        {
            if(saleStatMsgs.Count > 0)
            {
                var data = saleStatMsgs.Dequeue();
                yield return StartCoroutine(PurchaseStatByType(data.saleStatType));
            }
            yield return null;
        }
    }

    
    public IEnumerator PurchaseStatByType(SaleStatType statType)
    {
        // get current level & next statData by statType
        var payData = GetPayData(statType);

        // 최대 레벨 체크
        if (payData.isValidMaximumLevel)
        {
            //재화 : GOLD
            if (IsGoldItem(statType))
            {
                // 재화 충분 한지 판단
                if (IsValidPurchaseGold(payData.statData.salePrice))
                {
                    // 구매 
                    player.PayGold(payData.statData.salePrice);

                    // 데이터 적용
                    traningManager.SetInGameStatLevel(statType, payData.statData.level);
                    traningManager.SetInGameStatValue(statType, payData.statData.value);
                    
                    // 세이브 데이터 업데이트
                    GlobalData.instance.saveDataManager.SetLevelByTraningType(statType, payData.statData.level);

                    // UI 적용
                    if (payData.nextStatSaleData != null)
                    {
                        traningManager.SetUI_TraningSlot(statType);
                    }
                    else // 최종레벨 도달
                    {
                        Debug.Log("최종레벨 도달");
                    }
                }
                else
                {
                    // 골드가 부족 합니다.
                    Debug.Log($"{statType} - 골드가 부족 합니다.");
                }
            }
            //재화 : BONE
            else
            {
                if (IsValidPurchaseBone(payData.statData.salePrice))
                {
                    // 구매
                    player.PayBone(payData.statData.salePrice);

                    // 데이터 적용
                    traningManager.SetInGameStatLevel(statType, payData.statData.level);
                    traningManager.SetInGameStatValue(statType, payData.statData.value);

                    // 세이브 데이터 업데이트
                    GlobalData.instance.saveDataManager.SetLevelByTraningType(statType, payData.statData.level);

                    // UI 적용
                    if (payData.nextStatSaleData != null)
                    {
                        traningManager.SetUI_TraningSlot(statType);
                    }
                    else // 최종레벨 도달
                    {
                        Debug.Log("최종레벨 도달");
                    }
                }
                else
                {
                    // 뼈 조각이 부족 합니다.
                    Debug.Log($"{statType} - 뼈 조각이 부족합니다.");
                }
            }
        }
        else
        {
            // 최대 레벨 도달
            Debug.Log($"{statType} - 최대 레벨에 도달했습니다..");
        }

        yield return null;
    }

    (int level , StatSaleData statData, bool isValidMaximumLevel, StatSaleData nextStatSaleData) GetPayData( SaleStatType statType)
    {
        (int m_level, StatSaleData m_statData,bool m_isValidMaxLevel, StatSaleData m_nextStatData) items;
        var statDatas = GlobalData.instance.dataManager.GetSaleStatDataByType(statType);
        items.m_level = traningManager.GetInGameStatLevel(statType);
    
        // 맥시멈 체크
        var lastData = statDatas.data.Last();
        items.m_isValidMaxLevel =  lastData.level > items.m_level;
        if (items.m_isValidMaxLevel)
        {
            items.m_statData = GetStatSaleData(statDatas, items.m_level + 1);
        }
        else
        {
            items.m_statData = null;
        }

        

        // 다음 레벨 데이터 존재 하는지 체크
        if(lastData.level > items.m_level+2)
        {
            items.m_nextStatData = GetStatSaleData(statDatas, items.m_level + 2);
        }
        else
        {
            items.m_nextStatData = null;
        }
        
        return items;
    }


    StatSaleData GetStatSaleData(StatSaleDatas data, int level)
    {
        return data.data.FirstOrDefault(f => f.level == level);
    }

    bool IsGoldItem(SaleStatType saleStat)
    {
        return (saleStat == SaleStatType.trainingDamage || saleStat == SaleStatType.trainingCriticalChance || saleStat == SaleStatType.trainingCriticalDamage);
    }


    



    //UTILITY 

    bool IsValidPurchaseGold(int value)
    {
        return 0 <= player.gold - value;
    }


    bool IsValidPurchaseBone(int value)
    {
        return 0 <= player.bone - value;
    }


    bool IsValidMaximumLevel(SaleStatType statType ,int curLevel)
    {
        int maxumLevel = 100;
        return maxumLevel > curLevel;
    }
    

}
