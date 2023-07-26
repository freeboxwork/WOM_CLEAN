using System.Collections;
using UnityEngine;
using static EnumDefinition;

/// <summary>
/// ��� ���Ȱ��� �����Ͽ� ������
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

    //��ȯ ȿ���� ��ų UI �� ������ ���
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

    /// <summary> ���� ���ݷ� damage(�ѱ�) </summary>
    public double GetInsectDamage(InsectType insectType)
    {
        //��ȭ ���ݷ�
        var itd = GetEvolutionData(insectType).damage;
        //�Ʒ� ���ݷ�
        var ttd = GetTraningData(SaleStatType.trainingDamage).value;

        //Ư��,DNA,��ȭ�߰��ɷ�,����Ÿ�Կ� ���� ���ݷ� ������
        var ittd = GetInsectTalentDamage(insectType);

        //���� : (((��ȭ ���ݷ� + �Ʒ� ���ݷ�) * (Ư��������)) * ��ų������) * ���� ������
        var value = (itd + ttd) * (1 + ((ittd + skill_InsectDamageUp) * 0.01f));
        //Debug.Log($"<color=green>{insectType}��ȭ���ݷ�:{itd}+{insectType}�Ʒð��ݷ�:{ttd}+{insectType}���ݷ�������{(ittd)}+��ų���ݷ�:{skill_InsectDamageUp}</color>");

        // ad buff ���� ( damage )
        var buffValue = GlobalData.instance.adManager.GetBuffAdSlotByType(EnumDefinition.RewardTypeAD.adBuffDamage).addValue;
        //Debug.Log($"<color=red>������ ����� {insectType}���ݷ�:{value * (float)buffValue}</color>");

        return value * buffValue;
    }
    /// <summary> ���� ���ݷ� ������ damageRate</summary>
    double GetInsectTalentDamage(InsectType insectType)
    {
        var idr = GetEvolutionData(insectType).damageRate;
        var ttd = GetTraningData(SaleStatType.talentDamage).value;
        var upd = unionManager.GetAllUnionPassiveDamage();
        var did = GetDnaData(DNAType.insectDamage).power;
        var diceId = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectDamage);
        //Debug.Log($"{insectType}��ȭ���ݷ�������:{idr}+{insectType}�Ʒð��ݷ�������:{ttd}+���Ͽº������ݷ�������:{upd}+DNA���ݷ�������:{did}+��ȭ�ɷ°��ݷ�������:{diceId}");

        var value = ttd + upd + did + idr + diceId;
        //Debug.Log($"�� ���ݷ� ������:{value}");

        return value;
    }
    /// <summary> ���� ġ��Ÿ Ȯ�� Critical Chance</summary>
    public double GetInsectCriticalChance(InsectType insectType)
    {
        var trcc = GetTraningData(SaleStatType.trainingCriticalChance).value;//�Ҽ���
        var idr = GetEvolutionData(insectType).criticalChance;
        var tacc = GetTraningData(SaleStatType.talentCriticalChance).value;//�Ҽ���
        var icc = GetDnaData(DNAType.insectCriticalChance).power;//�Ҽ���
        var diceIcc = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectCriticalChance);//�Ҽ���
        var value = trcc + tacc + icc + diceIcc + idr + skill_AllUnitCriticalChanceUp;//��ų�� ����
        //Debug.Log($"<color=blue>����ġ��Ÿ Ȯ�� - �Ʒ�:{trcc} ��ȭ��� : {idr} Ư�� : {tacc} DNA : {icc} ��ȭ�߰��ɷ� : {diceIcc} ��:{value}</color>");

        return value;
    }

    /// <summary> ���� ġ��Ÿ ���ݷ� Critical Damage</summary>
    public double GetInsectCriticalDamage(InsectType insectType)
    {
        var trcd = GetTraningData(SaleStatType.trainingCriticalDamage).value;//����
        var tacd = GetTraningData(SaleStatType.talentCriticalDamage).value;//����
        var icc = GetDnaData(DNAType.insectCriticalDamage).power;//����
        var diceIcd = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectCriticalDamage);//����
        var value = trcd + tacd + icc + diceIcd;

        //Debug.Log($"�Ʒ�{trcd} Ư��{tacd} DNA{icc} �ֻ���{diceIcd} ���� ġ��Ÿ �� ���ݷ�:{value}");
        return value;
    }

    /// <summary> ���� �̵� �ӵ� ����,��ų ���� �ִ밪450�̸� 200������ ������ �ʿ�</summary>
    public double GetInsectMoveSpeed(InsectType insectType)
    {
        var ies = GetEvolutionData(insectType).speed;//����
        var tms = GetTraningData(SaleStatType.talentMoveSpeed).value;//����
        var ims = GetDnaData(DNAType.insectMoveSpeed).power;//����
        var diceIms = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectMoveSpeed);//����

        var value = ies + tms + ims + diceIms + skill_AllUnitSpeedUp;
        //Debug.Log($"�̵� �ӵ� : �⺻:{ies}/Ư��:{tms}/DNA:{ims}/�ֻ���{diceIms} = �հ� : {value}");

        // ad buff ���� ( speed )
        var buffValue = GlobalData.instance.adManager.GetBuffAdSlotByType(EnumDefinition.RewardTypeAD.adBuffSpeed).addValue;
        return value * buffValue;
    }

    /// <summary> ���� ���� �ӵ� </summary>
    public float GetInsectSpwanTime(InsectType insectType)
    {
        var ist = GetEvolutionData(insectType).spawnTime;//�Ҽ�
        var tst = GetTraningData(SaleStatType.talentSpawnSpeed).value;//�Ҽ�
        var diceIst = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectSpawnTime);//�Ҽ�
        var value = ist - (tst + diceIst);
        //Debug.Log($"���� ��ȯ�ð� : �⺻{ist} - Ư��{tst}�ֻ���{diceIst} ����{value}");
        return (float)value;
    }

    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region UNION

    /// <summary> ���Ͽ� ���ݷ� </summary>
    public double GetUnionDamage(int unionIndex)
    {

        //var ud = GetUnionData(unionIndex).damage + skill_UnionDamageUp;
        var ud = GetUnionData(unionIndex).damage; 
        var dms = GetDnaData(DNAType.unionDamage).power;
        var value = ud * (1 + ((dms + skill_UnionDamageUp)) * 0.01f);

         // ad buff ���� ( damage )
        var buffValue = GlobalData.instance.adManager.GetBuffAdSlotByType(EnumDefinition.RewardTypeAD.adBuffDamage).addValue;
       


        Debug.Log($"���Ͽ� ���ݷ� : �⺻:{ud}//DNA:{dms}= �հ� : {value}");

        return value * buffValue;
    }



    /// <summary> ���Ͽ� �̵��ӵ� </summary>
    public float GetUnionMoveSpeed(int unionIndex)
    {
        var ums = GetUnionData(unionIndex).moveSpeed;
        var dms = GetDnaData(DNAType.insectMoveSpeed).power;
        //var diceIms = GetEvolutionDiceValueByType(EvolutionDiceStatType.insectMoveSpeed);
        var value = ums + dms + skill_AllUnitSpeedUp;
        Debug.Log($"���Ͽ� �̵��ӵ� : �⺻:{ums}//DNA:{dms}= �հ� : {value}");

        return (float)value;
    }

    /// <summary> ���Ͽ� �����ӵ� </summary>
    public double GetUnionSpwanSpeed(int unionIndex)
    {

        var ums = GetUnionData(unionIndex).spawnTime;
        var dst = GetDnaData(DNAType.unionSpawnTime).power;
        var value= ums - dst;
        Debug.Log($"���Ͽ� �����ð� : �⺻:{ums}//DNA:{dst}= �հ� : {value}");

        return value;
    }






    // /// <summary> ���Ͽ� ���ݷ� ������ </summary>
    // public double GetUnionTalentDamage(int unionIndex)
    // {
    //     var dud = GetDnaData(DNAType.unionDamage).power;
    //     return dud;
    // }

    #endregion


    /*---------------------------------------------------------------------------------------------------------------*/


    #region GOODS

    /// <summary> ��� ȹ�淮 </summary>
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
        // �� �� �ʿ��ϸ� �߰� �� ��
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
            // ui ��ȯ ȿ���� �ߵ��Ǹ� ���
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
            // ui ��ȯ ȿ���� �ߵ��Ǹ� ���
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
            // ui ��ȯ ȿ���� �ߵ��Ǹ� ���
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
            // ui ��ȯ ȿ���� �ߵ��Ǹ� ���
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
            // ui ��ȯ ȿ���� �ߵ��Ǹ� ���
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
            // ui ��ȯ ȿ���� �ߵ��Ǹ� ���
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


    #region ���� DATA

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
