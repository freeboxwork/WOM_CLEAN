using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class AD_Manager : MonoBehaviour
{
    public List<BuffADSlot> buffADSlots = new List<BuffADSlot>();
    void Start()
    {

    }

    IEnumerator Init()
    {
        yield return null;
    }


    void SetBuffAdSlotData()
    {
        // foreach (var slot in buffADSlots)
        // {
        //   var data = GlobalData.instance.saveDataManager.GetSaveDataBuffAD_LeftDataByType(slot.buffADType);

        // }
    }

    BuffADSlot GetBuffAdSlotByType(EnumDefinition.RewardTypeAD buffADType)
    {
        return buffADSlots.Where(x => x.buffADType == buffADType).FirstOrDefault();
    }

    public void RewardAd(EnumDefinition.RewardTypeAD adRewardType, int reward = 0)
    {

        switch (adRewardType)
        {
            case EnumDefinition.RewardTypeAD.adGold:
                break;
            case EnumDefinition.RewardTypeAD.adGem:

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
}
