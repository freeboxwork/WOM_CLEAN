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

    public void UnLockSkillButton(SkillType skillType)
    {
        var skillData = GetSkillData(skillType);
        var btn = skillBtns[(int)skillType];
        btn.SetTxtCoolTime((int)skillData.coolTime);
        btn.gameObject.SetActive(true);
        btn.skillReady = true;
    }

    public IEnumerator Init()
    {
        yield return null;
        SetSkillInGameData();
        SetSlotUI();
        EnableBuyButtons();
    }

    void SetSkillInGameData()
    {
        foreach (SkillType type in System.Enum.GetValues(typeof(SkillType)))
        {
            var skillData = GetSkillData(type);
            
            Skill_InGameData data = new Skill_InGameData();
            data.skillType = type;

            //TODO : 추 후 저장된 값에서 불러와야 함
            data.level = 0;
            data.duaration = skillData.duration;
            data.power = skillData.power;
            data.damage = 0;
            data.coolTime = skillData.coolTime;
            data.skilName = skillData.name;

            skill_InGameDatas.Add(data);
        }
    }

    // 초기 UI 세팅
    void SetSlotUI()
    {
        for (int i = 0; i < skillSlots.Count; i++)
        {
            var slot = skillSlots[i];
            var data = GetSkillData(slot.skillType);
            var inGameData = GetSkillInGameDataByType(slot.skillType);
            var levelName = $"Lv{inGameData.level} {inGameData.skilName}";

            slot.SetTxt_Level(levelName);
           // slot.SetTxt_Name(data.name);

            slot.SetTxt_MaxLevel(data.maxLevel.ToString());
            slot.SetTxt_Cost(GetSkillPrice(data, inGameData).ToString());

            // set description 
            if (IsDP_Type(slot.skillType))
                slot.SetTxt_Description(GetDesicriptionDP(slot.skillType, data, inGameData));
            else if (IsD_Type(slot.skillType))
                slot.SetTxt_Description(GetDesicriptionDP(slot.skillType, data, inGameData));
            else
                slot.SetTxt_Description(data.desctiption);
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
        var skillData = GetSkillData(skillType);
        var skillSlot = GetSkillSlotByType(skillType);

        // 최대 레벨 판단
        var isMaximumLevel = IsMaximumLevel(skillData, inGameData);

        // 현재 가격
        var skillPrice = GetSkillPrice(skillData, inGameData);
        
        // 구매 가능 한지 확인
        var isPaySkill = IsPaySkill(skillPrice);

        if (!isMaximumLevel && isPaySkill)
        {
            // 구매
            GlobalData.instance.player.PayGold((int)skillPrice);
            Debug.Log($" {skillData.name} 스킬을 구매 하였습니다.");

            // 레벨업
            ++inGameData.level;

            var slot = GetSkillSlotByType(skillType);

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
                    SetUI(ref skillSlot, skillData, inGameData);
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
                    AddInGameDataDamage(skillData, ref inGameData);
                    SetUIAllUnitCDU(ref skillSlot, skillData, inGameData);
                    break;
            }

            // UNLOCK SKILL
            UnLockSkillButton(skillType);

            // set save data
            GlobalData.instance.saveDataManager.SaveSkillData(skillType, inGameData);
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

    SkillData GetSkillData(SkillType skillType)
    {
        return GlobalData.instance.dataManager.GetSkillDataById((int)skillType);
    }

    void AddInGameDataDamage(SkillData skillData, ref Skill_InGameData inGameData)
    {
        inGameData.power += GetPowerValue(skillData, inGameData);
        inGameData.damage += GetDamangeValue(inGameData.power);
    }

    void AddInGameDataValue(SkillData skillData, ref Skill_InGameData inGameData)
    {
        inGameData.duaration += GetDurationValue(skillData, inGameData);
        inGameData.power += GetPowerValue(skillData, inGameData);
    }

    void SetUI(ref SkilSlot skillSlot, SkillData skillData, Skill_InGameData inGameData)
    {
        var description = GetDesicriptionDP(skillSlot.skillType, skillData, inGameData);
        skillSlot.SetTxt_Cost(GetSkillPrice(skillData, inGameData).ToString());
        var levelName = $"Lv{inGameData.level} {inGameData.skilName}";
        skillSlot.SetTxt_Level(levelName);
        
        skillSlot.SetTxt_Description(description);

    }
    void SetUIAllUnitCDU(ref SkilSlot skillSlot, SkillData skillData, Skill_InGameData inGameData)
    {
        var description = GetDesicriptionD(skillSlot.skillType, skillData, inGameData);
        skillSlot.SetTxt_Cost(GetSkillPrice(skillData, inGameData).ToString());
        var levelName = $"Lv{inGameData.level} {inGameData.skilName}";
        skillSlot.SetTxt_Level(levelName);
        skillSlot.SetTxt_Description(description);
    }
    void SetUI_MonsterKing(ref SkilSlot skillSlot, SkillData skillData, Skill_InGameData inGameData)
    {
        skillSlot.SetTxt_Cost(GetSkillPrice(skillData, inGameData).ToString());
        var levelName = $"Lv{inGameData.level} {inGameData.skilName}";
        skillSlot.SetTxt_Level(levelName);
    }

    // duration , power
    string GetDesicriptionDP(SkillType skillType, SkillData skillData, Skill_InGameData inGameData)
    {
        var orl_description = skillData.desctiption;
        return orl_description.Replace("<Duration>초", $"<#40ff80>{inGameData.duaration}초</color>").Replace("<Power>", $"<#40ff80>{inGameData.power}%</color>");
    }
    // duration
    string GetDesicriptionD(SkillType skillType, SkillData skillData, Skill_InGameData inGameData)
    {
        var orl_description = skillData.desctiption;
        return orl_description.Replace("<Duration>초", $"<#40ff80>{inGameData.duaration}초</color>");
    }


    float GetDurationValue(SkillData skillData , Skill_InGameData skill_InGameData)
    {
        return skillData.duration + (skill_InGameData.level + skillData.addDurationTime);
    }

    float GetPowerValue(SkillData skillData, Skill_InGameData skill_InGameData)
    {
        return skillData.power + (skill_InGameData.level * skillData.addPowerRate);
    }

    float GetDamangeValue( float power)
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
        return data.defaultCost + (data.addCostAmount * skill_InGameData.level);
    }

    // 최대 레벨 도달 판단
    bool IsMaximumLevel(SkillData data, Skill_InGameData skill_InGameData)
    {
        return skill_InGameData.level >= data.maxLevel;
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
            var data = GetSkillData(slot.skillType);
            var inGameData = GetSkillInGameDataByType(slot.skillType);
            var price = GetSkillPrice(data, inGameData);
            slot.btnPay.interactable = IsPaySkill(price);
        }
    }



}
