using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using static EnumDefinition;

public class DungeonEnterPopup : MonoBehaviour
{

    public Image keyIcon;
    public Image goodsIcon;

    public TextMeshProUGUI textClearTicket;
    public TextMeshProUGUI textKeyCount;
    public TextMeshProUGUI textADKeyCount;
    public TextMeshProUGUI textPervClearLevel;
    public TextMeshProUGUI textRewardValue;

    public SerializableDictionary<EnumDefinition.MonsterType, Sprite> monsterTypeToIconMap;
    public SerializableDictionary<EnumDefinition.MonsterType, Sprite> monsterTypeToGoodsIconMap;

    public EnumDefinition.MonsterType curMonsterType;
    public GameObject contents;

    public Button btn_AD_Dungeon;
    public Button btn_KeyDungeon;
    public Button btn_Ticket_Dungeon;


    // 소탕권 관련
    int curLevel;
    public int clearTicketCount = 2;
    DungeonMonsterData curDungeonMonData;
    Dictionary<MonsterType, UnityAction<int>> addRewardMap;

    private void Start()
    {
        SetBtnEvents();
        addRewardMap = new Dictionary<MonsterType, UnityAction<int>>() {
        { MonsterType.dungeonGold, GlobalData.instance.player.AddGold },
        { MonsterType.dungeonBone, GlobalData.instance.player.AddBone },
        { MonsterType.dungeonDice, GlobalData.instance.player.AddDice },
        { MonsterType.dungeonCoal, GlobalData.instance.player.AddCoal },
    };
    }



    void SetBtnEvents()
    {
        btn_KeyDungeon.onClick.AddListener(() =>
        {
            if (IsValidDungeonKeyCount(curMonsterType))
            {
                // 일일 퀘스트 완료 : 던전
                EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.clearDungeon);
                EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDungeonMonsterChallenge, curMonsterType);
            }
            contents.SetActive(false);
        });

        btn_AD_Dungeon.onClick.AddListener(() =>
        {
            if (IsValidDungeonKeyCount(curMonsterType))
            {
                // 일일 퀘스트 완료 : 던전
                EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.clearDungeon);
                EventManager.instance.RunEvent(CallBackEventType.TYPES.OnDungeonMonsterChallenge, curMonsterType);

                if (GlobalData.instance.player.GetDungeonADKeyCountByMonsterType(curMonsterType) <= 0)
                {
                    btn_AD_Dungeon.interactable = false;
                }
            }
            contents.SetActive(false);
        });

        btn_Ticket_Dungeon.onClick.AddListener(() =>
        {
            UsingClearTicket();
        });
    }




    void UsingClearTicket()
    {
        var player = GlobalData.instance.player;
        var curTicketCount = player.clearTicket;
        if (curTicketCount >= clearTicketCount)
        {
            // pay ticket
            GlobalData.instance.player.PayClearTicekt(clearTicketCount);
            // 일일 퀘스트 완료 : 던전
            EventManager.instance.RunEvent<EnumDefinition.QuestTypeOneDay>(CallBackEventType.TYPES.OnQusetClearOneDayCounting, EnumDefinition.QuestTypeOneDay.clearDungeon);

            // reward
            PopupController.instance.InitPopup(EnumDefinition.RewardType.gold, curDungeonMonData.currencyAmount);
            // addRewardMap[curMonsterType].Invoke(curDungeonMonData.currencyAmount);

            Debug.Log("소탕권 사용. 리워드 지급");
        }
        else
        {
            // 티켓 부족 팝업
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("", 17);
        }
    }

    public void SetTxtClierTicket(int ticketCount)
    {
        textClearTicket.text = ticketCount.ToString();
    }


    public void EnablePopup(EnumDefinition.MonsterType monsterType)
    {
        curMonsterType = monsterType;
        curLevel = GlobalData.instance.player.dungeonMonsterClearLevel.GetLeveByDungeonMonType(curMonsterType);
        curDungeonMonData = GlobalData.instance.dataManager.GetDungeonMonsterDataByTypeLevel(curMonsterType, curLevel);

        SetKeyUI(monsterType);
        SetRewardUI(curLevel, curDungeonMonData);

        contents.SetActive(true);
    }

    void SetRewardUI(int level, DungeonMonsterData data)
    {
        textPervClearLevel.text = $"{level}단계";
        textRewardValue.text = data.currencyAmount.ToString();

    }

    // 던전 몬스터 열쇠 사용 가능 체크
    bool IsValidDungeonKeyCount(EnumDefinition.MonsterType monsterType)
    {
        var usingKeyCount = GlobalData.instance.monsterManager.GetMonsterDungeon().monsterToDataMap[monsterType].usingKeyCount;

        int curKeyCount = 0;
        if (GlobalData.instance.player.GetCurrentDungeonKeyCount(monsterType) > 0)
        {
            curKeyCount = GlobalData.instance.player.GetCurrentDungeonKeyCount(monsterType);
        }
        else
        {
            curKeyCount = GlobalData.instance.player.GetDungeonADKeyCountByMonsterType(monsterType);
        }

        if (curKeyCount < usingKeyCount)
        {
            // enable popup
            // TODO: 코드 간결화 및 리펙토링
            int messageId = 12;
            switch (monsterType)
            {
                case EnumDefinition.MonsterType.dungeonGold: messageId = 12; break;
                case EnumDefinition.MonsterType.dungeonBone: messageId = 13; break;
                case EnumDefinition.MonsterType.dungeonDice: messageId = 14; break;
                case EnumDefinition.MonsterType.dungeonCoal: messageId = 15; break;
            }
            // message popup (열쇠가 부족합니다)
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", messageId);

            return false;
        }
        return true;
    }


    public void SetKeyUI(EnumDefinition.MonsterType monsterType)
    {

        Debug.Log("던전 몬스터 타입 : " + monsterType.ToString());

        keyIcon.sprite = monsterTypeToIconMap[monsterType];
        goodsIcon.sprite = monsterTypeToGoodsIconMap[monsterType];
        // 던전별 보유 열쇠 수
        var keyCount = GlobalData.instance.player.GetCurrentDungeonKeyCount(monsterType);
        textKeyCount.text = keyCount.ToString();

        var adkeyCount = GlobalData.instance.player.GetDungeonADKeyCountByMonsterType(monsterType);
        textADKeyCount.text = adkeyCount.ToString();

        var hasKey = keyCount > 0;

        btn_KeyDungeon.gameObject.SetActive(hasKey);
        btn_AD_Dungeon.gameObject.SetActive(!hasKey);
        btn_AD_Dungeon.interactable = adkeyCount > 0;

        // 소탕권
        var ticket = GlobalData.instance.player.clearTicket;
        SetTxtClierTicket(ticket);
        btn_Ticket_Dungeon.interactable = ticket > 0;
    }




    public void SetDungeonEnterPopup(int clear, int key, Sprite sp)
    {
        textClearTicket.text = string.Format("{0}", clear);
        textKeyCount.text = string.Format("{0}", key);
        keyIcon.sprite = sp;
    }








}
