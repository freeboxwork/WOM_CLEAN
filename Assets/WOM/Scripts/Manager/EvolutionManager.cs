using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EnumDefinition;



public class EvolutionManager : MonoBehaviour
{
    // 진화 주사위 돌려서 획득한 데이터 저장
    public DiceEvolutionInGameData diceEvolutionData;

    // 진화 레벨
    public int evalutionLeveldx;

    [Header("진화 UI 관련 항목")]
    public List<Sprite> evolutionGradeBadgeImages = new List<Sprite>();
    public List<EvolutionSlot> evolutionSlots = new List<EvolutionSlot>();

    void Start()
    {

    }


    public IEnumerator Init()
    {
        SetButtonEvents();

        // 진화 판넬 UI 초기 값 세팅
        SetUI_Pannel_Evolution(GlobalData.instance.evolutionManager.evalutionLeveldx);

        // 진화 슬롯 UI 초기 세팅 
        SetUI_EvolutionSlots(GlobalData.instance.evolutionManager.evalutionLeveldx);

        // 진화 주사위 사용 개수 세팅
        SetTxtUsingDiceCount();

        yield return null;
    }

    public void SetUI_Pannel_Evolution(int dataId)
    {
        var data = GlobalData.instance.dataManager.GetRewaedEvolutionGradeDataByID(dataId);

        // set badge image
        UtilityMethod.SetImageSpriteCustomTypeByID(21, evolutionGradeBadgeImages[data.id]);

        // set txt evolution grade 
        UtilityMethod.SetTxtCustomTypeByID(61, data.evolutionGradeType);

        // set txt damage rate
        UtilityMethod.SetTxtCustomTypeByID(62, $"{data.damageRate}");

        // set txt slot count
        UtilityMethod.SetTxtCustomTypeByID(63, $"{data.slotCount}");
    }


    public void SetUI_EvolutionSlots(int dataId)
    {
        var data = GlobalData.instance.dataManager.GetRewaedEvolutionGradeDataByID(dataId);

        // slot count 만큼 슬롯 열어줌
        for (int i = 0; i < data.slotCount; i++)
        {
            evolutionSlots[i].UnLockSlot();
        }

        // 자물쇠 기본 열림상태
        //foreach (var slot in evolutionSlots)
        //    slot.LockEvent();
    }

    public void SetUI_EvolutuinSlotsLockerItems(int dataId)
    {

        var data = GlobalData.instance.dataManager.GetRewaedEvolutionGradeDataByID(dataId);

        // 자물쇠 오픈
        evolutionSlots[data.id].UnLock();
        //for (int i = 0; i < data.slotCount; i++)
        //{
        //    evolutionSlots[i].UnLock();
        //}

        //사용에 필요한 주사위 개수 변경
        GlobalData.instance.evolutionManager.SetTxtUsingDiceCount();

        //모든 slot 오픈시 진화전 이동 버튼 비활성화
        if (data.slotCount == GlobalData.instance.dataManager.rewardEvolutionGradeDatas.data.Max(m => m.slotCount))
        {
            UtilityMethod.GetCustomTypeBtnByID(20).interactable = false;
        }
    }


    public void SetEvolutuinSlotName(EvolutionDiceStatType type, EvolutionSlot slot, float value, string clorHexCode)
    {
        var data = GlobalData.instance.dataManager.GetConvertTextDataByEvolutionDiceStatType(type);
        var txtValue = $"{data.kr_Front} {value}{data.kr_Back}";
        slot.SettxtStatName($"<color=#{clorHexCode}>{txtValue}</color>");
    }



    public void EanbleBtnEvolutionRollDice()
    {
        // 주사위 하나라도 오픈 되어 있으면 주사위 굴리기 버튼 활성화
        var enableValue = evolutionSlots.Any(a => a.isUnlock == true);

        // 22 : 주사위 굴리기 버튼
        UtilityMethod.GetCustomTypeBtnByID(22).interactable = enableValue;
        var diceImageColor = enableValue ? Color.white : Color.gray;
        UtilityMethod.GetCustomTypeImageById(22).color = diceImageColor;

        // 주사위 사용 개수 
        var count = UtilityMethod.GetEvolutionDiceUsingCount();
        UtilityMethod.SetTxtCustomTypeByID(64, count.ToString());
    }

    public void SetTxtUsingDiceCount()
    {
        // 주사위 사용 개수 
        var count = UtilityMethod.GetEvolutionDiceUsingCount();
        UtilityMethod.SetTxtCustomTypeByID(64, count.ToString());
    }


    void SetButtonEvents()
    {
        // 뽑기 게임 10 , 11 , 30회
        UtilityMethod.SetBtnEventCustomTypeByID(17, () => UnionLotteryGameStart(1));
        UtilityMethod.SetBtnEventCustomTypeByID(18, () => UnionLotteryGameStart(11));
        UtilityMethod.SetBtnEventCustomTypeByID(19, () => UnionLotteryGameStart(30));


        /* 진화전 */

        // 진화전 버튼
        UtilityMethod.SetBtnEventCustomTypeByID(20, () =>
        {
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnEvolutionMonsterChallenge);

            // 메뉴 판넬 숨김
            GlobalData.instance.uiController.AllDisableMenuPanels();

            // 진화전 버튼 선택 효과 숨김
            // EnableMenuPanel(MenuPanelType.evolution);

            // 진화전 버튼 비활성화
            EnableBtnEvolutionMonsterChange(false);

            // 진화전 포기 버튼 활성화
            // UtilityMethod.GetCustomTypeBtnByID(30).gameObject.SetActive(true);

        });

        // 진화 업그레이드 이펙트 확인 버튼
        UtilityMethod.SetBtnEventCustomTypeByID(23, () =>
        {
            // 기존 UI Canvas 활성화
            UtilityMethod.GetCustomTypeGMById(6).SetActive(true);
            // 진화 업그레이트 이펙트 비활성화
            GlobalData.instance.gradeAnimCont.gameObject.SetActive(false);
        });

        // 진화 주사위 뽑기 버튼
        UtilityMethod.SetBtnEventCustomTypeByID(22, () =>
        {
            if (evolutionSlots.Any(a => a.isUnlock == true))
                StartCoroutine(GlobalData.instance.evolutionDiceLotteryManager.RollEvolutionDice());
            else
                GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("", 10);
        });

        // 진화전 포기 버튼
        UtilityMethod.SetBtnEventCustomTypeByID(30, () =>
        {
            StartCoroutine(GlobalData.instance.eventController.ProcessEvolutionMonsterGiveUp());
        });

    }


    public void SetDiceEvolutionData(EvolutionDiceStatType statType, float statValue)
    {
        switch (statType)
        {
            case EvolutionDiceStatType.insectDamage: diceEvolutionData.insectDamage = statValue; break;
            case EvolutionDiceStatType.insectCriticalChance: diceEvolutionData.insectCriticalChance = statValue; break;
            case EvolutionDiceStatType.insectCriticalDamage: diceEvolutionData.insectCriticalDamage = statValue; break;
            case EvolutionDiceStatType.goldBonus: diceEvolutionData.goldBonus = statValue; break;
            case EvolutionDiceStatType.insectMoveSpeed: diceEvolutionData.insectMoveSpeed = statValue; break;
            case EvolutionDiceStatType.insectSpawnTime: diceEvolutionData.insectSpawnTime = statValue; break;
            case EvolutionDiceStatType.insectBossDamage: diceEvolutionData.insectBossDamage = statValue; break;
        }

        // set save data
        GlobalData.instance.saveDataManager.SetEvolutionInGameData(diceEvolutionData);
    }

    public float GetDiceEvolutionDataValueByStatType(EvolutionDiceStatType statType)
    {
        switch (statType)
        {
            case EvolutionDiceStatType.insectDamage: return diceEvolutionData.insectDamage;
            case EvolutionDiceStatType.insectCriticalChance: return diceEvolutionData.insectCriticalChance;
            case EvolutionDiceStatType.insectCriticalDamage: return diceEvolutionData.insectCriticalDamage;
            case EvolutionDiceStatType.goldBonus: return diceEvolutionData.goldBonus;
            case EvolutionDiceStatType.insectMoveSpeed: return diceEvolutionData.insectMoveSpeed;
            case EvolutionDiceStatType.insectSpawnTime: return diceEvolutionData.insectSpawnTime;
            case EvolutionDiceStatType.insectBossDamage: return diceEvolutionData.insectBossDamage;
            default: return 0;
        }
    }

    // 유니온 뽑기
    public void UnionLotteryGameStart(int roundCount)
    {
        //trLotteryGameSet.gameObject.SetActive(true);
        GlobalData.instance.lotteryManager.LotteryStart(roundCount, () =>
        {
            Debug.Log(roundCount + "회 뽑기 게임 종료 이벤트 실행");
        });
    }


    // 진화전 전투 시작 버튼
    public void EnableBtnEvolutionMonsterChange(bool value)
    {
        UtilityMethod.GetCustomTypeBtnByID(20).interactable = value;
    }

}
