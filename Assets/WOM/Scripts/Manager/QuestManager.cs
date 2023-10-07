using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static EnumDefinition;
using System.Linq;

public class QuestManager : MonoBehaviour
{

    public Button btn_showQuestPopup;
    public Button btn_showNewUserEventPopup;
    public QuestPopup questPopup;
    public NewUserEventPopup newUserEventPopup;

    private Dictionary<QuestTypeOneDay, QuestData> questsOneDay = new Dictionary<QuestTypeOneDay, QuestData>();

    const string keyUsingReward = "_usingReward";
    const string keyQuestComplete = "_questComplete";
    const string keyUsingRewardAd = "_usingRewardAD";

    public QuestResetTimer questResetTimer;

    public AttendTimer attendTimer;
    public NewUserEventTimer newUserEventTimer;

    public string keyAttendUsedReawrd = "_attendUsedReward";
    public string keyNewUserEventUsedReward = "_newUserEventUsedReward";
    public string keyBattlePassUsedReward = "_battlePassUsedReward";
    public string keyBuyBattlePass = "_buyBattlePass";



    void Start()
    {
        AddEvents();
    }

    void OnDestroy()
    {
        RemoveEvents();
    }

    void AddEvents()
    {
        EventManager.instance.AddCallBackEvent<QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, IncreaseCountOneDayQuest);
        EventManager.instance.AddCallBackEvent<QuestData>(CallBackEventType.TYPES.OnQusetUsingRewardOneDay, EvnUsingReward);
        EventManager.instance.AddCallBackEvent<QuestData>(CallBackEventType.TYPES.OnQusetUsingRewardOneDayAD, EvnUsingRewardAD);
        EventManager.instance.AddCallBackEvent<string, int>(CallBackEventType.TYPES.OnQuestCompleteBattlePassStage, EvnUsingRewardBattlePassStage);
        EventManager.instance.AddCallBackEvent<string, int>(CallBackEventType.TYPES.OnQuestCompleteBattlePassStageBuyitem, EvnUsingRewardBattlePassBuyItem);
        EventManager.instance.AddCallBackEvent<string, int>(CallBackEventType.TYPES.OnUsingRewardAttend, EvnUsingRewardAttend);
        EventManager.instance.AddCallBackEvent<string[], int[]>(CallBackEventType.TYPES.OnUsingRewardNewUserEvent, EvnUsingRewardNewUserEvent);
    }

    void RemoveEvents()
    {
        EventManager.instance.RemoveCallBackEvent<QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, IncreaseCountOneDayQuest);
        EventManager.instance.RemoveCallBackEvent<QuestData>(CallBackEventType.TYPES.OnQusetUsingRewardOneDay, EvnUsingReward);
        EventManager.instance.RemoveCallBackEvent<QuestData>(CallBackEventType.TYPES.OnQusetUsingRewardOneDayAD, EvnUsingRewardAD);
        EventManager.instance.RemoveCallBackEvent<string, int>(CallBackEventType.TYPES.OnQuestCompleteBattlePassStage, EvnUsingRewardBattlePassStage);
        EventManager.instance.RemoveCallBackEvent<string, int>(CallBackEventType.TYPES.OnQuestCompleteBattlePassStageBuyitem, EvnUsingRewardBattlePassBuyItem);
        EventManager.instance.RemoveCallBackEvent<string[], int[]>(CallBackEventType.TYPES.OnUsingRewardNewUserEvent, EvnUsingRewardNewUserEvent);
    }


    public IEnumerator Init()
    {
        SetBtnEvent();
        AddOneDayQuestData();
        AddBattlePassData();

        // 일일 출석 보상 타이머를 계산한다.
        attendTimer.CalcAttendTimer();

        // 신규 유저 이벤트 타이머를 계산한다.
        newUserEventTimer.CalcTimer();

        yield return new WaitForEndOfFrame();

        AddAttendData();
        AddNewUserEventData();

        yield return null;
    }

    void SetBtnEvent()
    {
        btn_showQuestPopup.onClick.AddListener(() =>
        {
            questPopup.gameObject.SetActive(true);
            //btn_showQuestPopup.gameObject.SetActive(false);
        });

        btn_showNewUserEventPopup.onClick.AddListener(() =>
        {
            newUserEventPopup.EnablePopup(true);
        });
    }


    bool isOverOneDay = false;

    void AddOneDayQuestData()
    {
        var oneDayData = GlobalData.instance.dataManager.questDatasOneDay.data;

        //해시키가 있다면 자정시간이 세팅되었음
        if (questResetTimer.HasMidnightTime())
        {
            // 만약 퀘스트 재설정 타이머가 자정을 지난 경우 타이머를 재설정한다.
            if (questResetTimer.HasCrossedMidnight())
            {
                isOverOneDay = true;
                // 자정이 지난경우 던전 입장키 2개씩 보상.
                //Debug.Log("자정이 지났으므로 던전 입장 키를 2개씩 지급한다");
                GlobalData.instance.player.AddAllDungeonKeys();
                // 자정이 지난경우 일일 광고 보기 회 수 초기화
                GlobalData.instance.adManager.AllResetADLeftCount();
                //상점 열쇠 구매 카운트 초기화
                GlobalData.instance.shopManager.ResetBuyKeyCount();
                //자정시간 재설정
                questResetTimer.ResetTimer();
            }
        }

        for (int i = 0; i < oneDayData.Count; i++)
        {
            var clonData = oneDayData[i].ClonInstance();
            var slot = questPopup.questSlotsOneDay[i];
            //Debug.Log(clonData.curCountValue + " / " + clonData.targetValue + "111111");

            if (isOverOneDay == false)
            {
                //자정을 지나지 않은 경우 유저 메모리에서 저장된 데이터를 로드한다.
                LoadQuestDataFromUserMemory(ref clonData);
                //Debug.Log("자정이 지나지 않음");

            }

            questsOneDay.Add(GetQuestTypeOneDayByTypeName(clonData.questType), clonData);

            questPopup.SetUIQusetSlot(slot, clonData);

            //Debug.Log(clonData.curCountValue + " / " + clonData.targetValue + "222222");

        }

        isOverOneDay = false;
    }

    void AddBattlePassData()
    {
        var battlePassData = GlobalData.instance.dataManager.battlePassDatas.data;
        //for (int i = 0; i < battlePassData.Count; i++)
        for (int i = 0; i < questPopup.battlePassSlots.Count; i++)
        {
            var clonData = battlePassData[i].ClonInstance();
            var slot = questPopup.battlePassSlots[i];
            slot.SetBlockPassImage(!IsBattlePassBuy());
            var unlockCount = GlobalData.instance.player.stageIdx;
            questPopup.SetUIBattlePassSlot(slot, clonData, unlockCount);
        }
    }

    public bool IsBattlePassBuy()
    {
        if (PlayerPrefs.HasKey(keyBuyBattlePass))
        {
            return PlayerPrefs.GetInt(keyBuyBattlePass) == 1 ? true : false;
        }
        else
        {
            return false;
        }

    }

    void AddAttendData()
    {
        var attendData = GlobalData.instance.dataManager.attendDatas.data;
        var unLockCount = PlayerPrefs.GetInt("unlocked_attend_count");
        for (int i = 0; i < attendData.Count; i++)
        {
            var clonData = attendData[i].CopyInstance();
            var slot = questPopup.attendSlots[i];
            questPopup.SetUIAttendSlot(slot, clonData, unLockCount);
        }
    }

    void AddNewUserEventData()
    {

        var newUserEventData = GlobalData.instance.dataManager.newUserDatas.data;
        var unLockCount = PlayerPrefs.GetInt("unlocked_newUserEvent_count");

        for (int i = 0; i < newUserEventData.Count; i++)
        {
            var clonData = newUserEventData[i].ClonInstance();
            var slot = newUserEventPopup.newUserSlots[i];
            slot.SetUI(clonData, unLockCount);
        }

        bool NotAllUsingReward = false;
        //패널이 잠겨있지 않고 보상을 받을수 있는 상태인지 체크
        foreach (var v in newUserEventPopup.newUserSlots)
        {
            if (v.isLock == false && v.HasRewardKey() == false)
            {
                NotAllUsingReward = true;
                break;
            }
        }

        if (GlobalData.instance.tutorialManager.isEndTutorial)
            newUserEventPopup.EnablePopup(NotAllUsingReward);

        //더이상 보상받을 것이 없다면 버튼을 비활성화 한다.
        var clearAll = newUserEventPopup.newUserSlots.Any(x => x.HasRewardKey() == false);
        if (!clearAll) btn_showNewUserEventPopup.gameObject.SetActive(false);

    }


    void LoadQuestDataFromUserMemory(ref QuestData data)
    {


        if (PlayerPrefs.HasKey(data.questType))
        {
            data.curCountValue = PlayerPrefs.GetInt(data.questType);
        }

        if (PlayerPrefs.HasKey(data.questType + keyQuestComplete))
        {
            data.qusetComplete = PlayerPrefs.GetInt(data.questType + keyQuestComplete) == 1 ? true : false;
        }

        if (PlayerPrefs.HasKey(data.questType + keyUsingReward))
        {
            data.usingReward = PlayerPrefs.GetInt(data.questType + keyUsingReward) == 1 ? true : false;
        }

        if (PlayerPrefs.HasKey(data.questType + keyUsingRewardAd))
        {
            data.usingRewardAD = PlayerPrefs.GetInt(data.questType + keyUsingRewardAd) == 1 ? true : false;
        }

    }

    QuestTypeOneDay GetQuestTypeOneDayByTypeName(string typeName)
    {
        foreach (QuestTypeOneDay type in System.Enum.GetValues(typeof(QuestTypeOneDay)))
            if (type.ToString() == typeName)
                return type;
        return QuestTypeOneDay.none;
    }

    public void IncreaseCountOneDayQuest(QuestTypeOneDay type)
    {
        if (questsOneDay.ContainsKey(type))
        {

            var quest = questsOneDay[type];
            if (!quest.qusetComplete)
            {
                ++quest.curCountValue;
                if (quest.curCountValue >= quest.targetValue)
                {
                    quest.qusetComplete = true;
                }

                //ui update , playerprefs save event 실행
                var slot = questPopup.GetQuestSlotByQuestTypeOneDay(type);
                slot.UpdateUI(quest);

                // quest 의 변동 사항을 로그로 출력
                //Debug.Log("퀘스트 카운트 증가 : " + type.ToString() + " 현재 카운트 : " + quest.curCountValue + " / " + quest.targetValue);
                // save data
                SaveQuestData(quest);

                if (AllQuestComplete()) // 일일 퀘스트 전체 완료 체크
                {
                    // 일일 퀘스트 완료 : 모든 일일 퀘스트 완료
                    EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.allComplete);
                }
            }
        }
        else
        {
            Debug.LogError("퀘스트 카운트 에러 : " + type.ToString() + " 해당 퀘스트가 존재 하지 않습니다.");
        }
    }


    bool AllQuestComplete()
    {

        return questsOneDay.Where(x => x.Value != questsOneDay[EnumDefinition.QuestTypeOneDay.allComplete])
                      .All(x => x.Value.qusetComplete);
        // foreach (var v in questsOneDay)
        // {
        //     if (v.Value == questsOneDay[EnumDefinition.QuestTypeOneDay.allComplete])
        //         continue;
        //     else
        //     {
        //         if (!v.Value.qusetComplete)
        //             return false;
        //     }
        // }
        // return true;
    }


    // user quest data save
    void SaveQuestData(QuestData data)
    {
        PlayerPrefs.SetInt(data.questType, data.curCountValue);
        PlayerPrefs.SetInt(data.questType + keyQuestComplete, data.qusetComplete ? 1 : 0);
    }

    void EvnUsingReward(QuestData data)
    {
        data.usingReward = true;
        PlayerPrefs.SetInt(data.questType + keyUsingReward, data.usingReward ? 1 : 0);

        // 리워드 팝업 띄우기
        PopupController.instance.InitPopup(UtilityMethod.GetRewardTypeByTypeName(data.rewardType), data.rewardValue);
    }

    //일일퀘스트 버튼 이벤트 실행
    void EvnUsingRewardAD(QuestData data)
    {
        Admob.instance.ShowRewardedAdQusetOneDay(data);
    }

    public void RewardAD_OneDayQuest(QuestData data)
    {
        data.usingRewardAD = true;
        PlayerPrefs.SetInt(data.questType + keyUsingRewardAd, data.usingRewardAD ? 1 : 0);
        // 광고 실행
        // 리워드 팝업 띄우기
        PopupController.instance.InitPopup(UtilityMethod.GetRewardTypeByTypeName(data.rewardType), data.rewardValue);

        // 광고 버튼 비활성화
        data.questSlot.ActiveADButton(data);
    }

    void EvnUsingRewardBattlePassStage(string rewardType, int rewardValue)
    {
        // 리워드 팝업 띄우기
        var rewardTypeValue = UtilityMethod.GetRewardTypeByTypeName(rewardType);
        PopupController.instance.InitPopup(rewardTypeValue, rewardValue);
    }
    void EvnUsingRewardBattlePassBuyItem(string rewardType, int rewardValue)
    {
        // 리워드 팝업 띄우기
        var rewardTypeValue = UtilityMethod.GetRewardTypeByTypeName(rewardType);
        PopupController.instance.InitPopup(rewardTypeValue, rewardValue);
    }


    void EvnUsingRewardAttend(string rewardType, int rewardValue)
    {
        // 리워드 팝업 띄우기
        var rewardTypeValue = UtilityMethod.GetRewardTypeByTypeName(rewardType);
        PopupController.instance.InitPopup(rewardTypeValue, rewardValue);
    }


    void EvnUsingRewardNewUserEvent(string[] rewardTypes, int[] rewardValues)
    {
        List<EnumDefinition.RewardType> rewards = new List<RewardType>();
        foreach (var v in rewardTypes)
        {
            rewards.Add(UtilityMethod.GetRewardTypeByTypeName(v));
        }

        // 리워드 팝업 띄우기
        PopupController.instance.InitPopups(rewards.ToArray(), rewardValues);
    }





}
