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

    //saveTime마다 저장 
    float saveTime = 60f;

    /* 맥스 레벨 제한값 추가 되어야 함 */
    // 데이터 로드 했을때 현재 레벨이 맥스 레벨임에도 그 이상의 레벨을 요구할 경우 문제가 생기기 때문에 필히 예외 처리 해야함
    void Start()
    {
        globalData = FindObjectOfType<GlobalData>();
        
    }

    


    public IEnumerator Init()
    {
        yield return StartCoroutine(LoadDataFromFile());
        yield return new WaitForEndOfFrame();
        InvokeRepeating("SaveDataToFile", saveTime, saveTime);
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
        // 종료 시간 저장
        saveDataTotal.saveDataSystem.quitTime = DateTime.Now.ToString();
        SaveDataToFile();
    }

    void OnApplicationPause(bool pauseStatus)
    {
        //AudioSettings.Reset(AudioSettings.GetConfiguration());

        if (pauseStatus)
        {
            // 게임이 일시 중지될 때 
            // 종료 시간 저장
            saveDataTotal.saveDataSystem.quitTime = DateTime.Now.ToString();
            // 데이터 저장                
            SaveDataToFile();

        }
        else
        {

        }
    }




    public IEnumerator SaveDataToFileCoroutine()
    {
        // ui interaction disable
        UtilityMethod.EnableUIEventSystem(false);
        // 종료 시간 저장
        saveDataTotal.saveDataSystem.quitTime = DateTime.Now.ToString();
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

    IEnumerator LoadDataFromFile()
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

            yield return new WaitForEndOfFrame();
            var file = File.ReadAllText(path);
            saveDataTotal = JsonUtility.FromJson<SaveDataTotal>(file);
        }

        yield return null;
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
        // foreach (var data in saveDataTotal.saveDataSkills.saveDataSkills)
        // {
        //     data.skill_InGameData = new Skill_InGameData();
        // }

        // set evolution slot
        saveDataTotal.saveDataEvolution = new SaveDataEvolution();
        var slots = globalData.evolutionManager.evolutionSlots;
        for (int i = 0; i < slots.Count; i++)
        {
            var slot = new SaveDataEvolutionSolt();
            slot.slotId = i;
            saveDataTotal.saveDataEvolution.saveDataEvolutionSolts.Add(slot);
            slot.evolutionDiceStatType = EvolutionDiceStatType.none.ToString();
            //slot.clorHexCode = "#808080";
            //slot.symbolId = 0;
        }

        // 최초 던전 키 2개씩 지급
        saveDataTotal.saveDataGoods = new SaveDataGoods();
        saveDataTotal.saveDataGoods.dungeonKeyGold = 2;
        saveDataTotal.saveDataGoods.dungeonKeyBone = 2;
        saveDataTotal.saveDataGoods.dungeonKeyDice = 2;
        saveDataTotal.saveDataGoods.dungeonKeyCoal = 2;

        saveDataTotal.saveDataGoods.dungeonKeyADGold = 1;
        saveDataTotal.saveDataGoods.dungeonKeyADBone = 1;
        saveDataTotal.saveDataGoods.dungeonKeyADDice = 1;
        saveDataTotal.saveDataGoods.dungeonKeyADCoal = 1;

        saveDataTotal.saveDataDungeonLevel = new SaveDataDungeonLevel();
        saveDataTotal.saveDataDungeonLevel.dungeonLvGold = 0;
        saveDataTotal.saveDataDungeonLevel.dungeonLvBone = 0;
        saveDataTotal.saveDataDungeonLevel.dungeonLvDice = 0;
        saveDataTotal.saveDataDungeonLevel.dungeonLvCoal = 0;

        saveDataTotal.saveDataLabBuildingData = new SaveDataLabBuildingData();
        var zeroLevelPrice = globalData.dataManager.GetLabBuildingDataByLevel(0).price;
        saveDataTotal.saveDataLabBuildingData.labBuildIngameDatas = new List<LabBuildIngameData>(){

            new LabBuildIngameData(){ goodsType = EnumDefinition.GoodsType.gold , level = 0, price = zeroLevelPrice },
            new LabBuildIngameData(){ goodsType = EnumDefinition.GoodsType.dice, level = 0,price = zeroLevelPrice },
            new LabBuildIngameData(){ goodsType = EnumDefinition.GoodsType.bone, level = 0,price = zeroLevelPrice },
            new LabBuildIngameData(){ goodsType = EnumDefinition.GoodsType.coal, level = 0,price = zeroLevelPrice },
        };

        // ad buff timer
        saveDataTotal.saveDataBuffAD = new SaveDataBuffAD();
        saveDataTotal.saveDataBuffAD.buffAD_LeftDatas = new List<SaveDataBuffAD_LeftData>(){
            {new SaveDataBuffAD_LeftData(){ buffADType = EnumDefinition.RewardTypeAD.adBuffDamage, leftTime = 30*60 } },
            {new SaveDataBuffAD_LeftData(){ buffADType = EnumDefinition.RewardTypeAD.adBuffSpeed, leftTime = 30*60 } },
            {new SaveDataBuffAD_LeftData(){ buffADType = EnumDefinition.RewardTypeAD.adBuffGold, leftTime = 30*60 } },
        };
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
        dnaData.haveCount = inGameData.haveCount;
        //dnaData.power = inGameData.power;
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

    public void SetEvolutionSlotData(SaveDataEvolutionSolt data)
    {
        if (saveDataTotal.saveDataEvolution.saveDataEvolutionSolts.Any(a => a.slotId == data.slotId))
        {
            var slotData = saveDataTotal.saveDataEvolution.saveDataEvolutionSolts.FirstOrDefault(f => f.slotId == data.slotId);
            slotData.slotId = data.slotId;
            slotData.symbolId = data.symbolId;
            slotData.value = data.value;
            slotData.evolutionDiceStatType = data.evolutionDiceStatType;
            slotData.clorHexCode = data.clorHexCode;
            slotData.evolutionGrade = data.evolutionGrade;
        }
        else
        {
            saveDataTotal.saveDataEvolution.saveDataEvolutionSolts.Add(data);
        }
    }

    public SaveDataEvolution GetEvolutionData()
    {
        return saveDataTotal.saveDataEvolution;
    }




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
        // Debug.Log($"union id : {union.unionId}, => union.isEquip : {union.isEquip}, unionEquipSlot : {unionEquipSlot.slotIndex}");

        var equipSlotID = unionEquipSlot != null ? unionEquipSlot.slotIndex : 999;

        union.equipSlotId = equipSlotID;//unionEquipSlot?.slotIndex ?? 999;
    }

    public void SaveUnionUnEquioSlot(UnionSlot unionSlot)
    {
        var union = GetSaveDataUnion(unionSlot);
        union.isEquip = false;
        union.equipSlotId = 999;
    }


    public SaveDataUnion GetSaveDataUnion(UnionSlot unionSlot)
    {
        var inGmaeData = unionSlot.inGameData;
        var unionID = inGmaeData.unionIndex;
        var union = GetUnionData(unionID);
        //        Debug.Log($"유니온 ID : {unionID}, union : {union}");
        return union;
    }

    SaveDataUnion GetUnionData(int unionId)
    {
        return saveDataTotal.saveDataUnions.unions.FirstOrDefault(f => f.unionId == unionId);
    }


    //스킬 데이터 세팅
    public void SaveSkillData(SkillType skillType, Skill_InGameData skill_InGameData)
    {
        SaveDataSkill skillData = GetSaveDataByType(saveDataTotal.saveDataSkills.saveDataSkills,
            f => f.skillType == skillType, skillType.ToString());

        skillData.skillType = skillType;
        skillData.level = skill_InGameData.level;
        //skillData.damage = skill_InGameData.damage;
        skillData.isUsingSkill = skill_InGameData.isSkilUsing;
        skillData.leftCoolTime = skill_InGameData.skillLeftCoolTime;
        skillData.isUnLock = true;
        //skillData.duaration = skill_InGameData.duaration;
        //skillData.power = skill_InGameData.power;


        //skillData.skill_InGameData = skill_InGameData;
        //skillData.leftSkillTime = skill_InGameData.skillLeftTime;
    }

    public SaveDataSkill GetSaveDataSkill(SkillType skillType)
    {
        return GetSaveDataByType(saveDataTotal.saveDataSkills.saveDataSkills,
            f => f.skillType == skillType, skillType.ToString());
    }


    public void SetSkillLeftCoolTime(SkillType skillType, float leftTime)
    {
        SaveDataSkill skillData = GetSaveDataSkill(skillType);
        skillData.leftCoolTime = leftTime;
    }

    public void SetSkillUsingValue(SkillType skillType, bool isUsing)
    {
        SaveDataSkill skillData = GetSaveDataSkill(skillType);
        skillData.isUsingSkill = isUsing;
    }

    public void SetSkillCooltime(SkillType skillType, bool isCoolTime)
    {
        SaveDataSkill skillData = GetSaveDataSkill(skillType);
        skillData.isCooltime = isCoolTime;
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
    public void SaveDataGoodsGold(float gold)
    {
        saveDataTotal.saveDataGoods.gold = gold;
    }
    public void SaveDataGoodsGem(float gem)
    {
        saveDataTotal.saveDataGoods.gem = gem;
    }
    public void SaveDataGoodsBone(float bone)
    {
        saveDataTotal.saveDataGoods.bone = bone;
    }
    public void SaveDataGoodsDice(float dice)
    {
        saveDataTotal.saveDataGoods.dice = dice;
    }
    public void SaveDataGoodsCoal(float coal)
    {
        saveDataTotal.saveDataGoods.coal = coal;
    }
    public void SaveDataGoodsClearTicket(float clearTicket)
    {
        saveDataTotal.saveDataGoods.clearTicket = clearTicket;
    }
    public void SaveDataGoodsUnionTicket(float unionTicket)
    {
        saveDataTotal.saveDataGoods.unionTicket = unionTicket;
    }

    public void SaveDataGoodsDnaTicket(float dnaTicket)
    {
        saveDataTotal.saveDataGoods.dnaTicket = dnaTicket;
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
    public void SaveDataGoodsDungeonKey(GoodsType goodsType, float dungeonKey)
    {
        switch (goodsType)
        {
            case GoodsType.gold:
                saveDataTotal.saveDataGoods.dungeonKeyGold = dungeonKey; break;
            case GoodsType.bone:
                saveDataTotal.saveDataGoods.dungeonKeyBone = dungeonKey; break;
            case GoodsType.dice:
                saveDataTotal.saveDataGoods.dungeonKeyDice = dungeonKey; break;
            case GoodsType.coal:
                saveDataTotal.saveDataGoods.dungeonKeyCoal = dungeonKey; break;
        }
    }

    // 던전 광고 키 데이터 저장
    public void SaveDataGoodsDungeonADKey(GoodsType goodsType, float dungeonADKey)
    {
        switch (goodsType)
        {
            case GoodsType.gold:
                saveDataTotal.saveDataGoods.dungeonKeyADGold = dungeonADKey; break;
            case GoodsType.bone:
                saveDataTotal.saveDataGoods.dungeonKeyADBone = dungeonADKey; break;
            case GoodsType.dice:
                saveDataTotal.saveDataGoods.dungeonKeyADDice = dungeonADKey; break;
            case GoodsType.coal:
                saveDataTotal.saveDataGoods.dungeonKeyADCoal = dungeonADKey; break;
        }
    }

    /*
        ref int GetGoodsDungenKeyData(GoodsType goodsType)
        {
            switch (goodsType)
            {
                case GoodsType.gold:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyGold;
                case GoodsType.bone:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyBone;
                case GoodsType.dice:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyDice;
                case GoodsType.coal:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyCoal;
                default:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyGold;
            }
        }

        ref int GetGoodsDungenADKeyData(GoodsType goodsType)
        {
            switch (goodsType)
            {
                case GoodsType.gold:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyADGold;
                case GoodsType.bone:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyADBone;
                case GoodsType.dice:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyADDice;
                case GoodsType.coal:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyADCoal;
                default:
                    return ref saveDataTotal.saveDataGoods.dungeonKeyADGold;
            }
        }
    */

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

    // 캐슬 데이터 세팅
    public void SaveDataCastMineleLevel(int level)
    {
        saveDataTotal.saveDataCastle.mineLevel = level;
    }
    public void SaveDataCastleFactoryLevel(int level)
    {
        saveDataTotal.saveDataCastle.factoryLevel = level;
    }
    public void SaveDataCastleSaveGold(float gold)
    {
        saveDataTotal.saveDataCastle.savedGold = gold;
    }
    public void SaveDataCastleSaveBone(float bone)
    {
        saveDataTotal.saveDataCastle.savedBone = bone;
    }


    // 투토리얼 스텝 데이터 세팅
    public void SaveDataTutorialStep(int step)
    {
        saveDataTotal.saveDataSystem.tutorial_step = step;
    }

    // 투토리얼 세트 데이터 세팅
    public void SaveDataTutorialSetID(int setId)
    {
        saveDataTotal.saveDataTutorial.tutorialSetId = setId;
    }


    // 유니온 소환 등급 데이터 세팅
    public void SaveDataUnionSummonGradeLevel(int gradeLevel)
    {
        saveDataTotal.saveDataUnionSummonGrade.summonGradeLevel = gradeLevel;
    }

    // 유니온 토탈 갬블 카운트 데이터 세팅
    public void SaveDataUnionTotalGambleCount(int count)
    {
        saveDataTotal.saveDataUnionSummonGrade.totalGambleCount = count;
    }

    // 유니온 획득 리워드 아이디 데이터 세팅
    public void SaveDataUnionAddRewardId(int id)
    {
        saveDataTotal.saveDataUnionSummonGrade.rewaedUnionIds.Add(id);
    }


    // 캐슬 -> 연구소 데이터 세팅
    public void SaveDataLabBuildIngameData(LabBuildIngameData data)
    {
        var saveData = saveDataTotal.saveDataLabBuildingData.labBuildIngameDatas.Find(x => x.goodsType == data.goodsType);
        saveData.level = data.level;
        saveData.value = data.value;
        saveData.price = data.price;
    }

    // SaveData Remove UnionRewardId
    public void SaveDataRemoveUnionRewardId(int id)
    {
        //saveDataTotal.saveDataUnionSummonGrade.rewaedUnionIds.Remove(id);
        saveDataTotal.saveDataUnionSummonGrade.rewaedUnionIds_Remove.Add(id);
    }



    // save left time buff ad 
    public void SetSaveDataBuffAD_LeftTime(EnumDefinition.RewardTypeAD buffADType, float leftTime)
    {
        var buffAD_LeftData = GetSaveDataBuffAD_LeftDataByType(buffADType);
        buffAD_LeftData.leftTime = leftTime;
    }

    // set using buff ad
    public void SetSaveDataBuffAD_Using(EnumDefinition.RewardTypeAD buffADType, bool isUsing)
    {
        var buffAD_LeftData = GetSaveDataBuffAD_LeftDataByType(buffADType);
        buffAD_LeftData.isUsing = isUsing;
    }

    public SaveDataBuffAD_LeftData GetSaveDataBuffAD_LeftDataByType(EnumDefinition.RewardTypeAD buffADType)
    {
        return saveDataTotal.saveDataBuffAD.buffAD_LeftDatas.FirstOrDefault(x => x.buffADType == buffADType);
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
    public SaveDataCastle saveDataCastle;
    public SaveDataUnionSummonGrade saveDataUnionSummonGrade;
    public SaveDataLabBuildingData saveDataLabBuildingData;
    public SaveDataBuffAD saveDataBuffAD;
}



[System.Serializable]
public class SaveDataTutorial
{
    public int tutorial_step;
    public int tutorialSetId;
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
    // 슬롯 데이터 저장
    public List<SaveDataEvolutionSolt> saveDataEvolutionSolts = new List<SaveDataEvolutionSolt>();
}

[System.Serializable]
public class SaveDataEvolutionSolt
{
    public int slotId;
    public int symbolId;
    public float value;
    public string evolutionDiceStatType;
    public string clorHexCode;
    public int evolutionGrade;



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
    //public double power;
    public int dnaIndex;
    public int haveCount;
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
    //public double damage;
    public bool isUsingSkill;     // 스킬 사용중 표시
    //public float leftSkillTime; // 스킬 남은 시간  
    public float leftCoolTime;    // 스킬 쿨타임 남은 시간
    public bool isCooltime;       // 스킬 쿨타임중 표시
    public EnumDefinition.SkillType skillType;
    public bool isUnLock;
    //public float duaration;
    //public double power;

    //public Skill_InGameData skill_InGameData;
}

[System.Serializable]
public class SaveDataDungeonLevel
{
    public int dungeonLvGold = 0;
    public int dungeonLvBone = 0;
    public int dungeonLvDice = 0;
    public int dungeonLvCoal = 0;
}

[System.Serializable]
public class SaveDataShop
{
    public bool isBuyVip1;
    public bool isBuyVip2;
    public bool isBuyVip3;
    public bool isBuyVip4;
    public bool isBuyStaterPack;
    public bool isBuyAdRemove;
    public bool isBuyDungeonPack;
    public bool isBuyBattlePass;
    public bool isBuyFastestPack;
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
    public float gold;
    public float gem;
    public float bone;
    public float dice;
    public float coal;
    public float clearTicket;
    public float unionTicket;
    public float dnaTicket;
    public float dungeonKeyGold;
    public float dungeonKeyBone;
    public float dungeonKeyCoal;
    public float dungeonKeyDice;
    public float dungeonKeyADGold;
    public float dungeonKeyADBone;
    public float dungeonKeyADCoal;
    public float dungeonKeyADDice;
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
    // 게임 종료 시간
    public string quitTime;

}

[System.Serializable]
public class SaveDataCastle
{
    public int mineLevel;
    public int factoryLevel;
    public float savedGold;
    public float savedBone;
}


[System.Serializable]
public class SaveDataUnionSummonGrade
{
    public int summonGradeLevel = 0;
    public int totalGambleCount = 0;
    public List<int> rewaedUnionIds = new List<int>();
    public List<int> rewaedUnionIds_Remove = new List<int>();
}


[System.Serializable]
public class SaveDataLabBuildingData
{
    public List<LabBuildIngameData> labBuildIngameDatas = new List<LabBuildIngameData>();
}


[System.Serializable]
public class SaveDataBuffAD
{
    public List<SaveDataBuffAD_LeftData> buffAD_LeftDatas = new List<SaveDataBuffAD_LeftData>();
}

[System.Serializable]
public class SaveDataBuffAD_LeftData
{
    public EnumDefinition.RewardTypeAD buffADType;
    public float leftTime;
    public bool isUsing;
}

#endregion



