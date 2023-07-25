using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;


public class AD_Manager : MonoBehaviour
{

    // 광고 시청 후 랜덤한 보석 보상

    public ADRandomRewardPopupController adRandomRewardPopupController;
    public List<BuffADSlot> buffADSlots = new List<BuffADSlot>();

    public string adPassKey = "_adPass";
    public string buffPassKey = "_buffPass";

    public bool isPassAD = false;
    public bool isPassBuff = false;

    public GameObject btnBuffOpen;


    void Start()
    {
        LoadPassValues();
    }

    // 광고,버프 패스 확인
    void LoadPassValues()
    {
        isPassAD = LoadPassValue(adPassKey);
        isPassBuff = LoadPassValue(buffPassKey);
    }

    bool LoadPassValue(string key)
    {
        if (!PlayerPrefs.HasKey(key))
        {
            PlayerPrefs.SetInt(key, 0);
            return false;
        }

        return PlayerPrefs.GetInt(key) == 1;
    }



    public IEnumerator Init()
    {
        SetBuffAdSlotData();
        yield return null;
    }


    void SetBuffAdSlotData()
    {
        var datas = GlobalData.instance.saveDataManager.saveDataTotal.saveDataBuffAD.buffAD_LeftDatas;

        // buff pass
        if (isPassBuff)
        {
            btnBuffOpen.SetActive(false);
            foreach (var data in datas)
            {
                var slot = GetBuffAdSlotByType(data.buffADType);
                slot.addValue = slot.addValueBuff;
                slot.buffTimer.SetTxtBuffPass();
            }
        }
        else
        {
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

        GetBuffAdSlotByType(datas[0].buffADType).buffTimer.SetTxtBuffPass();

    }

    public BuffADSlot GetBuffAdSlotByType(EnumDefinition.RewardTypeAD buffADType)
    {
        return buffADSlots.Where(x => x.buffADType == buffADType).FirstOrDefault();
    }

    public void RewardAd(EnumDefinition.RewardTypeAD adRewardType, int reward = 0)
    {
        // 일일 퀘스트 완료 : 광고보기 완료
        EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.showAd);

        switch (adRewardType)
        {
            case EnumDefinition.RewardTypeAD.adGoldPig:
                GlobalData.instance.goldPigController.goldPigPopup.RewardAD();
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
            case EnumDefinition.RewardTypeAD.adOffline:
                GlobalData.instance.offlineRewardPopupContoller.RewardAD();
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
