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

    bool isPassAD = false;
    public bool isPassBuff = false;

    public GameObject btnBuffOpen;



    // 광고,버프 패스 확인
    void LoadPassValues()
    {
        isPassAD = LoadPassValue(adPassKey);
        isPassBuff = LoadPassValue(buffPassKey);
    }

    public bool GetADpAssValue()
    {
        isPassAD = LoadPassValue(adPassKey);
        return isPassAD;
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
        LoadPassValues();

        SetBuffAdSlotData();
        yield return null;
    }


    void SetBuffAdSlotData()
    {
        var datas = GlobalData.instance.saveDataManager.saveDataTotal.saveDataBuffAD.buffAD_LeftDatas;

        // buff pass
        if (isPassBuff)
        {
            //btnBuffOpen.SetActive(false);

            foreach (var data in datas)
            {
                //좌측 상단 버프 상태 아이콘 관련 세팅
                var slot = GetBuffAdSlotByType(data.buffADType);
                slot.addValue = slot.addValueBuff;
                slot.isUsingBuff = true;
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
    }

    public void BuyBuffPass()
    {

        isPassBuff = true;
        PlayerPrefs.SetInt(buffPassKey, 1);
        //btnBuffOpen.SetActive(false);

        foreach (var slot in buffADSlots)
        {
            // 타이머 종료
            slot.buffTimer.TimerEnd();
            slot.isUsingBuff = true;
            slot.addValue = slot.addValueBuff;
            slot.buffTimer.SetTxtBuffPass();
        }
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
            case EnumDefinition.RewardTypeAD.adDungeon:
                GlobalData.instance.dungeonEnterPopup.AD_DungeonIn();
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

    public void AllResetADLeftCount()
    {
        // 던전 광고 키 1개로 초기화
        GlobalData.instance.player.ResetDungeonADKeys();
        // 광고 버프 카운드 2개로 초기화
        foreach (var slot in buffADSlots)
        {
            slot.ResetLeftCount();
        }
        // 보석 광고 카운트 2개로 초기화
        adRandomRewardPopupController.ResetLeftCount();
    }

}
