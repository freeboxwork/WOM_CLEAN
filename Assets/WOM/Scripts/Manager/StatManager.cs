using System.Collections;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UIElements;
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
    float skill_InsectDamageUp = 0;
    float skill_UnionDamageUp = 0;
    float skill_AllUnitSpeedUp = 0;
    float skill_GoldBounsUp = 0;
    float skill_MonsterKing = 0;
    float skill_AllUnitCriticalChanceUp = 0;

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

    /// <summary> 곤충 공격력 </summary>
    public float GetInsectDamage(InsectType insectType)
    {
        var itd = GetEvolutionData(insectType).damage;
        var ttd = GetTraningData(SaleStatType.trainingDamage).value;
        var value = itd + ttd  + skill_InsectDamageUp;
        return value;
    }

    /// <summary> 곤충 치명타 확율 </summary>
    public float GetInsectCriticalChance(InsectType insectType)
    {
        var trcc = GetTraningData(SaleStatType.trainingCriticalChance).value;
        var tacc = GetTraningData(SaleStatType.talentCriticalChance).value;
        var icc = GetDnaData(DNAType.insectCriticalChance).power;
        var diceIcc = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectCriticalChance); 
        var value = trcc + tacc + icc + diceIcc + skill_AllUnitCriticalChanceUp;
        return value;
    }

    /// <summary> 곤충 치명타 공격력 </summary>
    public float GetInsectCriticalDamage(InsectType insectType)
    {
        var trcd = GetTraningData(SaleStatType.trainingCriticalDamage).value;
        var tacd = GetTraningData(SaleStatType.talentCriticalDamage).value;
        var icc = GetDnaData(DNAType.insectCriticalDamage).power;
        var diceIcd = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectCriticalDamage);
        var value = trcd + tacd + icc + diceIcd;
        return value;
    }

    /// <summary> 곤충 공격력 증가율 </summary>
    public float GetInsectTalentDamage(InsectType insectType)
    {
        var ttd = GetTraningData(SaleStatType.talentDamage).value;
        var upd = unionManager.GetAllUnionPassiveDamage();
        var did = GetDnaData(DNAType.insectDamage).power;
        var idr = GetEvolutionData(insectType).damageRate;
        var diceId = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectDamage);
        var value = ttd + upd + did + idr + diceId;
        return value;
    }

    /// <summary> 곤충 이동 속도 </summary>
    public float GetInsectMoveSpeed(InsectType insectType)
    {
        var ies = GetEvolutionData(insectType).speed;//150
        var tms = GetTraningData(SaleStatType.talentMoveSpeed).value;//5
        var ims = GetDnaData(DNAType.insectMoveSpeed).power;//1
        var diceIms = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectMoveSpeed);//50
        var value = ies * (1 + ((tms + ims + diceIms + skill_AllUnitSpeedUp) * 0.01f));
        //var value = ies + (ies * ((tms + ims + diceIms + skill_AllUnitSpeedUp) * 0.01f));
        // Debug.Log($"진화속도:{ies}/특성속도:{tms}/DNA속도:{ims}/주사위속도{diceIms} = 합계 : {value}");
        return value * 0.01f;
    }
    
    /// <summary> 곤충 생성 속도 </summary>
    public float GetInsectSpwanTime(InsectType insectType)
    {
        var ist = GetEvolutionData(insectType).spawnTime;
        var tst = GetTraningData(SaleStatType.talentSpawnSpeed).value;
        var diceIst = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectSpawnTime);
        var value = ist - (ist * (tst+ diceIst)) ;
        return value;
    }

    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region UNION

    /// <summary> 유니온 공격력 </summary>
    public float GetUnionDamage(int unionIndex)
    {
        var ud = GetUnionData(unionIndex).damage + skill_UnionDamageUp;
        return ud;
    }

    /// <summary> 유니온 이동속도 </summary>
    public float GetUnionMoveSpeed(int unionIndex)
    {
        var ums = GetUnionData(unionIndex).moveSpeed;
        var dms = GetDnaData(DNAType.insectMoveSpeed).power;
        var diceIms = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectMoveSpeed);
        var value = ums + (ums * ((dms + diceIms + skill_AllUnitSpeedUp) *0.01f)) ;
        return value * 0.01f;
    }

    /// <summary> 유니온 생성속도 </summary>
    public float GetUnionSpwanSpeed(int unionIndex)
    {
        var dst = GetDnaData(DNAType.unionSpawnTime).power; 
        return dst;
    }

    /// <summary> 유니온 공격력 증가율 </summary>
    public float GetUnionTalentDamage(int unionIndex)
    {
        var dud = GetDnaData(DNAType.unionDamage).power;
        return dud;
    }

    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region GOODS

    /// <summary> 골드 획득량 </summary>
    public float GetTalentGoldBonus()
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


    public IEnumerator EnableSkill_InsectDamageUP() 
    {
        var data = GetSkillData(SkillType.insectDamageUp);
        skill_InsectDamageUp = data.damage;
        data.isSkilUsing = true;

        float elapsedTime = 0.0f;
        var totalDuration = data.duaration * (1 + SkillDuration());
        while (elapsedTime < totalDuration)
        {
            elapsedTime += Time.deltaTime;
            data.skillLeftTime = totalDuration - elapsedTime;
            yield return null;
        }

        data.skillLeftTime = 0;
        skill_InsectDamageUp = 0;
        data.isSkilUsing = false;
    }

    public IEnumerator EnableSkill_UnionDamageUP()
    {
        var data = GetSkillData(SkillType.unionDamageUp);
        skill_UnionDamageUp = data.damage;
        data.isSkilUsing = true;

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            yield return null;
        }
        
        data.isSkilUsing = false;
        skill_UnionDamageUp = 0;
    }

    public IEnumerator EnableSkill_AllUnitSpeedUP()
    {
        var data = GetSkillData(SkillType.allUnitSpeedUp);
        skill_AllUnitSpeedUp = data.power;
        data.isSkilUsing = true;

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            yield return null;
        }

        data.isSkilUsing = false;
        skill_UnionDamageUp = 0;
    }

    public IEnumerator EnableSkill_GoldBonusUP()
    {
        var data = GetSkillData(SkillType.glodBonusUp);
        skill_GoldBounsUp = data.power;
        data.isSkilUsing = true;

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            yield return null;
        }

        data.isSkilUsing = false;
        skill_GoldBounsUp = 0;
    }

    public IEnumerator EnableSkill_MonsterKing()
    {
        var data = GetSkillData(SkillType.monsterKing);
        skill_MonsterKing = data.power;
        data.isSkilUsing = true;

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            yield return null;
        }

        data.isSkilUsing = false;
        skill_MonsterKing = 0;
    }

    public IEnumerator EnableSkill_AllUnitCriticalChanceUP()
    {
        var data = GetSkillData(SkillType.allUnitCriticalChanceUp);
        skill_AllUnitCriticalChanceUp = data.power;
        data.isSkilUsing = true;

        float elapsedTime = 0.0f;
        while (elapsedTime < data.duaration)
        {
            elapsedTime += Time.deltaTime;
            data.skillLeftTime = data.duaration - elapsedTime;
            yield return null;
        }

        data.isSkilUsing = false;
        skill_AllUnitCriticalChanceUp = 0;
    }


    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region 개별 DATA

    public float GoldPig()
    {
        return GetDnaData(DNAType.goldPig).power;
    }

    public float SkillDuration()
    {
        return GetDnaData(DNAType.skillDuration).power;
    }

    public float SkillCoolTime()
    {
        return GetDnaData(DNAType.skillCoolTime).power;
    }

    public float BossDamage()
    {
        return GetDnaData(DNAType.bossDamage).power;
    }

    public float MonsterHpLess()
    {
        return GetDnaData(DNAType.monsterHpLess).power;
    }

    public float BoneBonus()
    {
        return GetDnaData(DNAType.boneBonus).power;
    }

    public float GoldMonsterBonus()
    {
        return GetDnaData(DNAType.goldMonsterBonus).power;
    }

    public float OfflineBonus()
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
