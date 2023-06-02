using System.Collections.Generic;
using UnityEngine;
using System;

public class DataModel : MonoBehaviour
{

}



[Serializable]
public class StageData
{
    public int stageId;
    public string stageName;
    public int monsterNormalId;
    public int monsterBossId;
    public int monsterGoldId;
    public int monsterEvolId;
    public int phaseCount;
    public int bgId;
    public string rewardEvent;
    public string unlock;

    public StageData CopyInstance()
    {
        return new StageData
        {
            stageId = this.stageId,
            stageName = this.stageName,
            monsterNormalId = this.monsterNormalId,
            monsterBossId = this.monsterBossId,
            monsterGoldId = this.monsterGoldId,
            monsterEvolId = this.monsterEvolId,
            phaseCount = this.phaseCount,
            bgId = this.bgId,
            rewardEvent = this.rewardEvent,
            unlock = this.unlock
        };
    }
}

[Serializable]
public class MonsterSprite
{
    public int id;
    public int tail;
    public int hand;
    public int finger;
    public int foreArm;
    public int upperArm;
    public int head;
    public int body;
    public int leg_0;
    public int leg_1;
    public int leg_2;
}


[Serializable]
public class EvolutionData
{
    public int depthId;
    public string name;
    public EnumDefinition.InsectType insectType;
    public float damage;
    public float damageRate;
    public float criticalChance;
    public float criticalDamage;
    public float speed;
    public float goldBonus;
    public float bossDamage;
    public float spawnTime;

    public EvolutionData CopyInstance()
    {
        return new EvolutionData
        {
            depthId = this.depthId,
            name = this.name,
            insectType = this.insectType,
            damage = this.damage,
            damageRate = this.damageRate,
            criticalChance = this.criticalChance,
            criticalDamage = this.criticalDamage,
            speed = this.speed,
            goldBonus = this.goldBonus,
            bossDamage = this.bossDamage,
            spawnTime = this.spawnTime
        };
    }
}

[Serializable]
public class EvolutionOptionData
{
    public int optionId;
    public int optionA;
    public int optionB;
    public int requiredLevel;
}

[Serializable]
public class UpgradeData
{
    public int id;
    public int level;
    public float upgradeCost;
    public float damage;
}

[Serializable]
public class NewData
{
    public int monsterId;
}

[Serializable]
public class MonsterData
{
    public int id;
    public float hp;
    public float exp;
    public int bone;
    public int gold;
    public int boneCount;
    public int goldCount;
    public int imageId;
    public int bgId;
    public EnumDefinition.MonsterType monsterType;
    public EnumDefinition.AttackType attackType;
}

[Serializable]
public class InsectData
{
    public string name;
    public EnumDefinition.InsectType insectType;
    public float damage;
    public float damageRate;
    public float criticalChance;
    public float ciriticalDamage;
    public float speed;
    public float goldDamage;
    public float bossDamage;
}

[Serializable]
public class UnionData
{
    public int unionIndex;
    public string gradeType;
    public int grade;
    public string name;
    public string textColor;
    public string gradeName;
    public float damage;
    public float passiveDamage;
    public float spawnTime;
    public float moveSpeed;
    public string spriteName;
    public int reqirementCount;
    public int addReqirementCount;
    public float addPassiveDamage;
    public float addDamage;
    public int maxLevel;
}

[Serializable]
public class UnionInGameData
{
    public int unionIndex;
    public int level;
    public int unionCount;
    public int LevelUpReqirementCount;
    public bool isUnlock;
    public EnumDefinition.UnionGradeType unionGradeType;

    // stat
    public float damage;
    public float spawnTime;
    public float moveSpeed;
    public float passiveDamage;



}





[Serializable]
public class TrainingData
{
    public int trainingIndex;
    public float increaseValue;
    public int maxLevel;
    public string abilityType;
    public string explanation;
}

[Serializable]
public class EvolutionGradeData
{
    public string evolutionGrade;
    public int damageRate;
    public int slotCount;
    public int penaltyStoneAmount;
}

[Serializable]
public class PolishEvolutionData
{
    public string grade;
    public float insectDamage;
    public float insectCriticalChance;
    public float insectCriticalDamage;
    public float goldBonus;
    public float insectMoveSpeed;
    public float insectSpawnTime;
    public float InsectBossDamage;
    public float gradeProbability;
}

[Serializable]
public class TalentData
{
    public int talentIndex;
    public float increaseValue;
    public int maxLevel;
    public string abilityType;
    public string explanation;
}

[Serializable]
public class DNAData
{
    public int dnaIndex;
    public float power;
    public int maxLevel;
    public string spriteName;
    public string infoFront;
    public string infoBack;
    public string dnaName;
    public string dnaType;
}

public class DNAInGameData
{
    public int level;
    public int maxLevel;
    public float power;
    public float dataPower;
    public string name;

    public void LevelUp()
    {
        ++level;
        power = (level * dataPower);
    }
}

[Serializable]
public class UnionGambleData
{
    public int summonGrade;
    public float normal;
    public float high;
    public float rare;
    public float hero;
    public float legend;
    public float unique;
}

[Serializable]
public class SummonGradeData
{
    public int level;
    public int count;
    public int rewardUnionIndex;
}


[Serializable]
/// <summary> 판매 가능한 스탯 데이터</summary>
public class StatSaleData
{
    public int level;
    public int salePrice;

    // gold 로 획득
    public float value;
    public string unitName;
}



[Serializable]
// 훈련 스탯 인게임 데이터 ( 리펙토링으로 추가됨, StatSaleData 를 대체 함 )
public class TraningInGameData
{
    public int level;
    public int salePrice;
    public float value;
    public string unitName;
    public string trainingName;
}



[Serializable]
public class SaleStatMsgData
{
    public EnumDefinition.SaleStatType saleStatType;

    public SaleStatMsgData(EnumDefinition.SaleStatType saleStatType)
    {
        this.saleStatType = saleStatType;
    }
}


[Serializable]
public class RewardEvolutionGradeData
{
    public int id;
    public string evolutionGradeType;
    public int damageRate;
    public int slotCount;
    public string nameKR;
    public string nameEN;
}

[Serializable]
public class GlobalMessageData
{
    public int id;
    public string title;
    public string popupType;
    public string message_kor;
    public string message_eng;
}

[Serializable]
public class RewardDiceEvolutionData
{
    public int grade;
    public string symbol;
    public string gradeColor;
    public float insectDamage;
    public float insectCriticalChance;
    public float insectCriticalDamage;
    public float goldBonus;
    public float insectMoveSpeed;
    public float insectSpawnTime;
    public float insectBossDamage;
    public int unLockCount;
    public float gradeProbability;
}

/// <summary>  진화 주사위 획득 값 저장 </summary>
[Serializable]
public class DiceEvolutionInGameData
{
    public float insectDamage;
    public float insectCriticalChance;
    public float insectCriticalDamage;
    public float insectMoveSpeed;
    public float insectSpawnTime;
    public float insectBossDamage;
    public float goldBonus;

    public DiceEvolutionInGameData CopyInstance()
    {
        return (DiceEvolutionInGameData)this.MemberwiseClone();
    }
}

[Serializable]
public class SkillData
{
    public int id;
    public string skillType;
    public float duration;
    public float power;
    public string name;
    public float coolTime;
    public float addDurationTime;
    public float addPowerRate;
    public int maxLevel;
    public string desctiption;
    public float defaultCost;
    public float addCostAmount;
    public int unLockLevel;
}

[Serializable]
public class Skill_InGameData
{
    public EnumDefinition.SkillType skillType;
    public int level;
    public float duaration;
    public float power;
    public float damage;
    public float coolTime;
    public string skilName;
    public bool isSkilUsing = false;
    public float skillLeftTime;
}

[Serializable]
public class TrainingElementData
{
    public int id;
    public string buttonSprite;
    public string currencySprite;
    public string trainingName;
    public string trainingType;
    public string goodsType;

}

[Serializable]
public class ConvertTextData
{
    public string en_EN;
    public string ko_KR;

    public string en_Front;
    public string en_Back;
    public string kr_Front;
    public string kr_Back;

    public string type;
}


[Serializable]
public class TutorialStep
{
    public int id;              // 튜토리얼 스텝 ID
    public int setId;           // 튜토리얼 세트 ID
    public int step;            // 튜토리얼 스텝 번호
    public int tutorialBtnId;   // 버튼 ID
    public string goal;         // 튜토리얼 목표
    public string description;  // 튜토리얼 설명
    public bool isStepComplete = false; // 현재 스탭 완료 판단
}

// 던전 몬스터 기본 정보 클래스
[Serializable]
public class DungeonMonsterBase : MonoBehaviour
{
    //public EnumDefinition.GoodsType goodsType;
    //public EnumDefinition.MonsterType monsterType;
    //public int monsterFaceId;
    ////public string currencyType; 
    ////public string monsterType;
    //public float battleTime;
    //public int maxKeyCount;
    //public int maxAdCount;
    //public int bgID;
}

// 던전 몬스터 데이터 클래스
[Serializable]
public class DungeonMonsterData
{
    public int level;
    public int currencyAmount;
    public float monsterHP;

    public DungeonMonsterData CloneInstance()
    {
        DungeonMonsterData dungeonMonsterData = new DungeonMonsterData();
        dungeonMonsterData.level = level;
        dungeonMonsterData.currencyAmount = currencyAmount;
        dungeonMonsterData.monsterHP = monsterHP;
        return dungeonMonsterData;
    }
}

[Serializable]
public class DungeonMonsterDatas
{
    public List<DungeonMonsterData> data = new List<DungeonMonsterData>();
}


// 광산, 팩토리 빌딩 데이터
[Serializable]
public class MineAndFactoryBuildingData
{
    public int level;
    public int productionCount;
    public int maxSupplyAmount;
    public int price;
    public int productionTime;
    public string currencyType;
    EnumDefinition.GoodsType goodsType;

}

public class CampBuildingData
{
    public int level;
    public int upgradeCount;
    public string reward;
}

public class LabBuildingData
{
    public int level;
    public int price;
    public int gold;
    public int bone;
    public int dice;
    public int coal;
}

// 퀘스트 관련

[System.Serializable]
public class QuestData
{
    public string questType;
    public string questName;
    public int targetValue;
    public int curCountValue;
    public string rewardType;
    public int rewardValue;
    // 하루 한번만 보상을 받을 수 있도록 하기 위한 변수
    public bool qusetComplete = false;
    // 보상을 받았는지 확인
    public bool usingReward = false;

    public QuestData CloneInstance()
    {

        QuestData questData = new QuestData();
        questData.questType = questType;
        questData.questName = questName;
        questData.targetValue = targetValue;
        questData.rewardType = rewardType;
        questData.rewardValue = rewardValue;

        //아래 항목은 저장 되어 있는 데이터가 있다면 해당 데이터로 초기화
        questData.qusetComplete = qusetComplete;
        questData.curCountValue = curCountValue;
        questData.usingReward = usingReward;

        return questData;
    }

}

