using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class QuestPopup : MonoBehaviour
{
    public QuestManager QuestManager;

    public Button btn_close;
    public Button btn_showListOneDay;
    public Button btn_showAttend;
    public Button btn_showBattlePass;
    public List<QuestSlot> questSlotsOneDay;
    public List<BattlePassSlot> battlePassSlots;
    public List<AttendSlot> attendSlots;

    public GameObject questListOneDay;
    public GameObject attandPanel;
    public GameObject battlePassPanel;




    void Start()
    {
        SetBtnEvents();
    }

    void SetBtnEvents()
    {
        btn_showListOneDay.onClick.AddListener(ShowQuestListOneDay);
        btn_showAttend.onClick.AddListener(ShowAttend);
        btn_showBattlePass.onClick.AddListener(ShowBattlePass);
        btn_close.onClick.AddListener(ClosePopup);
    }

    void ClosePopup()
    {
        QuestManager.btn_showQuestPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    void ShowQuestListOneDay()
    {
        questListOneDay.SetActive(true);
        attandPanel.SetActive(false);
        battlePassPanel.SetActive(false);
    }

    void ShowAttend()
    {
        questListOneDay.SetActive(false);
        attandPanel.SetActive(true);
        battlePassPanel.SetActive(false);
    }

    void ShowBattlePass()
    {
        questListOneDay.SetActive(false);
        attandPanel.SetActive(false);
        battlePassPanel.SetActive(true);
    }


    // void InitOneDayQuestUI(List<QuestData> questDatas)
    // {

    //     for (int i = 0; i < questDatas.Count; i++)
    //     {

    //         var data = questDatas[i];
    //         var slot = questSlotsOneDay[i];
    //         SetUIQusetSlot(slot, data);

    //     }

    // }

    public void SetUIQusetSlot(QuestSlot slot, QuestData data)
    {
        slot.ActiveNotifyIcon(data);
        slot.SetTxtRewardValue(data.rewardValue.ToString());
        slot.SetQuestName(data.questName);
        slot.SetQuestProgress(data);
        slot.SetQuestProgressCount(data);
        slot.ActiveRewardButton(data);
        slot.SetDoingText(data);
        slot.SetQuestTypeOneDay(ConvertStringToQuestType(data.questType));
        slot.SetQuestData(data);
    }

    public void SetUIBattlePassSlot(BattlePassSlot slot, BattlePassData data)
    {
        slot.SetTxtStage(data.targetStage.ToString());
        slot.SetTxtRewardValue(data.commonRewardCount.ToString());
        slot.SetTxtPassRewardValue(data.passRewardCount.ToString());
        slot.battlePassData = data;
        //slot.targetStage = data.targetStage;
        // set icon image
        var rewardIcon = GlobalData.instance.spriteDataManager.GetRewardIcon(UtilityMethod.GetRewardTypeByTypeName(data.rewardType));
        slot.SetRewardIcon(rewardIcon);
        slot.SetPassRewardIcon(rewardIcon);
    }

    public void SetUIAttendSlot(AttendSlot slot, AttendData data, int unlockCount)
    {
        slot.SetTxtDayCount(data.day.ToString());
        slot.SetTxtRewardValue(data.rewardValue.ToString());
        slot.SetRewardIcon(GlobalData.instance.spriteDataManager.GetRewardIcon(UtilityMethod.GetRewardTypeByTypeName(data.rewardType)));

        var isLock = data.id > unlockCount;
        slot.SetBlockImage(isLock);


        // 리워드 사용 확인
        if (isLock == false)
        {
            var loadKey = $"{GlobalData.instance.questManager.keyAttendUsedReawrd}_{data.day}";
            var hasKey = PlayerPrefs.HasKey(loadKey);

            // Debug.Log($"hasKey : {hasKey} , loadKey : {loadKey} ");

            if (hasKey)
            {
                var enableValue = PlayerPrefs.GetInt(loadKey) == 0 ? true : false;
                slot.SetBtnRewardInteractable(enableValue);
            }
        }
        slot.attendData = data;
    }



    public QuestSlot GetQuestSlotByQuestTypeOneDay(EnumDefinition.QuestTypeOneDay type)
    {
        return questSlotsOneDay.Where(x => x.questTypeOneDay == type).FirstOrDefault();
    }


    public EnumDefinition.QuestTypeOneDay ConvertStringToQuestType(string questTypeString)
    {
        if (string.IsNullOrEmpty(questTypeString))
        {
            throw new System.ArgumentException("questTypeString cannot be null or empty.");
        }

        if (!System.Enum.TryParse(questTypeString, out EnumDefinition.QuestTypeOneDay questTypeEnum))
        {
            throw new System.ArgumentException($"{questTypeString} is not a valid value for QuestTypeOneDay enum.");
        }

        return questTypeEnum;
    }


    public void UnlockBattlePassSlot(int stageId)
    {
        var slot = GetBattlePassSlotByStage(stageId);
        slot.SetBlockImage(false);
    }

    public void UnlockBattlePassSlotItem(int stageId)
    {
        var slot = GetBattlePassSlotByStage(stageId);
        slot.SetBlockPassImage(false);
    }



    BattlePassSlot GetBattlePassSlotByStage(int stage)
    {
        return battlePassSlots.Where(x => x.battlePassData.targetStage == stage).FirstOrDefault();
    }

}

