using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using static EnumDefinition;
using System;

public class DataManager : MonoBehaviour
{

    /*
        evolutionData_bee,
        evolutionData_beetle,
        evolutionData_mentis,
        evolutionOptionData_bee,
        evolutionOptionData_beetle,
        evolutionOptionData_mentis,
        monsterData_boss,
        monsterData_gold,
        monsterData_normal,
        monsterData_evolution,
        stageData,
        upgradeData,
        monsterSpriteData,
        unionGambleData,
        summonGradeData,
        rewardEvolutionGradeData,
        rewardDiceEvolutionData,
        skillData,
        unionData,
        dnaData,
        TrainingElementData,
        convertTextData,
        monsterDataDungeonGold,
        monsterDataDungeonDice,
        monsterDataDungeonBone,
        monsterDataDungeonCoal,
        buildingDataMine,
        buildingDataFactory,
        buindingDataCamp,
        buindingDataLab

     */


    public List<TextAsset> sheetDatas = new List<TextAsset>();


    /*
        trainingDamageData,
        trainingCriticalChanceData,
        trainingCriticalDamageData,
        talentDamageData,
        talentCriticalChanceData,
        talentCriticalDamageData,
        talentMoveSpeedData,
        talentSpawnSpeed,
        talentGoldBonusData
     */

    public List<TextAsset> sheetDatas_saleStats = new List<TextAsset>();

    public StageDatas stageDatas;
    public List<EvolutionDatas> evolutionDatas;
    public List<EvolutionOptionDatas> evolutionOptionDatas;
    public List<MonsterDatas> monsterDatas;
    public UnionGambleDatas unionGambleDatas;
    public SummonGradeDatas summonGradeDatas;
    public UpgradeDatas upgradeData;
    public MonsterSprites monsterSpriteData;

    // REWARD DATA
    public RewardEvolutionGradeDatas rewardEvolutionGradeDatas;
    public RewardDiceEvolutionDatas rewardDiceEvolutionDatas;

    // SKILL DATA
    public SkillDatas skillDatas;

    // UNION DATA
    public UnionDatas unionDatas;

    // DNA DATA
    public DNADatas dnaDatas;

    // TRAINING ELEMENT DATA
    public TrainingElementDatas trainingElementDatas;

    // CONVERT TEXT DATA
    public ConvertTextDatas convertTextDatas;

    // GLOBAL POPUP DATA
    public TextAsset globalPopupSheetData;
    public GlobalMessageDatas globalMessageDatas;

    // DUNGEN MONSTER DATA 
    public DungeonMonsterDatas dungeonMonsterDataGold;
    public DungeonMonsterDatas dungeonMonsterDataDice;
    public DungeonMonsterDatas dungeonMonsterDataBone;
    public DungeonMonsterDatas dungeonMonsterDataCoal;



    // Traning Data ( 판매 데이터 )
    StatSaleDatas trainingDamageDatas;
    StatSaleDatas trainingCriticalChanceData;
    StatSaleDatas trainingCriticalDamageData;
    StatSaleDatas talentDamageData;
    StatSaleDatas talentCriticalChanceData;
    StatSaleDatas talentCriticalDamageData;
    StatSaleDatas talentMoveSpeedData;
    StatSaleDatas talentSpawnSpeed;
    StatSaleDatas talentGoldBonusData;
    public List<StatSaleDatas> statSaleDatas = new List<StatSaleDatas>();


    // 캐슬 데이터.
    public MineAndFactoryBuildingDatas buildDatasMine;
    public MineAndFactoryBuildingDatas buildDatasFactory;
    public CampBuildingData buildDatasCamp;
    public LabBuildingDatas buildDataLab;

    // 퀘스트 데이터
    public QuestDatas questDatasOneDay;

    void Start()
    {

    }

    public IEnumerator SetDatas()
    {
        // STAGE
        SetStageData();

        // EVOLUTION
        SetEvolutionData();

        // EVOLUTION OPTION
        SetEvolutionOptionData();

        // GLOBAL MSEEAGE
        SetGlobalMessageData();

        // REWARD EVOLUTION GRADE ,  REWARD EVOLUTION DICE DATA
        SetRewardData();

        // SKILL DATA
        SetSkillData();

        // UNION DATA
        SetUnionData();

        // DNA DATA
        SetDNAData();

        // TRAINING ELEMENT DATA
        SetTraningElementsData();

        // CONVERT TEXT DATA
        SetConvertTextDatas();

        // MONSTER
        SetMonsterData();

        // UPGRADE
        SetUpgradeData();

        // MONSTER SPRETE
        SetMonsterSpriteData();

        // SET UNION GAMBLE DATA ( 뽑기 데이터 , 뽑기 그레이드 데이터 )
        SetGamleData();

        // ADD SALE STAT LIST
        AddSaleStatElements();

        // SET SALE STAT DATA ( 판매 가능한 스탯 데이터 )
        SetSaleStatDatas();

        // SET DUNGEON DATA
        SetDungeonData();

        // SET CASTLE DATA
        SetCastleData();

        // SET QUEST DATA
        SetQuestData();

        yield return new WaitForEndOfFrame();
    }



    void AddSaleStatElements()
    {
        statSaleDatas.Add(trainingDamageDatas);
        statSaleDatas.Add(trainingCriticalChanceData);
        statSaleDatas.Add(trainingCriticalDamageData);
        statSaleDatas.Add(talentDamageData);
        statSaleDatas.Add(talentCriticalChanceData);
        statSaleDatas.Add(talentCriticalDamageData);
        statSaleDatas.Add(talentMoveSpeedData);
        statSaleDatas.Add(talentSpawnSpeed);
        statSaleDatas.Add(talentGoldBonusData);
    }


    void SetSaleStatDatas()
    {
        for (int i = 0; i < sheetDatas_saleStats.Count; i++)
        {
            statSaleDatas[i] = JsonUtility.FromJson<StatSaleDatas>(sheetDatas_saleStats[i].text);
        }
    }

    void SetGamleData()
    {
        unionGambleDatas = GetData<UnionGambleDatas>(SheetDataType.unionGambleData);
        summonGradeDatas = GetData<SummonGradeDatas>(SheetDataType.summonGradeData);
    }

    void SetDungeonData()
    {
        dungeonMonsterDataGold = GetData<DungeonMonsterDatas>(SheetDataType.monsterDataDungeonGold);
        dungeonMonsterDataDice = GetData<DungeonMonsterDatas>(SheetDataType.monsterDataDungeonDice);
        dungeonMonsterDataBone = GetData<DungeonMonsterDatas>(SheetDataType.monsterDataDungeonBone);
        dungeonMonsterDataCoal = GetData<DungeonMonsterDatas>(SheetDataType.monsterDataDungeonCoal);
    }

    void SetCastleData()
    {
        buildDatasMine = GetData<MineAndFactoryBuildingDatas>(SheetDataType.buildingDataMine);
        buildDatasFactory = GetData<MineAndFactoryBuildingDatas>(SheetDataType.buildingDataFactory);
        buildDatasCamp = GetData<CampBuildingData>(SheetDataType.buindingDataCamp);
        buildDataLab = GetData<LabBuildingDatas>(SheetDataType.buindingDataLab);
    }

    void SetQuestData()
    {
        questDatasOneDay = GetData<QuestDatas>(SheetDataType.questDataOneDay);
    }

    public MineAndFactoryBuildingData GetBuildDataMineByLevel(int level)
    {
        //any 를 이용한 예외처리
        if (buildDatasMine.data.Any(f => f.level == level) == false)
        {
            Debug.Log("Mine Data is not exist. level : " + level);
            return null;
        }
        return buildDatasMine.data.Find(f => f.level == level);
    }

    public MineAndFactoryBuildingData GetBuildDataFactoryByLevel(int level)
    {
        //any 를 이용한 예외처리
        if (buildDatasFactory.data.Any(f => f.level == level) == false)
        {
            Debug.Log("Factory Data is not exist. level : " + level);
            return null;
        }
        return buildDatasFactory.data.Find(f => f.level == level);
    }




    void SetStageData()
    {
        stageDatas = GetData<StageDatas>(SheetDataType.stageData);
    }
    private void SetGlobalMessageData()
    {
        globalMessageDatas = GetData<GlobalMessageDatas>(globalPopupSheetData);
    }
    void SetEvolutionData()
    {
        // 0 : mentins , 1 : bee , 2 : beetle
        var mentis = GetData<EvolutionDatas>(SheetDataType.evolutionData_mentis);
        var bee = GetData<EvolutionDatas>(SheetDataType.evolutionData_bee);
        var beetle = GetData<EvolutionDatas>(SheetDataType.evolutionData_beetle);

        // set type
        mentis.data.ForEach(f => f.insectType = InsectType.mentis);
        bee.data.ForEach(f => f.insectType = InsectType.bee);
        beetle.data.ForEach(f => f.insectType = InsectType.beetle);

        evolutionDatas.Add(mentis);
        evolutionDatas.Add(bee);
        evolutionDatas.Add(beetle);
    }
    void SetEvolutionOptionData()
    {
        // 0 : mentins , 1 : bee , 2 : beetle
        var mentis = GetData<EvolutionOptionDatas>(SheetDataType.evolutionOptionData_mentis);
        var bee = GetData<EvolutionOptionDatas>(SheetDataType.evolutionOptionData_bee);
        var beetle = GetData<EvolutionOptionDatas>(SheetDataType.evolutionOptionData_beetle);

        evolutionOptionDatas.Add(mentis);
        evolutionOptionDatas.Add(bee);
        evolutionOptionDatas.Add(beetle);
    }

    void SetRewardData()
    {
        rewardEvolutionGradeDatas = GetData<RewardEvolutionGradeDatas>(SheetDataType.rewardEvolutionGradeData);
        rewardDiceEvolutionDatas = GetData<RewardDiceEvolutionDatas>(SheetDataType.rewardDiceEvolutionData);
    }

    private void SetSkillData()
    {
        skillDatas = GetData<SkillDatas>(SheetDataType.skillData);
    }

    void SetUnionData()
    {
        unionDatas = GetData<UnionDatas>(SheetDataType.unionData);
    }

    void SetDNAData()
    {
        dnaDatas = GetData<DNADatas>(SheetDataType.dnaData);
    }

    void SetTraningElementsData()
    {
        trainingElementDatas = GetData<TrainingElementDatas>(SheetDataType.TrainingElementData);
    }

    void SetConvertTextDatas()
    {
        convertTextDatas = GetData<ConvertTextDatas>(SheetDataType.convertTextData);
    }

    void SetMonsterData()
    {
        var boss = GetData<MonsterDatas>(SheetDataType.monsterData_boss);
        var gold = GetData<MonsterDatas>(SheetDataType.monsterData_gold);
        var normal = GetData<MonsterDatas>(SheetDataType.monsterData_normal);
        var evolution = GetData<MonsterDatas>(SheetDataType.monsterData_evolution);

        // set type
        boss.data.ForEach(f => f.monsterType = MonsterType.boss);
        gold.data.ForEach(f => f.monsterType = MonsterType.gold);
        normal.data.ForEach(f => f.monsterType = MonsterType.normal);
        evolution.data.ForEach(f => f.monsterType = MonsterType.evolution);

        // 0 : normal , 1: gold , 2 : boss , 3 : evolution
        monsterDatas.Add(normal);
        monsterDatas.Add(gold);
        monsterDatas.Add(boss);
        monsterDatas.Add(evolution);
    }
    void SetUpgradeData()
    {
        upgradeData = GetData<UpgradeDatas>(SheetDataType.upgradeData);
    }

    void SetMonsterSpriteData()
    {
        monsterSpriteData = GetData<MonsterSprites>(SheetDataType.monsterSpriteData);
    }

    T GetData<T>(SheetDataType sheetDataType)
    {
        var data = GetSheetData(sheetDataType);
        return JsonUtility.FromJson<T>(data.text);
    }
    T GetData<T>(TextAsset sheetData)
    {
        return JsonUtility.FromJson<T>(sheetData.text);
    }




    public StageData GetStageDataById(int stageId)
    {
        return stageDatas.data.FirstOrDefault(f => f.stageId == stageId);
    }

    public EvolutionData GetEvolutionDataById(InsectType insectType, int id)
    {
        return evolutionDatas[TypeIdx(insectType)].data.FirstOrDefault(f => f.depthId == id);
    }

    public EvolutionOptionData GetEvolutionOptionDataById(InsectType insectType, int id)
    {
        return evolutionOptionDatas[TypeIdx(insectType)].data.FirstOrDefault(f => f.optionId == id);
    }

    public MonsterData GetMonsterDataById(MonsterType monsterType, int id)
    {
        return monsterDatas[TypeIdx(monsterType)].data.FirstOrDefault(f => f.id == id);
    }

    public DungeonMonsterDatas GetBuGetDungeonMonsterDatasByType(EnumDefinition.MonsterType monsterType)
    {
        switch (monsterType)
        {
            case MonsterType.dungeonGold: return dungeonMonsterDataGold;
            case MonsterType.dungeonDice: return dungeonMonsterDataDice;
            case MonsterType.dungeonBone: return dungeonMonsterDataBone;
            case MonsterType.dungeonCoal: return dungeonMonsterDataCoal;
            default: return null;
        }
    }

    public DungeonMonsterData GetDungeonMonsterDataByTypeLevel(EnumDefinition.MonsterType monsterType, int level)
    {
        switch (monsterType)
        {
            case MonsterType.dungeonGold: return GetDungeonMonsterDataByLevel(dungeonMonsterDataGold, level);
            case MonsterType.dungeonDice: return GetDungeonMonsterDataByLevel(dungeonMonsterDataDice, level);
            case MonsterType.dungeonBone: return GetDungeonMonsterDataByLevel(dungeonMonsterDataBone, level);
            case MonsterType.dungeonCoal: return GetDungeonMonsterDataByLevel(dungeonMonsterDataCoal, level);
            default: return null;
        }
    }

    public int GetDungeonMonsterKillRewardByLevel(MonsterType monsterType, int targetLevel)
    {
        var dataList = GetBuGetDungeonMonsterDatasByType(monsterType);

        if (!dataList.data.Any(data => data.level == targetLevel))
        {
            Debug.LogError($"Level {targetLevel} data not found.");
            return 0;
        }

        int totalCurrencyAmount = dataList.data.Where(data => data.level <= targetLevel).Sum(data => data.currencyAmount);
        return totalCurrencyAmount;
    }


    DungeonMonsterData GetDungeonMonsterDataByLevel(DungeonMonsterDatas monsterDatas, int level)
    {
        var maxLevel = monsterDatas.data.Max(f => f.level);
        if (maxLevel >= level)
        {
            return monsterDatas.data.FirstOrDefault((f) => f.level == level);
        }
        else
        {
            // 데이터 없음
            Debug.Log("맥시멈 레벨 도달 하였습니다.");
            return new DungeonMonsterData { level = 999, currencyAmount = 1, monsterHP = 999 };
        }
    }

    public UpgradeData GetUpgradeDataById(int id)
    {
        return upgradeData.data.FirstOrDefault(f => f.id == id);
    }

    public MonsterSprite GetMonsterSpriteDataById(int id)
    {
        return monsterSpriteData.data.FirstOrDefault(f => f.id == id);
    }

    public UnionGambleData GetUnionGambleDataBySummonGrade(int summonGrade)
    {
        return unionGambleDatas.data.FirstOrDefault(f => f.summonGrade == summonGrade);
    }

    public SummonGradeData GetSummonGradeDataByLevel(int level)
    {
        return summonGradeDatas.data.FirstOrDefault(f => f.level == level);
    }


    TextAsset GetSheetData(SheetDataType sheetDataType)
    {
        var idx = (int)sheetDataType;
        return sheetDatas[idx];
    }

    int TypeIdx(InsectType insectType)
    {
        return (int)insectType;
    }
    int TypeIdx(MonsterType monsterType)
    {
        return (int)monsterType;
    }

    public StatSaleData GetSaleStatDataByTypeId(SaleStatType statType, int level)
    {
        return statSaleDatas[(int)statType].data.FirstOrDefault(f => f.level == level);
    }

    public StatSaleDatas GetSaleStatDataByType(SaleStatType statType)
    {
        return statSaleDatas[(int)statType];
    }

    public RewardEvolutionGradeData GetRewaedEvolutionGradeDataByID(int id)
    {
        return rewardEvolutionGradeDatas.data.FirstOrDefault(f => f.id == id);
    }


    public GlobalMessageData GetGlobalMessageDataById(int id)
    {
        return globalMessageDatas.data.FirstOrDefault(f => f.id == id);
    }

    public RewardDiceEvolutionData GetRewardDiceEvolutionDataByGradeId(int id)
    {
        var data = rewardDiceEvolutionDatas?.data?.FirstOrDefault(f => f.grade == id);
        if (data == null)
        {
            throw new ArgumentNullException("rewardDiceEvolutionDatas", "Could not find data for the given grade id. - " + id);
        }
        return data;
        //return rewardDiceEvolutionDatas.data.FirstOrDefault(f => f.grade == id); 
    }

    public SkillData GetSkillDataById(int id)
    {
        return skillDatas.data.FirstOrDefault(f => f.id == id);
    }

    public DNAData GetDNADataById(int id)
    {
        return dnaDatas.data.FirstOrDefault(f => f.dnaIndex == id);
    }

    public TrainingElementData GetTrainingElementData(EnumDefinition.SaleStatType type)
    {
        return trainingElementDatas.data.FirstOrDefault(f => f.trainingType == type.ToString());
    }

    public ConvertTextData GetConvertTextDataByEvolutionDiceStatType(EvolutionDiceStatType type)
    {
        return convertTextDatas.data.FirstOrDefault(f => f.type == type.ToString());
    }

}


[Serializable]
public class StageDatas
{
    public List<StageData> data = new List<StageData>();
}

[Serializable]
public class EvolutionDatas // insect Datas
{
    public List<EvolutionData> data = new List<EvolutionData>();
}

[Serializable]
public class EvolutionOptionDatas
{
    public List<EvolutionOptionData> data = new List<EvolutionOptionData>();
}

[Serializable]
public class MonsterDatas
{
    public List<MonsterData> data = new List<MonsterData>();
}

[Serializable]
public class UpgradeDatas
{
    public List<UpgradeData> data = new List<UpgradeData>();
}


[Serializable]
public class MonsterSprites
{
    public List<MonsterSprite> data = new List<MonsterSprite>();
}

[Serializable]
public class UnionGambleDatas
{
    public List<UnionGambleData> data = new List<UnionGambleData>();
}

[Serializable]
public class SummonGradeDatas
{
    public List<SummonGradeData> data = new List<SummonGradeData>();
}

[Serializable]
public class StatSaleDatas
{
    public List<StatSaleData> data = new List<StatSaleData>();
}

[Serializable]
public class RewardEvolutionGradeDatas
{
    public List<RewardEvolutionGradeData> data = new List<RewardEvolutionGradeData>();
}

[Serializable]
public class GlobalMessageDatas
{
    public List<GlobalMessageData> data = new List<GlobalMessageData>();
}

[Serializable]
public class RewardDiceEvolutionDatas
{
    public List<RewardDiceEvolutionData> data = new List<RewardDiceEvolutionData>();
}

[Serializable]
public class SkillDatas
{
    public List<SkillData> data = new List<SkillData>();
}

[Serializable]
public class UnionDatas
{
    public List<UnionData> data = new List<UnionData>();
}

[Serializable]
public class DNADatas
{
    public List<DNAData> data = new List<DNAData>();
}

[Serializable]
public class TrainingElementDatas
{
    public List<TrainingElementData> data = new List<TrainingElementData>();
}


[Serializable]
public class ConvertTextDatas
{
    public List<ConvertTextData> data = new List<ConvertTextData>();
}

[Serializable]
public class MineAndFactoryBuildingDatas
{
    public List<MineAndFactoryBuildingData> data = new List<MineAndFactoryBuildingData>();
}

[Serializable]
public class CampBuildingDatas
{
    public List<CampBuildingData> data = new List<CampBuildingData>();
}

[Serializable]
public class LabBuildingDatas
{
    public List<LabBuildingData> data = new List<LabBuildingData>();
}


[Serializable]
public class QuestDatas
{
    public List<QuestData> data = new List<QuestData>();
}