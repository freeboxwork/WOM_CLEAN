using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System;
using static EnumDefinition;
using System.Linq;

public class SaveDataManager : MonoBehaviour
{
    public SaveDataTotal saveDataTotal;
    const string dataFileName = "saveData.json";
    public GlobalData globalData;

    /* 맥스 레벨 제한값 추가 되어야 함 */
    // 데이터 로드 했을때 현재 레벨이 맥스 레벨임에도 그 이상의 레벨을 요구할 경우 문제가 생기기 때문에 필히 예외 처리 해야함
    void Start()
    {
        globalData = FindObjectOfType<GlobalData>();

    }




    public IEnumerator Init()
    {
        LoadDataFromFile();

        //SetData();

        yield return new WaitForEndOfFrame();

        // Save Json File
        //SaveDataToFile();
    }


    // SMAPLE
    void SampleSaveJsonData()
    {
        SaveDataTest saveDataTest = new SaveDataTest();
        saveDataTest.skillType = EnumDefinition.SkillType.insectDamageUp;
        saveDataTest.level = 1;
        saveDataTest.saveDataUnions.Add(new SaveDataUnion() { equipSlotId = 1, level = 2, isEquip = false, unionId = 20 });

        var data = JsonUtility.ToJson(saveDataTest, true);
        File.WriteAllText(Application.dataPath + "/data.json", data);
    }


    public void SaveDataToFile()
    {
        var jsonData = JsonUtility.ToJson(saveDataTotal);
        var path = GetSaveDataFilePaht();
        File.WriteAllText(path, jsonData);
    }

    void OnApplicationQuit()
    {
        // 데이터 저장                
        var jsonData = JsonUtility.ToJson(saveDataTotal);
        var path = GetSaveDataFilePaht();
        File.WriteAllText(path, jsonData);
    }

    public IEnumerator SaveDataToFileCoroutine()
    {
        // ui interaction disable
        UtilityMethod.EnableUIEventSystem(false);
        // 데이터 저장                
        var jsonData = JsonUtility.ToJson(saveDataTotal);
        var path = GetSaveDataFilePaht();
        File.WriteAllText(path, jsonData);
        yield return new WaitForEndOfFrame();
        // 종료
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
        Application.Quit(); 
#endif    
    }

    void LoadDataFromFile()
    {
        var path = GetSaveDataFilePaht();
        if (File.Exists(path))
        {
            var file = File.ReadAllText(path);
            saveDataTotal = JsonUtility.FromJson<SaveDataTotal>(file);
            //SetData();
        }
        else
        {
            InitData();
            SaveDataToFile();
        }
    }

    // 저장된 파일에서 데이터 읽어오기
    // public void SetData()
    // {
    //     var player = globalData.player;
    //     player.gold = saveDataTotal.saveDataGoods.gold;
    //     player.bone = saveDataTotal.saveDataGoods.bone;
    //     player.diceCount = saveDataTotal.saveDataGoods.dice;
    //     player.gem = saveDataTotal.saveDataGoods.gem;
    //     player.clearTicket = saveDataTotal.saveDataGoods.clearTicket;
    //     //TODO: dungen key 추가 작업 필요함
    // }

    public void InitData()
    {
        saveDataTotal = new SaveDataTotal();


        // set traning 
        saveDataTotal.saveDataTranings = new SaveDataTranings();
        foreach (SaleStatType type in Enum.GetValues(typeof(SaleStatType)))
        {
            SaveDataTraning saveData = new SaveDataTraning { traningType = type, level = 0 };
            saveDataTotal.saveDataTranings.tranings.Add(saveData);
        }

        // set union 
        saveDataTotal.saveDataUnions = new SaveDataUnions();
        foreach (var union in globalData.dataManager.unionDatas.data)
        {
            saveDataTotal.saveDataUnions.unions.Add(new SaveDataUnion { unionId = union.unionIndex });
        }

        // 성능 테스트
        // for (int i = 0; i < 64; i++)
        // {
        //     saveDataTotal.saveDataUnions.unions.Add(new SaveDataUnion { unionId = i });
        // }

        // set DNA
        saveDataTotal.saveDataDNAs = new SaveDataDNAs();
        foreach (DNAType type in Enum.GetValues(typeof(DNAType)))
        {
            saveDataTotal.saveDataDNAs.saveDatas.Add(new SaveDataDNA { dnaType = type });
        }

        // set skill
        saveDataTotal.saveDataSkills = new SaveDataSkills();
        foreach (SkillType type in Enum.GetValues(typeof(SkillType)))
        {
            saveDataTotal.saveDataSkills.saveDataSkills.Add(new SaveDataSkill { skillType = type });
        }

    }


    #region UTILITY METHOD

    public T GetSaveDataByType<T>(IEnumerable<T> saveDataList, Func<T, bool> predicate, string type)
    {
        var saveData = saveDataList.FirstOrDefault(predicate);
        if (saveData == null)
        {
            throw new Exception($"{type}은 존재하지 않습니다.");
        }
        return saveData;
    }


    // 트레이닝 데이터 세팅

    public void SetLevelByTraningType(EnumDefinition.SaleStatType traningType, int newLevel)
    {
        SaveDataTraning traningData = GetSaveDataByType(saveDataTotal.saveDataTranings.tranings,
            f => f.traningType == traningType, traningType.ToString());

        if (traningData != null)
        {
            traningData.level = newLevel;
        }
    }

    public SaveDataTraning GetTraningData(EnumDefinition.SaleStatType traningType)
    {
        var data = saveDataTotal.saveDataTranings.tranings.FirstOrDefault(f => f.traningType == traningType);
        if (data == null)
        {
            Debug.LogError($"save data maanger => traningType : {traningType} is null");
        }

        return data;
    }


    // DNA 데이터 세팅
    public void SetLevelDNAByType(DNAType dnaType, DNAInGameData inGameData)
    {
        SaveDataDNA dnaData = GetSaveDataByType(saveDataTotal.saveDataDNAs.saveDatas,
            f => f.dnaType == dnaType, dnaType.ToString());
        dnaData.level = inGameData.level;
        dnaData.power = inGameData.power;
    }

    public SaveDataDNA GetSaveDataDNA(DNAType dnaType)
    {
        return GetSaveDataByType(saveDataTotal.saveDataDNAs.saveDatas,
            f => f.dnaType == dnaType, dnaType.ToString());
    }

    // 진화 데이터 세팅
    public void SetEvolutionLevel(int evolutionLevel)
    {
        saveDataTotal.saveDataEvolution.level_evolution = evolutionLevel;
    }
    public void SetEvolutionInGameData(DiceEvolutionInGameData inGameData)
    {
        saveDataTotal.saveDataEvolution.diceEvolutionData = inGameData.CopyInstance();
    }

    public SaveDataEvolution GetEvolutionData()
    {
        return saveDataTotal.saveDataEvolution;
    }

    //public SaveDataUnion GetUnionDataById(int unionID)
    //{
    //    var union = saveDataTotal.saveDataUnions.unions.FirstOrDefault(f=> f.unionId == unionID);
    //    if (union == null)
    //    {
    //        throw new Exception($"Union with ID {unionID} not found.");
    //    }
    //    return union;
    //}


    // 유니온 데이터 세팅
    //public void SaveUnionData(UnionSlot unionSlot)
    //{
    //    var inGmaeData = unionSlot.inGameData;
    //    var unionID = inGmaeData.unionIndex;
    //    var union = GetSaveDataByType(saveDataTotal.saveDataUnions.unions, f => f.unionId == unionID, $"????? ID : {unionID}");

    //    // union.unionId = inGmaeData.unionIndex;
    //    union.level = inGmaeData.level;
    //    union.isEquip = unionSlot.unionEquipType == UnionEquipType.Equipped;
    //    if (union.isEquip)
    //    {
    //        union.equipSlotId = unionSlot.unionEquipSlot.slotIndex;
    //    }
    //}

    public void SaveUnionLevelData(UnionSlot unionSlot)
    {
        var inGmaeData = unionSlot.inGameData;
        GetSaveDataUnion(unionSlot).level = inGmaeData.level;
    }

    public void SaveUnionCountData(UnionSlot unionSlot)
    {
        var inGmaeData = unionSlot.inGameData;
        GetSaveDataUnion(unionSlot).unionCount = inGmaeData.unionCount;
    }

    public void SaveUnionEquipSlotData(UnionSlot unionSlot)
    {
        var union = GetSaveDataUnion(unionSlot);
        union.isEquip = unionSlot.unionEquipType == UnionEquipType.Equipped;
        union.equipSlotId = union.isEquip ? unionSlot.unionEquipSlot.slotIndex : 999;
    }

    public void SaveUnionEquipSlotData(UnionSlot unionSlot, UnionEquipSlot unionEquipSlot)
    {
        var union = GetSaveDataUnion(unionSlot);
        union.isEquip = unionEquipSlot != null;

        // union.isEquip 을 로그로 출력 하는 코드
        Debug.Log($"SaveUnionEquipSlotData => union.isEquip : {union.isEquip}, unionEquipSlot : {unionEquipSlot}");


        union.equipSlotId = unionEquipSlot?.slotIndex ?? 999;
    }




    public SaveDataUnion GetSaveDataUnion(UnionSlot unionSlot)
    {
        var inGmaeData = unionSlot.inGameData;
        var unionID = inGmaeData.unionIndex;
        var union = GetSaveDataByType(saveDataTotal.saveDataUnions.unions, f => f.unionId == unionID, $"유니온 ID : {unionID}");
        return union;
    }


    //스킬 데이터 세팅
    public void SaveSkillData(SkillType skillType, Skill_InGameData skill_InGameData)
    {
        SaveDataSkill skillData = GetSaveDataByType(saveDataTotal.saveDataSkills.saveDataSkills,
            f => f.skillType == skillType, skillType.ToString());

        skillData.skillType = skillType;
        skillData.level = skill_InGameData.level;
        skillData.damage = skill_InGameData.damage;
        skillData.isUsingSkill = skill_InGameData.isSkilUsing;
        skillData.leftSkillTime = skill_InGameData.skillLeftTime;

    }

    public SaveDataSkill GetSaveDataSkill(SkillType skillType)
    {
        return GetSaveDataByType(saveDataTotal.saveDataSkills.saveDataSkills,
            f => f.skillType == skillType, skillType.ToString());
    }


    public void SetSkillLeftTime(SkillType skillType, float leftTime)
    {
        SaveDataSkill skillData = GetSaveDataSkill(skillType);
        skillData.leftSkillTime = leftTime;
    }

    public void SetSkillUsingValue(SkillType skillType, bool isUsing)
    {
        SaveDataSkill skillData = GetSaveDataSkill(skillType);
        skillData.isUsingSkill = isUsing;
    }

    // SHOP 데이터 세팅


    // 스테이지 데이터 세팅
    public void SaveStageDataLevel(int stageLevel)
    {
        saveDataTotal.saveDataStage.stageLevel = stageLevel;
    }

    public void SaveStageDataPhaseCount(int phaseCount)
    {
        saveDataTotal.saveDataStage.phaseCount = phaseCount;
    }


    // 재화 데이터 세팅
    public void SaveDataGoodsGold(int gold)
    {
        saveDataTotal.saveDataGoods.gold = gold;
    }
    public void SaveDataGoodsGem(int gem)
    {
        saveDataTotal.saveDataGoods.gem = gem;
    }
    public void SaveDataGoodsBone(int bone)
    {
        saveDataTotal.saveDataGoods.bone = bone;
    }
    public void SaveDataGoodsDice(int dice)
    {
        saveDataTotal.saveDataGoods.dice = dice;
    }
    public void SaveDataGoodsCoal(int coal)
    {
        saveDataTotal.saveDataGoods.coal = coal;
    }
    public void SaveDataGoodsClearTicket(int clearTicket)
    {
        saveDataTotal.saveDataGoods.clearTicket = clearTicket;
    }

    // 던전 레벨 데이터 저장
    public void SaveDataDungeonLevelGold(int dungeonLevel)
    {
        saveDataTotal.saveDataDungeonLevel.dungeonLvGold = dungeonLevel;
    }

    public void SaveDataDungeonLevelBone(int dungeonLevel)
    {
        saveDataTotal.saveDataDungeonLevel.dungeonLvBone = dungeonLevel;
    }

    public void SaveDataDungeonLevelDice(int dungeonLevel)
    {
        saveDataTotal.saveDataDungeonLevel.dungeonLvDice = dungeonLevel;
    }

    public void SaveDataDungeonLevelCoal(int dungeonLevel)
    {
        saveDataTotal.saveDataDungeonLevel.dungeonLvCoal = dungeonLevel;
    }

    // 던전 키 데이터 저장
    public void SaveDataGoodsDungeonKey(GoodsType goodsType, int dungeonKey)
    {
        var keyValue = GetGoodsData(goodsType);
        keyValue = dungeonKey;
    }

    ref int GetGoodsData(GoodsType goodsType)
    {
        switch (goodsType)
        {
            case GoodsType.gold:
                return ref saveDataTotal.saveDataGoods.gold;
            case GoodsType.gem:
                return ref saveDataTotal.saveDataGoods.gem;
            case GoodsType.bone:
                return ref saveDataTotal.saveDataGoods.bone;
            case GoodsType.dice:
                return ref saveDataTotal.saveDataGoods.dice;
            case GoodsType.coal:
                return ref saveDataTotal.saveDataGoods.coal;
            default:
                return ref saveDataTotal.saveDataGoods.gold;
        }
    }

    // 던전 레벨 데이터 저장 -> 던전 레벨은 무조건 0 부터 시작 , 티켓으로 구매하면 클리어한 레벨의 금액만큼 더해준다.





    // 타임 데이터 세팅
    public void SaveDataTimeGameEnd(DateTime time)
    {
        saveDataTotal.saveDataDateTime.time_gameEnd = time;
    }

    public void SaveDataTimeAD_Reset(DateTime time)
    {
        saveDataTotal.saveDataDateTime.time_AD_Reset = time;
    }

    // 시스템 데이터 세팅
    public void SaveDataSystem_SFX_BG(bool value)
    {
        saveDataTotal.saveDataSystem.sfx_bgOnOff = value;
    }
    public void SaveDataSystem_SFX_EFF(bool value)
    {
        saveDataTotal.saveDataSystem.sfx_eff = value;
    }

    public void SaveDataSystem_SFX_BG_Volume(float value)
    {
        saveDataTotal.saveDataSystem.sfx_bg_Volume = value;
    }
    public void SaveDataSystem_SFX_BG_Eff(float value)
    {
        saveDataTotal.saveDataSystem.sfx_eff_Volume = value;
    }
    public void SaveDataSystem_TutorialStap(int value)
    {
        saveDataTotal.saveDataSystem.tutorial_step = value;
    }

    // 투토리얼 스텝 데이터 세팅
    public void SaveDataTutorialStep(int step)
    {
        saveDataTotal.saveDataSystem.tutorial_step = step;
    }


    string GetSaveDataFilePaht()
    {
        string path = "";
#if UNITY_EDITOR
        path = Application.dataPath + "/" + dataFileName;
#elif UNITY_ANDROID
            path = Application.persistentDataPath + "/"+ dataFileName;
#endif
        return path;
    }


    #endregion

}


#region MODELS


// SAMOPLE
[System.Serializable]
public class SaveDataTest
{
    public EnumDefinition.SkillType skillType;
    public int level;
    public List<SaveDataUnion> saveDataUnions = new List<SaveDataUnion>();
}


[System.Serializable]
public class SaveDataTotal
{
    public SaveDataTranings saveDataTranings;
    public SaveDataEvolution saveDataEvolution;
    public SaveDataUnions saveDataUnions;
    public SaveDataDNAs saveDataDNAs;
    public SaveDataSkills saveDataSkills;
    public SaveDataShop saveDataShop;
    public SaveDataStage saveDataStage;
    public SaveDataGoods saveDataGoods;
    public SaveDataDateTime saveDataDateTime;
    public SaveDataSystem saveDataSystem;
    public SaveDataDungeonLevel saveDataDungeonLevel;
    public SaveDataTutorial saveDataTutorial;

}

#region old
//public int level_trainingDamage;
//public int level_trainingCriticalChance;
//public int level_trainingCriticalDamage;
//public int level_talentDamage;
//public int level_talentCriticalChance;
//public int level_talentCriticalDamage;
//public int level_talentMoveSpeed;
//public int level_talentSpawnSpeed;
//public int level_talentGoldBonus;
#endregion

[System.Serializable]
public class SaveDataTutorial
{
    public int tutorial_step;
}

[System.Serializable]
public class SaveDataTranings
{
    public List<SaveDataTraning> tranings = new List<SaveDataTraning>();
}

[System.Serializable]
public class SaveDataTraning
{
    public int level;
    public EnumDefinition.SaleStatType traningType;
}

[System.Serializable]
public class SaveDataEvolution
{
    // 진화 레벨 ( 진화 레벨에 따라 슬롯 오픈됨 )
    public int level_evolution;
    // 진화 주사위 돌려서 획득한 데이터 저장
    public DiceEvolutionInGameData diceEvolutionData;
}


[System.Serializable]
public class SaveDataUnions
{
    public List<SaveDataUnion> unions = new List<SaveDataUnion>();
}

[System.Serializable]
public class SaveDataUnion
{
    public int unionId;
    public int level;
    public int unionCount;// 현재 유니온 보유 수
    public int equipSlotId;
    public bool isEquip;

}


[System.Serializable]
public class SaveDataDNAs
{
    #region data list
    /*
        insectDamage,
        insectCriticalChance,
        insectCriticalDamage,
        unionDamage,
        glodBonus,
        insectMoveSpeed,
        unionMoveSpeed,
        unionSpawnTime,
        goldPig,
        skillDuration,
        skillCoolTime,
        bossDamage,
        monsterHpLess,
        boneBonus,
        goldMonsterBonus,
        offlineBonus
     */
    #endregion

    public List<SaveDataDNA> saveDatas = new List<SaveDataDNA>();
}

[System.Serializable]
public class SaveDataDNA
{
    public int level;
    public float power;
    public int dnaIndex;
    public DNAType dnaType;
}

[System.Serializable]
public class SaveDataSkills
{
    /*
    insectDamageUp;
    unionDamageUp;
    allUnitSpeedUp;
    glodBonusUp;
    monsterKing;
    allUnitCriticalChanceUp;
    */
    public List<SaveDataSkill> saveDataSkills = new List<SaveDataSkill>();
}

[System.Serializable]
public class SaveDataSkill
{
    public int level;
    public float damage;
    public bool isUsingSkill; // 스킬 사용중 표시
    public float leftSkillTime; // 스킬 남은 시간  
    public EnumDefinition.SkillType skillType;
}

[System.Serializable]
public class SaveDataDungeonLevel
{
    public int dungeonLvGold = 1;
    public int dungeonLvBone = 1;
    public int dungeonLvDice = 1;
    public int dungeonLvCoal = 1;
}

[System.Serializable]
public class SaveDataShop
{

}

[System.Serializable]
public class SaveDataStage
{
    public int stageLevel;
    public int phaseCount;
}


[System.Serializable]
public class SaveDataGoods
{
    public int gold;
    public int gem;
    public int bone;
    public int dice;
    public int coal;
    public int clearTicket;
    public int unionTicket;
    public int dnaTicket;
    public int dungeonKeyGold;
    public int dungeonKeyBone;
    public int dungeonKeyCoal;
    public int dungeonKeyDice;
}



[System.Serializable]
public class SaveDataDateTime
{
    public DateTime time_gameEnd;
    public DateTime time_AD_Reset;
}

[System.Serializable]
public class SaveDataSystem
{
    // 배경음 OnOff
    public bool sfx_bgOnOff;
    public float sfx_bg_Volume;
    // 효과음 OnOff
    public bool sfx_eff;
    public float sfx_eff_Volume;
    // 투토리얼 진행 스탭
    public int tutorial_step;
}

#endregion
