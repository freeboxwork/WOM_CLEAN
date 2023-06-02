using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumDefinition;

public class EvolutionDiceLotteryManager : MonoBehaviour
{

    /* 진화 주사위 뽑기 프로세스
    0. 수사위 Lock 풀려 있는 개수대로 x 10 + (1회당 주사위 10개)
    1. grede 확률 뽑기
    2. grade 데이터 불러 오기
    3. 데이터에서 랜덤하게 능력치 뽑기
    4. 뽑아온 능력치 적용
     */

    // 랜덤 가중치.
    float[] weightValues;
    RewardDiceEvolutionDatas rewardData;
    
    // 주사위 굴리고 있는지 판단
    bool rollDice = false;

    public Sprite[] symbols;

    void Start()
    {
        
    }

    /// <summary> 뽑기 필요 데이터 세팅</summary>
    public IEnumerator Init()
    {
        rewardData = GlobalData.instance.dataManager.rewardDiceEvolutionDatas;
        
        // 랜덤 가중치 설정
        SetWeightValues();
        yield return new WaitForEndOfFrame();  
    }

    void SetWeightValues()
    {
        var dataCount = rewardData.data.Count;
        weightValues = new float[dataCount];
        for (int i = 0; i < dataCount; i++)
        {
            weightValues[i] = rewardData.data[i].gradeProbability;
        }
    }


    public IEnumerator RollEvolutionDice()
    {

        if(rollDice == false)
        {
            rollDice = true;

            var slots = GlobalData.instance.evolutionManager.evolutionSlots;
            var isPervGradeS = IsContainsSgrade(slots);
            if (isPervGradeS)
            {
                btnApply = false;
                btnCancel = false;

                // 팝업 실행
                GlobalData.instance.globalPopupController.EnableMessageTwoBtnPopup(11, Apply, Cancel);

                // 팝업 버튼 클릭 대기
                yield return new WaitUntil(() => btnApply || btnCancel);

                if (isPervGradeS && btnCancel)
                {
                    rollDice = false;
                    yield break;
                }

            }
            
            
            
            for (int i = 0; i < slots.Count; i++)
            {
                var slot = slots[i];
                if (slot.isUnlock && slot.statOpend)
                {
                  yield return  StartCoroutine(DiceRoll(slot));
                }
            }

            rollDice = false;
        }

        yield return null;
    }

    bool IsContainsSgrade(List<EvolutionSlot> slots)
    {
        return slots.Any(a => a.GetEvolutionRewardGrade() == EvolutionRewardGrade.S && a.isUnlock == true);
    }


    // 주사위 굴리기

    void Apply()
    {
        btnApply = true;
        Debug.Log("btn apply " + btnApply);
    }

    void Cancel()
    {
        btnCancel = true;
        Debug.Log("btn cancel" + btnCancel);
    }
    bool btnApply = false;
    bool btnCancel = false;
    IEnumerator DiceRoll(EvolutionSlot slot)
    {
        yield return null;

        // 현재 주사위 사용 개수 판단
        // 기본 10 + (10 X unlock count)
        var usingDice = UtilityMethod.GetEvolutionDiceUsingCount();

        // 주사위 개수 충분한지 판단
        if (IsReadyDiceCount(usingDice))
        {
            //bool isPervGradeS = slot.GetEvolutionRewardGrade() == EvolutionRewardGrade.S;
            //// 기존 Grade 가 S등급일 경우 팝업창 띄워서 더 진행 할것인지 확인.
            //if (isPervGradeS)
            //{
            //    btnApply = false;
            //    btnCancel = false;

            //    // 팝업 실행
            //    GlobalData.instance.globalPopupController.EnableMessageTwoBtnPopup(11, Apply, Cancel);

            //    // 팝업 버튼 클릭 대기
            //    yield return new WaitUntil(() => btnApply || btnCancel);

            //    if (isPervGradeS && btnCancel) yield break;
            //}

            // 주사위 사용
            GlobalData.instance.player.PayDice(usingDice);

            // 남은 주사위 UI 표시
            UtilityMethod.SetTxtCustomTypeByID(65, GlobalData.instance.player.diceCount.ToString());

            // 랜덤 그레이드 뽑기
            var randomGradeData = GetRandomWeightEvolutionGradeData();

            // SET GRADE
            slot.SetEvolutionRewardGrade((EvolutionRewardGrade)randomGradeData.grade);

            // SET SLOT UI
            slot.SetSymbol(symbols[randomGradeData.grade]);
            //slot.SetGradeTxtColor(randomGradeData.gradeColor);

            // 랜덤 능력치 뽑기
            var statValue = GetRandomStatValue(randomGradeData);

            Debug.Log($"랜덤하게 뽑은 능력치값 :{randomStatType.ToString()} / {statValue}");

            // 능력치 적용
            GlobalData.instance.evolutionManager.SetDiceEvolutionData(randomStatType, statValue);

            // UI TEXT 적용
            GlobalData.instance.evolutionManager.SetEvolutuinSlotName(randomStatType, slot, statValue, randomGradeData.gradeColor);
            
            

        }
        else
        {
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 1);
        }

       
    }



    bool IsReadyDiceCount(int usingDiceCount)
    {
        return GlobalData.instance.player.diceCount >  usingDiceCount;
    }

    RewardDiceEvolutionData GetRandomWeightEvolutionGradeData()
    {
        int grade = (int)UtilityMethod.GetWeightRandomValue(weightValues);
        return GlobalData.instance.dataManager.GetRewardDiceEvolutionDataByGradeId(grade + 1);
    }

    EvolutionDiceStatType randomStatType;
    EvolutionDiceStatType GetRandomStatType()
    {
        var randomValue = Random.Range(0, 7);
        return (EvolutionDiceStatType)randomValue;
    }

    float GetRandomStatValue (RewardDiceEvolutionData data)
    {
        var statType = GetRandomStatType();
        randomStatType = statType;
        switch (statType)
        {
            case EvolutionDiceStatType.insectDamage: return data.insectDamage;
            case EvolutionDiceStatType.insectCriticalChance: return data.insectCriticalChance;
            case EvolutionDiceStatType.insectCriticalDamage: return data.insectCriticalDamage;
            case EvolutionDiceStatType.goldBonus: return data.goldBonus;
            case EvolutionDiceStatType.insectMoveSpeed: return data.insectMoveSpeed;
            case EvolutionDiceStatType.insectSpawnTime: return data.insectSpawnTime;
            case EvolutionDiceStatType.insectBossDamage: return data.insectBossDamage;
        }
        return 0;
    }

    //void PrintGrade()
    //{

    //    var value = $"grade : {data.grade} / probability :{data.gradeProbability}";
    //    Debug.Log(value);
    //}
}

