using System;
using System.Collections;
using UnityEngine;
using static EnumDefinition;


public class Player : MonoBehaviour
{
    // 스테이지 레벨
    public int stageIdx;
    public int upgradeLevelIdx;
    // 훈련 레벨
    public int traningLevelIdx;

    public int gold;
    public int bone;
    public int gem;
    public int coal;
    // 소탕권
    public int clearTicket;

    // Dungeon Key
    public SerializableDictionary<GoodsType, int> dungeonKeys;

    // Dungeon AD Key
    public SerializableDictionary<GoodsType, int> dungeonADKeys;

    public DateTime playTime;
    public float currentMonsterHp;

    // 주사위 개수
    public int diceCount;


    /// <summary> 현재 진행중인 스테이지 데이터 </summary>
    public StageData currentStageData;
    public int pahseCountOriginalValue;

    /// <summary> 현재 전투중인 몬스터 </summary>
    public MonsterBase currentMonster;

    /// <summary> 현재 전투중인 몬스터 타입 </summary>
    public MonsterType curMonsterType;

    /// <summary> 직전에 전투한 몬스터 타입 </summary>
    public MonsterType prevMonsterType;


    /// <summary> 현재 플레이어의 스탯 데이터 </summary>


    // 기본 몬스터를 제외한 몬스터 사냥중일때
    public bool isSpacialMonsterHunting;

    // 보스 몬스터 도전 가능 상태 판단
    public bool isBossMonsterChllengeEnable = false;

    // 던전 몬스터 최종 클리어 레벨
    public DungeonMonsterClearLevel dungeonMonsterClearLevel;


    void Start()
    {

    }

    private void Awake()
    {

    }

    public IEnumerator Init(SaveData saveData)
    {
        SetPlayerDataFromSaveData(saveData);
        yield return null;


    }


    public void SetCurrentMonster(MonsterBase monsterBase)
    {
        currentMonster = monsterBase;
    }


    public MonsterBase GetCurrntMonster()
    {
        return currentMonster;
    }

    public void SetCurrentMonsterType(MonsterType monsterType)
    {
        curMonsterType = monsterType;
    }

    public void SetPervMonsterType(MonsterType monsterType)
    {
        prevMonsterType = monsterType;
    }


    public void SetCurrentMonsterHP(float hpValue)
    {
        currentMonsterHp = hpValue;

        //* (1- GlobalData.instance.statManager.MonsterHpLess())
    }


    public void SetPlayerDataFromSaveData(SaveData saveData)
    {
        stageIdx = saveData.stageIdx;
        upgradeLevelIdx = saveData.upgradeLevelIdx;
        gold = saveData.gold;
        diceCount = saveData.dice;
        bone = saveData.bone;
        gem = saveData.gem;
        coal = saveData.coal;
        clearTicket = saveData.clearTicker;


        //TODO: 재화 추가 로직 필요
        dungeonKeys[GoodsType.gold] = saveData.dungeonKeyGold;
        dungeonKeys[GoodsType.bone] = saveData.dungeonKeyBone;
        dungeonKeys[GoodsType.dice] = saveData.dungeonKeyDice;
        dungeonKeys[GoodsType.coal] = saveData.dungeonKeyCoal;

        dungeonADKeys[GoodsType.gold] = saveData.dungeonKeyADGold;
        dungeonADKeys[GoodsType.bone] = saveData.dungeonKeyADBone;
        dungeonADKeys[GoodsType.dice] = saveData.dungeonKeyADDice;
        dungeonADKeys[GoodsType.coal] = saveData.dungeonKeyADCoal;

        SetCurrentStageData(stageIdx);

        // 던전 몬스터 클리어 레벨 데이터 로드
        dungeonMonsterClearLevel = new DungeonMonsterClearLevel();
        dungeonMonsterClearLevel.goldLv = saveData.dungeonLvGold;
        dungeonMonsterClearLevel.boneLv = saveData.dungeonLvBone;
        dungeonMonsterClearLevel.diceLv = saveData.dungeonLvDice;
        dungeonMonsterClearLevel.coalLv = saveData.dungeonLvCoal;
    }

    // 던전 입장 키 초기화 ( 2개씩 지급 )
    public void AddAllDungeonKeys()
    {
        AddDungeonADKey(GoodsType.gold, 2);
        AddDungeonADKey(GoodsType.bone, 2);
        AddDungeonADKey(GoodsType.dice, 2);
        AddDungeonADKey(GoodsType.coal, 2);
        AddDungeonKey(GoodsType.gold, 2);
        AddDungeonKey(GoodsType.bone, 2);
        AddDungeonKey(GoodsType.dice, 2);
        AddDungeonKey(GoodsType.coal, 2);
    }

    public void SetCurrentStageData(int stageIdx)
    {
        var stageData = GlobalData.instance.dataManager.GetStageDataById(stageIdx); ;
        currentStageData = stageData;
        pahseCountOriginalValue = stageData.phaseCount;
    }

    public void AddGold(int value)
    {
        gold += value;
        GlobalData.instance.traningManager.EnableBuyButtons(); // RELOAD BTN UI
        GlobalData.instance.skillManager.EnableBuyButtons();// RELOAD BTN UI
        GlobalData.instance.uiController.SetTxtGold(gold, value); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsGold(gold); // set save data
    }

    public void AddBone(int value)
    {
        bone += value;
        GlobalData.instance.traningManager.EnableBuyButtons(); // RELOAD BTN UI
        GlobalData.instance.uiController.SetTxtBone(bone, value); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsBone(bone); // set save data
    }
    public void AddDice(int value)
    {
        diceCount += value;
        GlobalData.instance.uiController.SetTxtDice(diceCount); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsDice(diceCount); // set save data
    }


    public void AddCoal(int value)
    {
        coal += value;
        // set ui;
        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsCoal(coal);
    }

    public void AddClearTicket(int value)
    {
        clearTicket += value;

        // set ui 
        GlobalData.instance.dungeonEnterPopup.SetTxtClierTicket(clearTicket);
        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsClearTicket(clearTicket);
    }



    public void AddGem(int value)
    {
        gem += value;
        GlobalData.instance.uiController.SetTxtGem(gem, value); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsGem(gem); // set save data
    }

    public void PayGold(int value)
    {
        gold -= value;
        if (gold < 0) gold = 0;
        GlobalData.instance.traningManager.EnableBuyButtons(); // RELOAD BTN UI
        GlobalData.instance.skillManager.EnableBuyButtons();// RELOAD BTN UI
        GlobalData.instance.uiController.SetTxtGold(gold, 0); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsGold(gold); // set save data
    }

    public void PayBone(int value)
    {
        bone -= value;
        if (bone < 0) bone = 0;
        GlobalData.instance.traningManager.EnableBuyButtons(); // RELOAD BTN UI
        GlobalData.instance.uiController.SetTxtBone(bone, 0); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsBone(bone); // set save data
    }
    public void PayDice(int value)
    {
        diceCount -= value;
        if (diceCount < 0) diceCount = 0;
        GlobalData.instance.uiController.SetTxtDice(diceCount); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsDice(diceCount); // set save data
    }
    public void PayGem(int value)
    {
        gem -= value;
        if (gem < 0) gem = 0;
        GlobalData.instance.uiController.SetTxtGem(gem, 0); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsGem(gem); // set save data
    }

    public void PayCoal(int value)
    {
        coal -= value;
        if (coal < 0) coal = 0;

        // set ui

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsCoal(coal);
    }

    public void PayClearTicekt(int value)
    {
        clearTicket -= value;
        if (clearTicket < 0) clearTicket = 0;

        // set ui 
        GlobalData.instance.dungeonEnterPopup.SetTxtClierTicket(clearTicket);

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsClearTicket(clearTicket);
    }

    public void AddDungeonKey(GoodsType goodsType, int addKeyCount)
    {
        dungeonKeys[goodsType] += addKeyCount;
        if (dungeonKeys[goodsType] > 2) dungeonKeys[goodsType] = 2; // 던전 키 최대 보유수 2개 제한

        // RELOAD UI
        // ...

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonKey(goodsType, dungeonKeys[goodsType]);
    }

    public void PayDungeonKey(GoodsType goodsType, int keyCount)
    {
        dungeonKeys[goodsType] -= keyCount;
        if (dungeonKeys[goodsType] < 0) dungeonKeys[goodsType] = 0;

        // RELOAD UI
        // ...

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonKey(goodsType, dungeonKeys[goodsType]);
    }

    // ADD DUNGEON AD KEY BY GOODS TYPE
    public void AddDungeonADKey(GoodsType goodsType, int addKeyCount)
    {
        dungeonADKeys[goodsType] += addKeyCount;
        if (dungeonADKeys[goodsType] > 2) dungeonADKeys[goodsType] = 2; // 던전 키 최대 보유수 2개 제한
        // RELOAD UI
        // ...

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonADKey(goodsType, dungeonADKeys[goodsType]);
    }

    // PAY DUNGEON AD KEY BY GOODS TYPE
    public void PayDungeonADKey(GoodsType goodsType, int keyCount)
    {
        dungeonADKeys[goodsType] -= keyCount;
        if (dungeonADKeys[goodsType] < 0) dungeonADKeys[goodsType] = 0;

        // RELOAD UI
        // ...

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonADKey(goodsType, dungeonADKeys[goodsType]);
    }


    public int GetCurrentDungeonKeyCount(MonsterType monsterType)
    {
        switch (monsterType)
        {
            case MonsterType.dungeonGold: return dungeonKeys[GoodsType.gold];
            case MonsterType.dungeonBone: return dungeonKeys[GoodsType.bone];
            case MonsterType.dungeonDice: return dungeonKeys[GoodsType.dice];
            case MonsterType.dungeonCoal: return dungeonKeys[GoodsType.coal];
            default: return 0;
        }
    }

    public void PayDungeonKeyByMonsterType(MonsterType monsterType, int count)
    {
        switch (monsterType)
        {
            case MonsterType.dungeonGold: PayDungeonKey(GoodsType.gold, count); break;
            case MonsterType.dungeonBone: PayDungeonKey(GoodsType.bone, count); break;
            case MonsterType.dungeonDice: PayDungeonKey(GoodsType.dice, count); break;
            case MonsterType.dungeonCoal: PayDungeonKey(GoodsType.coal, count); break;
        }
    }

    // get dungeon ad key count by monster type
    public int GetDungeonADKeyCountByMonsterType(MonsterType monsterType)
    {
        switch (monsterType)
        {
            case MonsterType.dungeonGold: return dungeonADKeys[GoodsType.gold];
            case MonsterType.dungeonBone: return dungeonADKeys[GoodsType.bone];
            case MonsterType.dungeonDice: return dungeonADKeys[GoodsType.dice];
            case MonsterType.dungeonCoal: return dungeonADKeys[GoodsType.coal];
            default: return 0;
        }
    }

    // pay dungeon ad key by monster type
    public void PayDungeonADKeyByMonsterType(MonsterType monsterType, int count)
    {
        switch (monsterType)
        {
            case MonsterType.dungeonGold: PayDungeonADKey(GoodsType.gold, count); break;
            case MonsterType.dungeonBone: PayDungeonADKey(GoodsType.bone, count); break;
            case MonsterType.dungeonDice: PayDungeonADKey(GoodsType.dice, count); break;
            case MonsterType.dungeonCoal: PayDungeonADKey(GoodsType.coal, count); break;
        }
    }



}


[System.Serializable]
public class DungeonMonsterClearLevel
{
    public int goldLv = 1;
    public int boneLv = 1;
    public int diceLv = 1;
    public int coalLv = 1;

    public void SetLevelGold(int level)
    {
        goldLv = level;

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataDungeonLevelGold(goldLv);
    }
    public void SetLevelBone(int level)
    {
        boneLv = level;

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataDungeonLevelBone(boneLv);
    }
    public void SetLevelDice(int level)
    {
        diceLv = level;
        // set save data;
        GlobalData.instance.saveDataManager.SaveDataDungeonLevelDice(diceLv);
    }
    public void SetLevelCoal(int level)
    {
        coalLv = level;
        // set save data;
        GlobalData.instance.saveDataManager.SaveDataDungeonLevelCoal(coalLv);
    }

    public int GetLeveByDungeonMonType(EnumDefinition.MonsterType monsterType)
    {
        switch (monsterType)
        {
            case MonsterType.dungeonGold: return goldLv;
            case MonsterType.dungeonBone: return boneLv;
            case MonsterType.dungeonDice: return diceLv;
            case MonsterType.dungeonCoal: return coalLv;
            default: return 0;
        }
    }

    public void SetLevelFromMonsterType(EnumDefinition.MonsterType monsterType, int level)
    {
        switch (monsterType)
        {
            case MonsterType.dungeonGold: SetLevelGold(level); break;
            case MonsterType.dungeonBone: SetLevelBone(level); break;
            case MonsterType.dungeonDice: SetLevelDice(level); break;
            case MonsterType.dungeonCoal: SetLevelCoal(level); break;
        }
    }
}

