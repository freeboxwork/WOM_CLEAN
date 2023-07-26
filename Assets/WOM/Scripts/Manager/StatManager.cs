using System.Collections;
using UnityEngine;
using static EnumDefinition;

/// <summary>
/// 모든 스탯값을 통합하여 관리함
/// </summary>
public class StatManager : MonoBehaviour
{
    DataManager dataManager;
    EvolutionManager evolutionManager;
    TraningManager traningManager;
    UnionManager unionManager;
    DNAManager dnaManager;
    SkillManager skillManager;

    //SKILL VALUES
    double skill_InsectDamageUp = 0;
    double skill_UnionDamageUp = 0;
    double skill_AllUnitSpeedUp = 0;
    double skill_GoldBounsUp = 0;
    double skill_MonsterKing = 0;
    double skill_AllUnitCriticalChanceUp = 0;

    //전환 효과로 스킬 UI 가 꺼질때 사용
    public bool transitionUI = false;

    public bool allUnitCriticalOn = false;

    #region STAT INFOMATION


    #endregion

    void Start()
    {


    }

    public IEnumerator Init()
    {
        yield return null;
        GetManagers();
    }

    void GetManagers()
    {
        dataManager = GlobalData.instance.dataManager;
        evolutionManager = GlobalData.instance.evolutionManager;
        traningManager = GlobalData.instance.traningManager;
        unionManager = GlobalData.instance.unionManager;
        dnaManager = GlobalData.instance.dnaManger;
        skillManager = GlobalData.instance.skillManager;
    }

    #region INSECT 

    /// <summary> 곤충 공격력 damage(한글) </summary>
    public double GetInsectDamage(InsectType insectType)
    {
        //진화 공격력
        var itd = GetEvolutionData(insectType).damage;
        //훈련 공격력
        var ttd = GetTraningData(SaleStatType.trainingDamage).value;

        //특성,DNA,진화추가능력,곤충타입에 따른 공격력 증가율
        var ittd = GetInsectTalentDamage(insectType);

        //공식 : (((진화 공격력 + 훈련 공격력) * (특성증가율)) * 스킬증가율) * 버프 증가율
        var value = (itd + ttd) * (1 + ((ittd + skill_InsectDamageUp) * 0.01f));
        //Debug.Log($"<color=green>{insectType}진화공격력:{itd}+{insectType}훈련공격력:{ttd}+{insectType}공격력증가율{(ittd)}+스킬공격력:{skill_InsectDamageUp}</color>");

        // ad buff 적용 ( damage )
        var buffValue = GlobalData.instance.adManager.GetBuffAdSlotByType(EnumDefinition.RewardTypeAD.adBuffDamage).addValue;
        //Debug.Log($"<color=red>버프가 적용된 {insectType}공격력:{value * (float)buffValue}</color>");

        return value * buffValue;
    }
    /// <summary> 곤충 공격력 증가율 damageRate</summary>
    double GetInsectTalentDamage(InsectType insectType)
    {
        var idr = GetEvolutionData(insectType).damageRate;
        var ttd = GetTraningData(SaleStatType.talentDamage).value;
        var upd = unionManager.GetAllUnionPassiveDamage();
        var did = GetDnaData(DNAType.insectDamage).power;
        var diceId = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectDamage);
        //Debug.Log($"{insectType}진화공격력증가율:{idr}+{insectType}훈련공격력증가율:{ttd}+유니온보유공격력증가율:{upd}+DNA공격력증가율:{did}+진화능력공격력증가율:{diceId}");

        var value = ttd + upd + did + idr + diceId;
        //Debug.Log($"총 공격력 증가율:{value}");

        return value;
    }
    /// <summary> 곤충 치명타 확율 Critical Chance</summary>
    public double GetInsectCriticalChance(InsectType insectType)
    {
        var trcc = GetTraningData(SaleStatType.trainingCriticalChance).value;//소숫점
        var idr = GetEvolutionData(insectType).criticalChance;
        var tacc = GetTraningData(SaleStatType.talentCriticalChance).value;//소숫점
        var icc = GetDnaData(DNAType.insectCriticalChance).power;//소숫점
        var diceIcc = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectCriticalChance);//소수점
        var value = trcc + tacc + icc + diceIcc + idr + skill_AllUnitCriticalChanceUp;//스킬은 정수
        //Debug.Log($"<color=blue>곤충치명타 확률 - 훈련:{trcc} 진화등급 : {idr} 특성 : {tacc} DNA : {icc} 진화추가능력 : {diceIcc} 총:{value}</color>");

        return value;
    }

    /// <summary> 곤충 치명타 공격력 Critical Damage</summary>
    public double GetInsectCriticalDamage(InsectType insectType)
    {
        var trcd = GetTraningData(SaleStatType.trainingCriticalDamage).value;//정수
        var tacd = GetTraningData(SaleStatType.talentCriticalDamage).value;//정수
        var icc = GetDnaData(DNAType.insectCriticalDamage).power;//정수
        var diceIcd = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectCriticalDamage);//정수
        var value = trcd + tacd + icc + diceIcd;

        //Debug.Log($"훈련{trcd} 특성{tacd} DNA{icc} 주사위{diceIcd} 곤충 치명타 총 공격력:{value}");
        return value;
    }

    /// <summary> 곤충 이동 속도 버프,스킬 포함 최대값450이며 200내에서 조정이 필요</summary>
    public double GetInsectMoveSpeed(InsectType insectType)
    {
        var ies = GetEvolutionData(insectType).speed;//정수
        var tms = GetTraningData(SaleStatType.talentMoveSpeed).value;//정수
        var ims = GetDnaData(DNAType.insectMoveSpeed).power;//정수
        var diceIms = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectMoveSpeed);//정수

        var value = ies + tms + ims + diceIms + skill_AllUnitSpeedUp;
        //Debug.Log($"이동 속도 : 기본:{ies}/특성:{tms}/DNA:{ims}/주사위{diceIms} = 합계 : {value}");

        // ad buff 적용 ( speed )
        var buffValue = GlobalData.instance.adManager.GetBuffAdSlotByType(EnumDefinition.RewardTypeAD.adBuffSpeed).addValue;
        return value * buffValue;
    }

    /// <summary> 곤충 생성 속도 </summary>
    public float GetInsectSpwanTime(InsectType insectType)
    {
        var ist = GetEvolutionData(insectType).spawnTime;//소수
        var tst = GetTraningData(SaleStatType.talentSpawnSpeed).value;//소수
        var diceIst = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectSpawnTime);//소수
        var value = ist - (tst + diceIst);
        //Debug.Log($"곤충 소환시간 : 기본{ist} - 특성{tst}주사위{diceIst} 최종{value}");
        return (float)value;
    }

    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region UNION

    /// <summary> 유니온 공격력 </summary>
    public double GetUnionDamage(int unionIndex)
    {

        //var ud = GetUnionData(unionIndex).damage + skill_UnionDamageUp;
        var ud = GetUnionData(unionIndex).damage; 
        var dms = GetDnaData(DNAType.unionDamage).power;
        var value = ud * (1 + ((dms + skill_UnionDamageUp)) * 0.01f);

         // ad buff 적용 ( damage )
        var buffValue = GlobalData.instance.adManager.GetBuffAdSlotByType(EnumDefinition.RewardTypeAD.adBuffDamage).addValue;
       


        Debug.Log($"유니온 공격력 : 기본:{ud}//DNA:{dms}= 합계 : {value}");

        return value * buffValue;
    }



    /// <summary> 유니온 이동속도 </summary>
    public float GetUnionMoveSpeed(int unionIndex)
    {
        var ums = GetUnionData(unionIndex).moveSpeed;
        var dms = GetDnaData(DNAType.insectMoveSpeed).power;
        //var diceIms = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectMoveSpeed);
        var value = ums + dms + skill_AllUnitSpeedUp;
        Debug.Log($"유니온 이동속도 : 기본:{ums}//DNA:{dms}= 합계 : {value}");

        return (float)value;
    }

    /// <summary> 유니온 생성속도 </summary>
    public double GetUnionSpwanSpeed(int unionIndex)
    {

        var ums = GetUnionData(unionIndex).spawnTime;
        var dst = GetDnaData(DNAType.unionSpawnTime).power;
        var value= ums - dst;
        Debug.Log($"유니온 스폰시간 : 기본:{ums}//DNA:{dst}= 합계 : {value}");

        return value;
    }






    // /// <summary> 유니온 공격력 증가율 </summary>
    // public double GetUnionTalentDamage(int unionIndex)
    // {
    //     var dud = GetDnaData(DNAType.unionDamage).power;
    //     return dud;
    // }

    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region GOODS

    /// <summary> 골드 획득량 </summary>
    public double GetTalentGoldBonus()
    {
        var dgb = GetDnaData(DNAType.glodBonus).power;
        var tgb = GetTraningData(SaleStatType.talentGoldBonus).value;
        var diceGb = GetEvolutionDiceValueByType(EvolutionDiceStatType.goldBonus);
        var value = dgb + tgb + diceGb + skill_GoldBounsUp;
        return value;
    }

    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region SKILLS


    public void UsingSkill(SkillType skillType)
    {
        switch (skillType)
        {
            case SkillType.insectDamageUp:
                StartCoroutine(EnableSkill_InsectDamageUP());
                break;
            case SkillType.unionDamageUp:
                StartCoroutine(EnableSkill_UnionDamageUP());
                break;
            case SkillType.allUnitSpeedUp:
                StartCoroutine(EnableSkill_AllUnitSpeedUP());
                break;
            case SkillType.glodBonusUp:
                StartCoroutine(EnableSkill_GoldBonusUP());
                break;
            case SkillType.monsterKing:
                StartCoroutine(EnableSkill_MonsterKing());
                break;
            case SkillType.allUnitCriticalChanceUp:
                StartCoroutine(EnableSkill_AllUnitCriticalChanceUP());
                break;
        }
    }

    public void SetTransitionUI(bool value)
    {
        transitionUI = value;
    }


    void SetUsingSkillSaveData(SkillType skillType, bool isUsing)
    {
        GlobalData.instance.saveDataManager.SetSkillUsingValue(skillType, isUsing);
    }

    void SetLeftSkillTimeSaveData(SkillType skillType, float leftTime)
    {
        // 추 후 필요하면 추가 할 것
        //GlobalData.instance.saveDataManager.SetSkillLeftTime(skillType, leftTime);
    }

    public IEnumerator EnableSkill_InsectDamageUP()
    {
        var data = GetSkillData(SkillType.insectDamageUp);
        skill_InsectDamageUp = data.power;
        data.isSkilUsing = true;

        // set save data
        var skilType = SkillType.insectDamageUp;
        SetUsingSkillSaveData(skilType, true);



        float elapsedTime = 0.0f;
        var totalDuration = data.duaration * (1 + SkillDuration());
        while (elapsedTime < totalDuration)
        {
            // ui 전환 효과가 발동되면 대기
            yield return new WaitUntil(() => transitionUI == false);

            elapsedTime += Time.deltaTime;
            data.skillLeftTime = totalDuration - elapsedTime;
            // set save data
            //SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
            yield return null;
        }


        data.skillLeftTime = 0;
        skill_InsectDamageUp = 0;
        data.isSkilUsing = false;

        // set save data
        // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
        SetUsingSkillSaveData(skilType, false);

    }

    public IEnumerator EnableSkill_UnionDamageUP()
    {
        var data = GetSkillData(SkillType.unionDamageUp);
        skill_UnionDamageUp = data.power;
        data.isSkilUsing = true;

        // set save data
        var skilType = SkillType.insectDamageUp;
        SetUsingSkillSaveData(skilType, true);



        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            // ui 전환 효과가 발동되면 대기
            yield return new WaitUntil(() => transitionUI == false);

            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            // set save data
            // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
            yield return null;
        }

        data.isSkilUsing = false;
        skill_UnionDamageUp = 0;

        // set save data
        // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
        SetUsingSkillSaveData(skilType, false);




    }



    public IEnumerator EnableSkill_AllUnitSpeedUP()
    {
        var data = GetSkillData(SkillType.allUnitSpeedUp);
        skill_AllUnitSpeedUp = data.power;
        data.isSkilUsing = true;

        // set save data
        var skilType = SkillType.insectDamageUp;
        SetUsingSkillSaveData(skilType, true);

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            // ui 전환 효과가 발동되면 대기
            yield return new WaitUntil(() => transitionUI == false);

            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            // set save data
            // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
            yield return null;
        }

        data.isSkilUsing = false;
        skill_UnionDamageUp = 0;

        // set save data
        // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
        SetUsingSkillSaveData(skilType, false);
    }

    public IEnumerator EnableSkill_GoldBonusUP()
    {
        var data = GetSkillData(SkillType.glodBonusUp);
        skill_GoldBounsUp = data.power;
        data.isSkilUsing = true;

        // set save data
        var skilType = SkillType.insectDamageUp;
        SetUsingSkillSaveData(skilType, true);

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            // ui 전환 효과가 발동되면 대기
            yield return new WaitUntil(() => transitionUI == false);

            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            // set save data
            // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
            yield return null;
        }

        data.isSkilUsing = false;
        skill_GoldBounsUp = 0;

        // set save data
        // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
        SetUsingSkillSaveData(skilType, false);

    }

    public IEnumerator EnableSkill_MonsterKing()
    {
        var data = GetSkillData(SkillType.monsterKing);
        skill_MonsterKing = data.power;
        data.isSkilUsing = true;

        // set save data
        var skilType = SkillType.insectDamageUp;
        SetUsingSkillSaveData(skilType, true);

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            // ui 전환 효과가 발동되면 대기
            yield return new WaitUntil(() => transitionUI == false);

            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            // set save data
            // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
            yield return null;
        }

        data.isSkilUsing = false;
        skill_MonsterKing = 0;

        // set save data
        // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
        SetUsingSkillSaveData(skilType, false);
    }

    public IEnumerator EnableSkill_AllUnitCriticalChanceUP()
    {

        allUnitCriticalOn = true;
        Debug.Log("EnableSkill_AllUnitCriticalChanceUP");

        var data = GetSkillData(SkillType.allUnitCriticalChanceUp);
        skill_AllUnitCriticalChanceUp = data.power;
        data.isSkilUsing = true;

        // set save data
        var skilType = SkillType.insectDamageUp;
        SetUsingSkillSaveData(skilType, true);

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            // ui 전환 효과가 발동되면 대기
            yield return new WaitUntil(() => transitionUI == false);

            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            // set save data
            // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
            yield return null;
        }


        data.isSkilUsing = false;
        skill_AllUnitCriticalChanceUp = 0;

        // set save data
        // SetLeftSkillTimeSaveData(skilType, data.skillLeftTime);
        SetUsingSkillSaveData(skilType, false);

        allUnitCriticalOn = false;

    }


    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region 개별 DATA

    public float GoldPig()
    {
        return (float)GetDnaData(DNAType.goldPig).power;
    }

    public float SkillDuration()
    {
        return (float)GetDnaData(DNAType.skillDuration).power;
    }

    public float SkillCoolTime()
    {
        return (float)GetDnaData(DNAType.skillCoolTime).power;
    }

    public double BossDamage()
    {
        return GetDnaData(DNAType.bossDamage).power;
    }

    public double MonsterHpLess()
    {
        return GetDnaData(DNAType.monsterHpLess).power;
    }

    public double BoneBonus()
    {
        return GetDnaData(DNAType.boneBonus).power;
    }

    public double GoldMonsterBonus()
    {
        return GetDnaData(DNAType.goldMonsterBonus).power;
    }

    public double OfflineBonus()
    {
        return GetDnaData(DNAType.offlineBonus).power;
    }


    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region UTILITY METHOD
    EvolutionData GetEvolutionData(InsectType insectType)
    {
        // Debug.Log(insectType);
        return dataManager.GetEvolutionDataById(insectType, evolutionManager.evalutionLeveldx);
    }

    TraningInGameData GetTraningData(SaleStatType saleStatType)
    {
        return traningManager.GetTraningInGameData(saleStatType);
    }

    DNAInGameData GetDnaData(DNAType dnaType)
    {
        return dnaManager.GetDNAInGameData(dnaType);
    }

    UnionInGameData GetUnionData(int unionIndex)
    {
        return unionManager.GetUnionInGameDataByID(unionIndex);
    }
    Skill_InGameData GetSkillData(SkillType skillType)
    {
        return skillManager.GetSkillInGameDataByType(skillType);
    }

    float GetEvolutionDiceValueByType(EvolutionDiceStatType statType)
    {
        return evolutionManager.GetDiceEvolutionDataValueByStatType(statType);
    }


    #endregion

}
