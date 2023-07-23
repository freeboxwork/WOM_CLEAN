using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;


public class AD_Manager : MonoBehaviour
{

    // 광고 시청 후 랜덤한 보석 보상

    public ADRandomRewardPopupController adRandomRewardPopupController;
    public List<BuffADSlot> buffADSlots = new List<BuffADSlot>();



    void Start()
    {

    }

    public IEnumerator Init()
    {
        SetBuffAdSlotData();
        yield return null;
    }


    void SetBuffAdSlotData()
    {
        var datas = GlobalData.instance.saveDataManager.saveDataTotal.saveDataBuffAD.buffAD_LeftDatas;
        foreach (var data in datas)
        {
            if (data.isUsing)
            {
                // 타이머 재개
                var slot = GetBuffAdSlotByType(data.buffADType);
                slot.buffTimer.ReloadTimer(data.leftTime);
            }
        }

    }

    public BuffADSlot GetBuffAdSlotByType(EnumDefinition.RewardTypeAD buffADType)
    {
        return buffADSlots.Where(x => x.buffADType == buffADType).FirstOrDefault();
    }

    public void RewardAd(EnumDefinition.RewardTypeAD adRewardType, int reward = 0)
    {

        switch (adRewardType)
        {
            case EnumDefinition.RewardTypeAD.adGoldPig:
                GlobalData.instance.goldPigController.goldPigPopup.AdReward();
                break;
            case EnumDefinition.RewardTypeAD.adGem:
                adRandomRewardPopupController.GetRandomReward();
                break;
            case EnumDefinition.RewardTypeAD.adBuffDamage:
                GetBuffAdSlotByType(adRewardType).BuffTimerStart();
                break;
            case EnumDefinition.RewardTypeAD.adBuffSpeed:
                GetBuffAdSlotByType(adRewardType).BuffTimerStart();
                break;
            case EnumDefinition.RewardTypeAD.adBuffGold:
                GetBuffAdSlotByType(adRewardType).BuffTimerStart();
                break;
        }

    }

    // 광고 시청 횟수 초기화
    public void AllResetBuffAdLeftCount()
    {
        foreach (var slot in buffADSlots)
        {
            slot.ResetLeftCount();
        }
    }
    public void ResetLeftCount(EnumDefinition.RewardTypeAD buffADType)
    {
        var slot = GetBuffAdSlotByType(buffADType);
        slot.ResetLeftCount();
    }
}
