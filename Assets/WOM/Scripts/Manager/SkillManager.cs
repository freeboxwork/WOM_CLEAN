using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using static EnumDefinition;

public class SkillManager : MonoBehaviour
{
    public List<SkilSlot> skillSlots = new List<SkilSlot>();
    public List<Skill_InGameData> skill_InGameDatas = new List<Skill_InGameData>();
    public List<SkillBtn> skillBtns = new List<SkillBtn>();

    void Start()
    {

    }

    public void UnLockSkillButton(SkillType skillType, int level)
    {
        var skillData = GetSkillData(skillType, level);
        var btn = skillBtns[(int)skillType];
        btn.SetTxtCoolTime((int)skillData.coolTime);
        btn.SetTxtLevel(level);
        btn.gameObject.SetActive(true);
        btn.skillReady = true;
    }

    public IEnumerator Init()
    {
        SetSkillInGameData();
        yield return new WaitForEndOfFrame();
        SetSlotUI();
        EnableBuyButtons();

        yield return null;
    }

    void SetSkillInGameData()
    {
        foreach (SkillType type in System.Enum.GetValues(typeof(SkillType)))
        {
            Skill_InGameData inGameData = new Skill_InGameData();
            inGameData.skillType = type;

            //저장된 값에서 불러옴
            var saveData = GlobalData.instance.saveDataManager.GetSaveDataSkill(type);
            inGameData.level = saveData.level;
            inGameData.isSkilUsing = saveData.isUsingSkill;
            //data.skillLeftTime = saveData.leftSkillTime;

            var skillData = GetSkillData(type, inGameData.level);


            inGameData.coolTime = skillData.coolTime;
            inGameData.skilName = skillData.name;

            //data = saveData.skill_InGameData;

            inGameData.power = GetInitPowerValue(skillData, inGameData);
            inGameData.damage = GetDamangeValue(inGameData.power);
            inGameData.duaration = GetInitDurationValue(skillData, inGameData);
            //inGamedata.damage = saveData.damage; // 계산식 필요
            //inGamedata.power = saveData.power; // 계산식 필요

            skill_InGameDatas.Add(inGameData);

            if (saveData.isUnLock)
            {
                UnLockSkillButton(type, inGameData.level);

                skillBtns[(int)type].Init(saveData.isCooltime, saveData.leftCoolTime);
            }

            skillBtns[(int)type].SetTxtLevel(inGameData.level);


        }
    }

    public void SkillCoolTimeSkipByType(SkillType skillType)
    {
        var skillBtn = GetSkillBtnByType(skillType);
        skillBtn.SkipCoolTime();
    }

    // 초기 UI 세팅
    void SetSlotUI()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            var slot = skillSlots[i];
            var inGameData = GetSkillInGameDataByType(slot.skillType);
            var levelName = $"Lv{inGameData.level} {inGameData.skilName}";
            var data = GetSkillData(slot.skillType, inGameData.level);

            slot.SetTxt_Level(levelName);
            // slot.SetTxt_Name(data.name);

            slot.SetTxt_MaxLevel(data.maxLevel.ToString());
            slot.SetTxt_Cost(GetSkillPrice(data, inGameData));



            // set description 


            // if (IsDP_Type(slot.skillType))
            //     slot.SetTxt_Description(GetDesicriptionDP(slot.skillType, data, inGameData));
            // else if (IsD_Type(slot.skillType))
            //     slot.SetTxt_Description(GetDesicriptionDP(slot.skillType, data, inGameData));
            // else
            //     slot.SetTxt_Description(data.desctiption);


            switch (slot.skillType)
            {
                case SkillType.insectDamageUp:
                    SetUI(ref slot, data, inGameData); // SET UI
                    break;

                case SkillType.unionDamageUp:
                    SetUI(ref slot, data, inGameData);
                    break;

                case SkillType.allUnitSpeedUp:
                    SetUIMoveSpeed(ref slot, data, inGameData);
                    break;

                case SkillType.glodBonusUp:
                    SetUI(ref slot, data, inGameData);
                    break;

                case SkillType.monsterKing:
                    SetUI_MonsterKing(ref slot, data, inGameData);
                    break;

                case SkillType.allUnitCriticalChanceUp:
                    SetUIAllUnitCDU(ref slot, data, inGameData);
                    break;
            }



            // set max 
            if (data.maxLevel <= inGameData.level)
            {
                slot.MaxStat();
            }

        }
    }

    bool IsDP_Type(SkillType skillType)
    {

        return skillType == SkillType.insectDamageUp || skillType == SkillType.unionDamageUp || skillType == SkillType.allUnitSpeedUp || skillType == SkillType.glodBonusUp;

    }
    bool IsD_Type(SkillType skillType)
    {
        return skillType == SkillType.allUnitCriticalChanceUp;
    }

    public void LevelUpSkill(SkillType skillType)
    {
        var inGameData = GetSkillInGameDataByType(skillType);
        var skillData = GetSkillData(skillType, inGameData.level);
        var skillSlot = GetSkillSlotByType(skillType);

        // 최대 레벨 판단
        var isMaximumLevel = IsMaximumLevel(skillData, inGameData);



        // 현재 가격
        var skillPrice = GetSkillPrice(skillData, inGameData);

        // 구매 가능 한지 확인
        var isPaySkill = IsPaySkill(skillPrice);

        if (isPaySkill)
        {

            // 구매
            GlobalData.instance.player.PayGold((int)skillPrice);
            //Debug.Log($" {skillData.name} 스킬을 구매 하였습니다.");
            // 레벨업
            inGameData.level++;

            skillData = GetSkillData(skillType, inGameData.level);

            var slot = GetSkillSlotByType(skillType);
            skillBtns[(int)skillType].SetTxtLevel(inGameData.level);

            switch (skillType)
            {
                case SkillType.insectDamageUp:
                    AddInGameDataValue(skillData, ref inGameData); // SET DATA
                    SetUI(ref skillSlot, skillData, inGameData); // SET UI
                    break;

                case SkillType.unionDamageUp:
                    AddInGameDataValue(skillData, ref inGameData);
                    SetUI(ref skillSlot, skillData, inGameData);
                    break;

                case SkillType.allUnitSpeedUp:
                    AddInGameDataValue(skillData, ref inGameData);
                    SetUIMoveSpeed(ref skillSlot, skillData, inGameData);
                    break;

                case SkillType.glodBonusUp:
                    AddInGameDataValue(skillData, ref inGameData);
                    SetUI(ref skillSlot, skillData, inGameData);
                    break;

                case SkillType.monsterKing:
                    AddInGameDataDamage(skillData, ref inGameData);
                    SetUI_MonsterKing(ref skillSlot, skillData, inGameData);
                    break;

                case SkillType.allUnitCriticalChanceUp:
                    AddInGameDataValue(skillData, ref inGameData);
                    SetUIAllUnitCDU(ref skillSlot, skillData, inGameData);
                    break;
            }

            // UNLOCK SKILL
            UnLockSkillButton(skillType, inGameData.level);

            // set save data
            GlobalData.instance.saveDataManager.SaveSkillData(skillType, inGameData);

            isMaximumLevel = IsMaximumLevel(skillData, inGameData);
            if (isMaximumLevel)
            {
                slot.MaxStat();
            }
        }
        else
        {
            // 맥시멈 레벨 도달
            if (isMaximumLevel)
            {
                Debug.Log($"{skillData.name} 스킬은 최대 레벨에 도달 했습니다.");
            }

            // 구매 불가
            if (!isPaySkill)
            {
                Debug.Log($"보유 골드가 부족하여 {skillData.name} 스킬을 구매 할 수 없습니다.");
            }
        }
    }

    // SkillData GetSkillData(SkillType skillType)
    // {
    //     return GlobalData.instance.dataManager.GetSkillDataById((int)skillType);
    // }

    SkillData GetSkillData(SkillType skillType, int level)
    {
        SkillLevelData data = GlobalData.instance.dataManager.GetSkillLevelData(skillType, level);
        SkillData skillData = new SkillData();

        skillData.maxLevel = data.maxLevel;
        skillData.duration = data.duration;
        skillData.power = data.power;
        skillData.name = data.name;
        skillData.coolTime = data.coolTime;
        skillData.desctiption = data.desctiption;
        skillData.salePrice = data.salePrice;
        return skillData;
    }


    void AddInGameDataDamage(SkillData skillData, ref Skill_InGameData inGameData)
    {
        inGameData.power = GetPowerValue(skillData, inGameData);
        inGameData.damage += GetDamangeValue(inGameData.power);
    }

    void AddInGameDataValue(SkillData skillData, ref Skill_InGameData inGameData)
    {
        inGameData.duaration = GetDurationValue(skillData, inGameData);
        inGameData.power = GetPowerValue(skillData, inGameData);
    }

    void SetUI(ref SkilSlot skillSlot, SkillData skillData, Skill_InGameData inGameData)
    {
        var description = GetDesicriptionDP(skillSlot.skillType, skillData, inGameData);
        skillSlot.SetTxt_Cost(GetSkillPrice(skillData, inGameData));
        var levelName = $"Lv{inGameData.level} {inGameData.skilName}";
        skillSlot.SetTxt_Level(levelName);
        skillSlot.SetTxt_Description(description);

        // var isMaximumLevel = IsMaximumLevel(skillData, inGameData);
        // if (isMaximumLevel)
        // {
        //     skillSlot.MaxStat();
        // }
    }

    void SetUIMoveSpeed(ref SkilSlot skillSlot, SkillData skillData, Skill_InGameData inGameData)
    {
        var description = GetDesicriptionDNoneP(skillSlot.skillType, skillData, inGameData);
        skillSlot.SetTxt_Cost(GetSkillPrice(skillData, inGameData));
        var levelName = $"Lv{inGameData.level} {inGameData.skilName}";
        skillSlot.SetTxt_Level(levelName);
        skillSlot.SetTxt_Description(description);
    }


    void SetUIAllUnitCDU(ref SkilSlot skillSlot, SkillData skillData, Skill_InGameData inGameData)
    {
        var description = GetDesicriptionD(skillSlot.skillType, skillData, inGameData);
        skillSlot.SetTxt_Cost(GetSkillPrice(skillData, inGameData));
        var levelName = $"Lv{inGameData.level} {inGameData.skilName}";
        skillSlot.SetTxt_Level(levelName);
        skillSlot.SetTxt_Description(description);
    }
    void SetUI_MonsterKing(ref SkilSlot skillSlot, SkillData skillData, Skill_InGameData inGameData)
    {
        skillSlot.SetTxt_Cost(GetSkillPrice(skillData, inGameData));
        var levelName = $"Lv{inGameData.level} {inGameData.skilName}";
        skillSlot.SetTxt_Level(levelName);

        var description = GetDesicriptionDP(skillSlot.skillType, skillData, inGameData);
        skillSlot.SetTxt_Description(description);
    }

    // duration , power
    string GetDesicriptionDP(SkillType skillType, SkillData skillData, Skill_InGameData inGameData)
    {
        var orl_description = skillData.desctiption;
        return orl_description.Replace("<Duration>초", $"<#40ff80>{inGameData.duaration}초</color>").Replace("<Power>", $"<#40ff80>{inGameData.power}%</color>");
    }
    string GetDesicriptionDNoneP(SkillType skillType, SkillData skillData, Skill_InGameData inGameData)
    {
        var orl_description = skillData.desctiption;
        return orl_description.Replace("<Duration>초", $"<#40ff80>{inGameData.duaration}초</color>").Replace("<Power>", $"<#40ff80>{inGameData.power}</color>");
    }
    // duration
    string GetDesicriptionD(SkillType skillType, SkillData skillData, Skill_InGameData inGameData)
    {
        var orl_description = skillData.desctiption;
        return orl_description.Replace("<Duration>초", $"<#40ff80>{inGameData.duaration}초</color>");
    }



    float GetInitPowerValue(SkillData skillData, Skill_InGameData skill_InGameData)
    {
        // double power = 0;
        // for (int i = 0; i < skill_InGameData.level; i++)
        // {
        //     power += skillData.power + (i * skillData.addPowerRate);
        // }
        // return power;

        return skillData.power;
    }
    float GetInitDurationValue(SkillData skillData, Skill_InGameData skill_InGameData)
    {
        // float duration = 0;
        // for (int i = 0; i < skill_InGameData.level; i++)
        // {
        //     duration += skillData.duration + (i * skillData.addDurationTime);
        // }
        // return duration;
        return skillData.duration;
    }



    float GetPowerValue(SkillData skillData, Skill_InGameData skill_InGameData)
    {
        //return skillData.power + (skill_InGameData.level * skillData.addPowerRate);
        return skillData.power;
    }
    float GetDurationValue(SkillData skillData, Skill_InGameData skill_InGameData)
    {
        // return skillData.duration + (skill_InGameData.level + skillData.addDurationTime);
        return skillData.duration;
    }

    float GetDamangeValue(float power)
    {
        return GlobalData.instance.insectManager.GetInsectsDps() * power;
    }

    // 현재 골드로 구매 가능한지 확인
    bool IsPaySkill(float price)
    {
        return GlobalData.instance.player.gold >= price;
    }

    float GetSkillPrice(SkillData data, Skill_InGameData skill_InGameData)
    {
        return data.salePrice;
        // return data.defaultCost + (data.addCostAmount * skill_InGameData.level);
    }

    // 최대 레벨 도달 판단
    bool IsMaximumLevel(SkillData data, Skill_InGameData skill_InGameData)
    {
        return skill_InGameData.level == data.maxLevel;
    }


    SkilSlot GetSkillSlotByType(SkillType skillType)
    {
        return skillSlots.FirstOrDefault(f => f.skillType == skillType);
    }

    public Skill_InGameData GetSkillInGameDataByType(SkillType skillType)
    {
        return skill_InGameDatas.FirstOrDefault(f => f.skillType == skillType);
    }

    // 골드 보류량에 따라 스킬 구매 버튼 활성, 비활성화
    public void EnableBuyButtons()
    {
        StartCoroutine(EnableBuyButtons_Cor());
    }

    IEnumerator EnableBuyButtons_Cor()
    {
        yield return new WaitForEndOfFrame();
        foreach (var slot in skillSlots)
        {
            var inGameData = GetSkillInGameDataByType(slot.skillType);
            var data = GetSkillData(slot.skillType, inGameData.level);
            var price = GetSkillPrice(data, inGameData);
            slot.btnPay.interactable = IsPaySkill(price);
        }
    }

    public SkillBtn GetSkillBtnByType(SkillType skillType)
    {
        return skillBtns.FirstOrDefault(f => f.skillType == skillType);
    }

    public bool IsUsingSkillByType(SkillType skillType)
    {
        return GetSkillBtnByType(skillType).skillAddValue;
    }

    public bool IsUnionDamageUpSkillEffOn()
    {
        return GetSkillBtnByType(SkillType.unionDamageUp).skillAddValue;
    }

    public bool IsAllUnitCritChanceUpSkillEffOn()
    {
        return GetSkillBtnByType(SkillType.allUnitCriticalChanceUp).skillAddValue;
    }


}
