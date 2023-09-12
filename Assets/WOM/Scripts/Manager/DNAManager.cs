using ProjectGraphics;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static UtilityMethod;


public class DNAManager : MonoBehaviour
{
    public LotteryAnimationController lotteryAnimCont;
    public List<DNASlot> dnaSlots = new List<DNASlot>();
    public List<Sprite> dnaIconImages = new List<Sprite>();
    const int btnLottery30 = 29;
    const int btnLottery10 = 28;
    const int btnLottery01 = 27;
    bool isGambling = false;
    void Start()
    {
        SetBtnEvent();
    }
    void SetBtnEvent()
    {
        // 뽑기버튼 3종
        // 27 : 1회 , 28: 11회 , 29 : 33회

        SetBtnEventCustomTypeByID(27, () =>
        {
            DNALotteryGameStart(1, 100, EnumDefinition.RewardType.gem);
        });

        SetBtnEventCustomTypeByID(28, () =>
        {
            DNALotteryGameStart(11, 1000, EnumDefinition.RewardType.gem);
        });

        SetBtnEventCustomTypeByID(29, () =>
        {
            DNALotteryGameStart(33, 3000, EnumDefinition.RewardType.gem);
        });
    }

    public void DNALotteryGameStart(int gameCount, int payValue, EnumDefinition.RewardType rewardType)
    {
        if (isGambling == false)
        {
            if (GetLotteryDNATypes().Count <= 0)
            {
                // message popup (더이상 뽑을 수 없습니다,모든 DNA 레벨이 MAX 상태에 도달 했습니다.)
                GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 21);
            }
            else
            {
                Lottery_Start(gameCount, payValue, rewardType);
            }
        }
    }


    public IEnumerator Init()
    {


        // SET SLOT
        for (int i = 0; i < dnaSlots.Count; i++)
        {
            int index = i;
            var slot = dnaSlots[index];
            var data = GlobalData.instance.dataManager.GetDNADataById(index);
            slot.inGameData = new DNAInGameData();

            var type = (EnumDefinition.DNAType)index;
            // set type
            slot.SetDnaType(type);

            // 저장된 데이터에서 불러와야 함
            // set in game data (  )
            var saveData = GlobalData.instance.saveDataManager.GetSaveDataDNA(type);

            slot.inGameData.level = saveData.level;
            slot.inGameData.power = (saveData.level * data.power);


            slot.inGameData.maxLevel = data.maxLevel;
            slot.inGameData.name = data.dnaName;
            slot.inGameData.dataPower = data.power;

            // set ui
            slot.SetTxtName(data.dnaName);
            slot.SetTxtMaxLevel(data.maxLevel);
            slot.SetTxtInfo(data.infoFront, StaticDefine.COLOR_GREEN, slot.inGameData.power, data.infoBack);
            slot.SetTxtHasCount(slot.inGameData.level, slot.inGameData.maxLevel);
            slot.SetFace(GetDnaIconImage(data.spriteName));

        }
        yield return null;
        // 뽑기버튼 비활성화
        EnableValidButtons();
    }


    DNASlot GetSlotByDNAType(EnumDefinition.DNAType type)
    {
        return dnaSlots.FirstOrDefault(f => f.DNAType == type);
    }

    public DNAInGameData GetDNAInGameData(EnumDefinition.DNAType type)
    {
        return GetSlotByDNAType(type).inGameData;
    }


    public void Lottery_Start(int roundCount, int payValue, EnumDefinition.RewardType rewardType)
    {
        if (lotteryAnimCont.toggleRepeatGame.isOn)
            StartCoroutine(RepeatGame(roundCount, payValue, rewardType));
        else
            StartCoroutine(LotteryStart(roundCount, payValue, rewardType));
    }

    IEnumerator RepeatGame(int roundCount, int payValue, EnumDefinition.RewardType rewardType)
    {
        while (lotteryAnimCont.toggleRepeatGame.isOn)
        {
            yield return StartCoroutine(LotteryStart(roundCount, payValue, rewardType));
            yield return new WaitForSeconds(0.5f);
        }
    }
    // DNA 뽑기
    IEnumerator LotteryStart(int gameCount, int payValue, EnumDefinition.RewardType rewardType)
    {
        if (IsValidGemCount(payValue, rewardType))
        {
            isGambling = true;

            if (rewardType == EnumDefinition.RewardType.dnaTicket)
            {
                // pay dna ticket
                GlobalData.instance.player.PayDnaTicket(payValue);
            }
            else
            {
                // pay gem
                GlobalData.instance.player.PayGem(payValue);
            }

            // 랜덤하게 뽑은 DNA TYPES ( UI 리셋할때 활용, 중복제외 ) 
            List<EnumDefinition.DNAType> dnaTypes = new List<EnumDefinition.DNAType>();

            // 연출을 위한 DNA TYPES ( 중복 포함 )
            List<EnumDefinition.DNAType> dnaEffectTypes = new List<EnumDefinition.DNAType>();

            // 다시 뽑기 버튼 비활성화
            UtilityMethod.SetBtnsInteractableEnable(new List<int> { 27, 28, 29, 44 }, false);


            for (int i = 0; i < gameCount; i++)
            {
                var lotteryTypes = GetLotteryDNATypes();
                var randomType = GetRandomType(lotteryTypes);
                var slot = GetSlotByDNAType(randomType);
                slot.inGameData.LevelUp();

                // set save data
                GlobalData.instance.saveDataManager.SetLevelDNAByType(randomType, slot.inGameData);

                lotteryTypes.Add(randomType);
                dnaEffectTypes.Add(randomType);

                if (!dnaTypes.Contains(randomType))
                    dnaTypes.Add(randomType);

                // 연출 이펙트 등장
                var result = $"뽑은 유전자 : {slot.inGameData.name} 유전자 레벨 : {slot.inGameData.level}";
                Debug.Log("[ 연출이펙트 ] " + result);
                yield return null;
            }

            // UI RESET
            foreach (var type in dnaTypes)
                ResetUI(type);

            // 연출 등장
            lotteryAnimCont.gameObject.SetActive(true);
            EnableLotteryBtnsSet(EnumDefinition.LotteryPageType.DNA);

            yield return new WaitForEndOfFrame();

            yield return StartCoroutine(lotteryAnimCont.ShowDNAIconSlotCardOpenProcess(GetTypeListToInt(dnaEffectTypes)));

            // 다시 뽑기 버튼 활성화
            UtilityMethod.SetBtnsInteractableEnable(new List<int> { 27, 28, 29, 44 }, true);


            // 뽑기버튼 비활성화
            EnableValidButtons();
            yield return new WaitForSeconds(0.2f);

        }
        else
        {
            // message popup (보석이 부족합니다) -> 재화가 부족 합니다.
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 3);
            StopAllCoroutines();
            yield break;
        }


        yield return new WaitForEndOfFrame();

        isGambling = false;
    }

    void EnableLotteryBtnsSet(EnumDefinition.LotteryPageType lotteryPageType)
    {
        var unionPage = lotteryPageType == EnumDefinition.LotteryPageType.UNION;
        UtilityMethod.GetCustomTypeGMById(8).SetActive(unionPage);
        UtilityMethod.GetCustomTypeGMById(9).SetActive(!unionPage);
    }
    public void SetCustomLevel(EnumDefinition.DNAType type, int level)
    {
        var slot = GetSlotByDNAType(type);
        slot.inGameData.CustomLevel(level);

        // set save data
        //GlobalData.instance.saveDataManager.SetLevelDNAByType(type, slot.inGameData);

        ResetUI(type);
    }

    int[] GetTypeListToInt(List<EnumDefinition.DNAType> types)
    {
        int[] values = new int[types.Count];
        for (int i = 0; i < types.Count; i++)
        {
            values[i] = (int)types[i];
        }

        foreach (var t in values)
            Debug.Log(t);

        return values;
    }



    void ResetUI(EnumDefinition.DNAType type)
    {
        var data = GlobalData.instance.dataManager.dnaDatas.data[(int)type];
        var slot = GetSlotByDNAType(type);

        slot.SetTxtInfo(data.infoFront, StaticDefine.COLOR_GREEN, slot.inGameData.power, data.infoBack);
        slot.SetTxtHasCount(slot.inGameData.level, slot.inGameData.maxLevel);
    }

    /// <summary> DNA LEVEL이 MAX LEVEL에 도달한 DNA 타입을 제외하고 반환한다 </summary>
    List<EnumDefinition.DNAType> GetLotteryDNATypes()
    {
        List<EnumDefinition.DNAType> types = new List<EnumDefinition.DNAType>();
        foreach (var slot in dnaSlots)
        {
            if (slot.inGameData.level < slot.inGameData.maxLevel)
                types.Add(slot.DNAType);
        }
        return types;
    }

    EnumDefinition.DNAType GetRandomType(List<EnumDefinition.DNAType> types)
    {

        Debug.Log("count!!! " + types.Count);
        var randomIndex = Random.Range(0, types.Count);
        return types[randomIndex];
    }



    void EnableValidButtons()
    {
        // 보유 dns의 총 합을 계산 하여 남은 개수가 버튼이 요구하는 개수 보다 많을때 버튼 활성화    
        var dnaLeftCount = LeftDnaCount();
        if (dnaLeftCount < 30)
            DisableLotteryBtn(btnLottery30);
        if (dnaLeftCount < 10)
            DisableLotteryBtn(btnLottery10);
        if (dnaLeftCount <= 0)
            DisableLotteryBtn(btnLottery01);
    }

    void DisableLotteryBtn(int btnId)
    {
        GetCustomTypeBtnByID(btnId).interactable = false;
    }


    bool IsValidGemCount(int payValue, EnumDefinition.RewardType rewardType)
    {
        var goods = GlobalData.instance.player.GetGoodsByRewardType(rewardType);
        return goods >= payValue;
    }

    // 보유중인 전체 DNA 수
    int GetTotalDnaCount()
    {
        int total = 0;
        foreach (var slot in dnaSlots)
        {
            total += slot.inGameData.level;
        }
        return total;
    }

    // 최대 뽑기 수
    int GetTotalDnaMaxCount()
    {
        int maxCount = 0;
        foreach (var data in GlobalData.instance.dataManager.dnaDatas.data)
        {
            maxCount += data.maxLevel;
        }
        return maxCount;
    }

    // 뽑기 가능 횟수
    int LeftDnaCount()
    {
        var dnaTotal = GetTotalDnaCount();
        var maxCount = GetTotalDnaMaxCount();
        return maxCount - dnaTotal;

    }

    Sprite GetDnaIconImage(string spriteName)
    {
        return dnaIconImages.FirstOrDefault(f => f.name == spriteName);
    }
}
