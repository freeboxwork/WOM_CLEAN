using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class QuestPopup : MonoBehaviour
{
    public QuestManager QuestManager;

    public Button btn_close;
    public Button btn_showListOneDay;
    public Button btn_showListRepeat;

    public GameObject questListOneDay;
    public GameObject questListRepeat;


    public List<QuestSlot> questSlotsOneDay;
    public List<QuestSlot> questSlotsRepeat;

    void Start()
    {
        SetBtnEvents();
    }

    void SetBtnEvents()
    {
        btn_showListOneDay.onClick.AddListener(ShowQuestListOneDay);
        btn_showListRepeat.onClick.AddListener(ShowQuestListRepeat);
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
        questListRepeat.SetActive(false);
    }

    void ShowQuestListRepeat()
    {
        questListOneDay.SetActive(false);
        questListRepeat.SetActive(true);
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

    void InitRepeatQuestUI(List<QuestData> questDatas)
    {


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

}
