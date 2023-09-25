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


    public Button[] passTabButtons;

    public ScrollRect passScrollLect;

    public GameObject[] passPanels;

    public Color selectColor;
    public Color deSelectColor;

    void Start()
    {
        SetBtnEvents();
        ShowPassTabPanel(0);
    }

    void SetBtnEvents()
    {
        btn_showListOneDay.onClick.AddListener(ShowQuestListOneDay);
        btn_showAttend.onClick.AddListener(ShowAttend);
        btn_showBattlePass.onClick.AddListener(ShowBattlePass);
        btn_close.onClick.AddListener(ClosePopup);


        passTabButtons[0].onClick.AddListener(() => ShowPassTabPanel(0));   
        passTabButtons[1].onClick.AddListener(() => ShowPassTabPanel(1));   
        passTabButtons[2].onClick.AddListener(() => ShowPassTabPanel(2));   
        passTabButtons[3].onClick.AddListener(() => ShowPassTabPanel(3));   
        passTabButtons[4].onClick.AddListener(() => ShowPassTabPanel(4));   

    }

    void ClosePopup()
    {
        //QuestManager.btn_showQuestPopup.gameObject.SetActive(true);
        gameObject.SetActive(false);
    }

    void ShowPassTabPanel(int index)
    {
        for(int i = 0; i < passPanels.Length; i++)
        {
            passPanels[i].SetActive(false);
            passTabButtons[i].GetComponent<Image>().color = deSelectColor;
        }
        passTabButtons[index].GetComponent<Image>().color = selectColor;
        passPanels[index].SetActive(true);
        passScrollLect.content = passPanels[index].GetComponent<RectTransform>();
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
        slot.ActiveADButton(data);
    }

    public void SetUIBattlePassSlot(BattlePassSlot slot, BattlePassData data, int unlockCount)
    {
        slot.SetTxtStage(data.targetStage.ToString());
        slot.SetTxtRewardValue(data.commonRewardCount.ToString());
        slot.SetTxtPassRewardValue(data.passRewardCount.ToString());
        slot.battlePassData = data;

        var rewardIconType = UtilityMethod.GetRewardTypeByTypeName(data.commonRewardType);
        var rewardIcon = GlobalData.instance.spriteDataManager.GetRewardIcon(rewardIconType);
        slot.SetRewardIcon(rewardIcon);

        var passRewardIconType = UtilityMethod.GetRewardTypeByTypeName(data.passRewardType);
        var passRewardIcon = GlobalData.instance.spriteDataManager.GetRewardIcon(passRewardIconType);
        slot.SetPassRewardIcon(passRewardIcon);

        var isLock = data.targetStage > unlockCount;

        slot.SetBlockImage(isLock);


        // 리워드 사용확인
        if (isLock == false)
        {

            string loadKey = GlobalData.instance.questManager.keyBattlePassUsedReward + "_" + data.targetStage;
            bool enableCurrentValue = true;

            if (PlayerPrefs.HasKey(loadKey))
            {
                int storedValue = PlayerPrefs.GetInt(loadKey);
                enableCurrentValue = storedValue == 0;
            }

            slot.SetBtnRewardInteractable(enableCurrentValue);

            var buyPass = GlobalData.instance.questManager.IsBattlePassBuy();

            if(buyPass == false)
            {
                slot.SetBtnPassRewardInteractable(false);
                return;
            }

            string loadKeyBuyItem = GlobalData.instance.questManager.keyBuyBattlePass + "_" + data.targetStage;
            bool enablePassValue = true;

            if (PlayerPrefs.HasKey(loadKeyBuyItem))
            {
                int storedValue = PlayerPrefs.GetInt(loadKeyBuyItem);
                enablePassValue = storedValue == 0;
            }

            slot.SetBtnPassRewardInteractable(enablePassValue);

        }
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
            else
            {
                slot.SetBtnRewardInteractable(true);

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

        if (slot != null)
        {
            slot.SetBlockImage(false);
            slot.SetBtnRewardInteractable(true);
            var buyPass = GlobalData.instance.questManager.IsBattlePassBuy();
            slot.SetBlockPassImage(!buyPass);
            slot.SetBtnPassRewardInteractable(buyPass);
        }
                  
    }

    public void AllUnlockBattlePassSlotItem()
    {
        foreach (var slot in battlePassSlots)
        {
            slot.SetBlockPassImage(false);

            if(slot.IsActiveHideBlockImage() == false)
            {
                slot.SetBtnPassRewardInteractable(true);
            }

        }
    }

    BattlePassSlot GetBattlePassSlotByStage(int stage)
    {
        return battlePassSlots.Where(x => x.battlePassData.targetStage == stage).FirstOrDefault();
    }

}

