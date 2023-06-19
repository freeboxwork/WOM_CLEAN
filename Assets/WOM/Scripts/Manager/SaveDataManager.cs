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

    /* �ƽ� ���� ���Ѱ� �߰� �Ǿ�� �� */
    // ������ �ε� ������ ���� ������ �ƽ� �����ӿ��� �� �̻��� ������ �䱸�� ��� ������ ����� ������ ���� ���� ó�� �ؾ���
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

    public IEnumerator SaveDataToFileCoroutine()
    {
        // ui interaction disable
        UtilityMethod.EnableUIEventSystem(false);
        // ������ ����                
        var jsonData = JsonUtility.ToJson(saveDataTotal);
        var path = GetSaveDataFilePaht();
        File.WriteAllText(path, jsonData);
        yield return new WaitForEndOfFrame();
        // ����
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

    // ����� ���Ͽ��� ������ �о����
    public void SetData()
    {
        var player = globalData.player;
        player.gold = saveDataTotal.saveDataGoods.gold;
        player.bone = saveDataTotal.saveDataGoods.bone;
        player.diceCount = saveDataTotal.saveDataGoods.dice;
        player.gem = saveDataTotal.saveDataGoods.gem;
        player.clearTicket = saveDataTotal.saveDataGoods.clearTicket;
        //TODO: dungen key �߰� �۾� �ʿ���
    }

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

        // ���� �׽�Ʈ
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
            throw new Exception($"{type}�� �������� �ʽ��ϴ�.");
        }
        return saveData;
    }


    // Ʈ���̴� ������ ����

    public void SetLevelByTraningType(EnumDefinition.SaleStatType traningType, int newLevel)
    {
        SaveDataTraning traningData = GetSaveDataByType(saveDataTotal.saveDataTranings.tranings,
            f => f.traningType == traningType, traningType.ToString());

        if (traningData != null)
        {
            traningData.level = newLevel;
        }
    }


    // DNA ������ ����
    public void SetLevelDNAByType(DNAType dnaType, int level)
    {
        SaveDataDNA dnaData = GetSaveDataByType(saveDataTotal.saveDataDNAs.saveDatas,
            f => f.dnaType == dnaType, dnaType.ToString());
        dnaData.level = level;
    }

    // ��ȭ ������ ����
    public void SetEvolutionLevel(int evolutionLevel)
    {
        saveDataTotal.saveDataEvolution.level_evolution = evolutionLevel;
    }
    public void SetEvolutionInGameData(DiceEvolutionInGameData inGameData)
    {
        saveDataTotal.saveDataEvolution.diceEvolutionData = inGameData.CopyInstance();
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


    // ���Ͽ� ������ ����
    //public void SaveUnionData(UnionSlot unionSlot)
    //{
    //    var inGmaeData = unionSlot.inGameData;
    //    var unionID = inGmaeData.unionIndex;
    //    var union = GetSaveDataByType(saveDataTotal.saveDataUnions.unions, f => f.unionId == unionID, $"���Ͽ� ID : {unionID}");

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

    public void SaveUnionEquipSlotData_(UnionSlot unionSlot)
    {
        var union = GetSaveDataUnion(unionSlot);
        union.isEquip = unionSlot.unionEquipType == UnionEquipType.Equipped;
        union.equipSlotId = union.isEquip ? unionSlot.unionEquipSlot.slotIndex : 999;
    }

    public void SaveUnionEquipSlotData(UnionSlot unionSlot, UnionEquipSlot unionEquipSlot)
    {
        var union = GetSaveDataUnion(unionSlot);
        union.isEquip = unionEquipSlot != null;
        union.equipSlotId = unionEquipSlot?.slotIndex ?? 999;
    }


    SaveDataUnion GetSaveDataUnion(UnionSlot unionSlot)
    {
        var inGmaeData = unionSlot.inGameData;
        var unionID = inGmaeData.unionIndex;
        var union = GetSaveDataByType(saveDataTotal.saveDataUnions.unions, f => f.unionId == unionID, $"���Ͽ� ID : {unionID}");
        return union;
    }


    //��ų ������ ����
    public void SaveSkillData(SkillType skillType, Skill_InGameData skill_InGameData)
    {
        SaveDataSkill skillData = GetSaveDataByType(saveDataTotal.saveDataSkills.saveDataSkills,
            f => f.skillType == skillType, skillType.ToString());

        skillData.skillType = skillType;
        skillData.level = skill_InGameData.level;
        skillData.isUsingSkill = skill_InGameData.isSkilUsing;
        skillData.leftSkillTime = skill_InGameData.skillLeftTime;
    }

    // SHOP ������ ����


    // �������� ������ ����
    public void SaveStageDataLevel(int stageLevel)
    {
        saveDataTotal.saveDataStage.stageLevel = stageLevel;
    }

    public void SaveStageDataPhaseCount(int phaseCount)
    {
        saveDataTotal.saveDataStage.phaseCount = phaseCount;
    }


    // ��ȭ ������ ����
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


    // �߰� �۾� �ʿ�
    public void SaveDataGoodsDungeonKey(int dungeonKey)
    {
        //saveDataTotal.saveDataGoods.dungeonKey = dungeonKey;
    }


    // Ÿ�� ������ ����
    public void SaveDataTimeGameEnd(DateTime time)
    {
        saveDataTotal.saveDataDateTime.time_gameEnd = time;
    }

    public void SaveDataTimeAD_Reset(DateTime time)
    {
        saveDataTotal.saveDataDateTime.time_AD_Reset = time;
    }

    // �ý��� ������ ����
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
    // ��ȭ ���� ( ��ȭ ������ ���� ���� ���µ� )
    public int level_evolution;
    // ��ȭ �ֻ��� ������ ȹ���� ������ ����
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
    public bool isUsingSkill; // ��ų ����� ǥ��
    public float leftSkillTime; // ��ų ���� �ð� 
    public EnumDefinition.SkillType skillType;
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
    //public int dungeonKey_gold;
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
    // ����� OnOff
    public bool sfx_bgOnOff;
    public float sfx_bg_Volume;
    // ȿ���� OnOff
    public bool sfx_eff;
    public float sfx_eff_Volume;
    // ���丮�� ���� ����
    public int tutorial_step;
}

#endregion
