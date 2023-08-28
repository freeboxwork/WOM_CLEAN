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

    public long gold;
    public long bone;
    public long gem;
    public long coal;

    // 소탕권
    public long clearTicket;
    public long unionTicket;
    public long dnaTicket;

    // Dungeon Key
    public SerializableDictionary<GoodsType, long> dungeonKeys;

    // Dungeon AD Key
    public SerializableDictionary<GoodsType, long> dungeonADKeys;

    // Goods Map
    //SerializableDictionary<RewardType, int> rewardToGoodsMap;

    public DateTime playTime;
    public double currentMonsterHp;

    // 주사위 개수
    public long diceCount;


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
        //SetRewardToGoodsMap();
        yield return null;
    }

    // void SetRewardToGoodsMap()
    // {
    //     rewardToGoodsMap = new SerializableDictionary<RewardType, int>{

    //         {RewardType.gold, GetGold() },
    //         {RewardType.bone, GetBone() },
    //         {RewardType.gem,  GetGem() },
    //         {RewardType.dice, GetDice() },
    //         {RewardType.coal, GetCoal() },
    //         {RewardType.clearTicket, GetClearTicket() },
    //         {RewardType.unionTicket, GetUnionTicket() },
    //         {RewardType.dnaTicket, GetDnaTicket() }
    //     };
    // }

    // 인자로 RewardType 을 받고 switch를 이용하여 해당 재화를 리턴하는 함수
    public long GetGoodsByRewardType(RewardType rewardType)
    {
        switch (rewardType)
        {
            case RewardType.gold: return gold;
            case RewardType.bone: return bone;
            case RewardType.gem: return gem;
            case RewardType.dice: return diceCount;
            case RewardType.coal: return coal;
            case RewardType.clearTicket: return clearTicket;
            case RewardType.unionTicket: return unionTicket;
            case RewardType.dnaTicket: return dnaTicket;
            default: return 0;
        }
    }




    // int GetGold()
    // {
    //     return gold;
    // }
    // int GetBone()
    // {
    //     return bone;
    // }
    // int GetGem()
    // {
    //     return gem;
    // }
    // int GetDice()
    // {
    //     return diceCount;
    // }

    // int GetCoal()
    // {
    //     return coal;
    // }
    // int GetClearTicket()
    // {
    //     return clearTicket;
    // }
    // int GetUnionTicket()
    // {
    //     return unionTicket;
    // }
    // int GetDnaTicket()
    // {
    //     return dnaTicket;
    // }




    // public int GetGoodsByRewardType(RewardType rewardType)
    // {
    //     return rewardToGoodsMap[rewardType];
    // }


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


    public void SetCurrentMonsterHP(double hpValue)
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
        unionTicket = saveData.unionTicket;
        dnaTicket = saveData.dnaTicket;


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
        if (dungeonKeys[GoodsType.gold] < 2)
            AddDungeonKey(GoodsType.gold, 2);

        if (dungeonKeys[GoodsType.bone] < 2)
            AddDungeonKey(GoodsType.bone, 2);

        if (dungeonKeys[GoodsType.dice] < 2)
            AddDungeonKey(GoodsType.dice, 2);

        if (dungeonKeys[GoodsType.coal] < 2)
            AddDungeonKey(GoodsType.coal, 2);


        AddDungeonADKey(GoodsType.gold, 1);
        AddDungeonADKey(GoodsType.bone, 1);
        AddDungeonADKey(GoodsType.dice, 1);
        AddDungeonADKey(GoodsType.coal, 1);
    }

    public void SetCurrentStageData(int stageIdx)
    {
        var stageData = GlobalData.instance.dataManager.GetStageDataById(stageIdx); ;
        currentStageData = stageData;
        pahseCountOriginalValue = stageData.phaseCount;
    }

    public void AddGold(long value)
    {

        var result = value * (1 + (GlobalData.instance.statManager.GetTalentGoldBonus() * 0.01f));

        // ad buff 적용
        var buffValue = GlobalData.instance.adManager.GetBuffAdSlotByType(EnumDefinition.RewardTypeAD.adBuffGold).addValue;
        gold += (long)(result * buffValue);



        GlobalData.instance.traningManager.EnableBuyButtons(); // RELOAD BTN UI
        GlobalData.instance.skillManager.EnableBuyButtons();// RELOAD BTN UI
        GlobalData.instance.uiController.SetTxtGold(gold, (long)result); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsGold(gold); // set save data
    }

    public void AddBone(long value)
    {
        var result = value * (1 + (GlobalData.instance.statManager.BoneBonus() * 0.01f));

        bone += (long)result;
        GlobalData.instance.traningManager.EnableBuyButtons(); // RELOAD BTN UI
        GlobalData.instance.uiController.SetTxtBone(bone, (long)result); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsBone(bone); // set save data
    }
    public void AddDice(long value)
    {
        diceCount += value;
        GlobalData.instance.uiController.SetTxtDice(diceCount); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsDice(diceCount); // set save data
    }


    public void AddCoal(long value)
    {
        coal += value;
        // set ui;
        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsCoal(coal);
        // ui update
        GlobalData.instance.uiController.SetTxtCoal(coal);
    }

    public void AddClearTicket(long value)
    {
        clearTicket += value;

        // set ui 
        GlobalData.instance.dungeonEnterPopup.SetTxtClierTicket(clearTicket);
        GlobalData.instance.uiController.SetTxtDungeonClearTicketCount(value);

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsClearTicket(clearTicket);
    }



    public void AddGem(long value)
    {
        gem += value;
        GlobalData.instance.uiController.SetTxtGem(gem, value); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsGem(gem); // set save data
    }

    public void AddUnionTicket(long value)
    {
        unionTicket += value;
        GlobalData.instance.uiController.SetTxtUnionTicket(unionTicket); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsUnionTicket(unionTicket); // set save data
    }

    public void AddDnaTicket(long value)
    {
        dnaTicket += value;
        GlobalData.instance.uiController.SetTxtDnaTicket(dnaTicket); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsDnaTicket(dnaTicket); // set save data
    }

    public void PayUnionTicket(long value)
    {
        unionTicket -= value;
        if (unionTicket < 0) unionTicket = 0;
        GlobalData.instance.uiController.SetTxtUnionTicket(unionTicket); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsUnionTicket(unionTicket); // set save data

        // button interactable check    
        GlobalData.instance.uiController.ButtonInteractableCheck(EnumDefinition.RewardType.unionTicket);
    }

    public void PayDnaTicket(long value)
    {
        dnaTicket -= value;
        if (dnaTicket < 0) dnaTicket = 0;
        GlobalData.instance.uiController.SetTxtDnaTicket(dnaTicket); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsDnaTicket(dnaTicket); // set save data

        // button interactable check    
        GlobalData.instance.uiController.ButtonInteractableCheck(EnumDefinition.RewardType.dnaTicket);
    }

    public void PayGold(long value)
    {
        gold -= value;
        if (gold < 0) gold = 0;
        GlobalData.instance.traningManager.EnableBuyButtons(); // RELOAD BTN UI
        GlobalData.instance.skillManager.EnableBuyButtons();// RELOAD BTN UI
        GlobalData.instance.uiController.SetTxtGold(gold, 0); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsGold(gold); // set save data
    }

    public void PayBone(long value)
    {
        bone -= value;
        if (bone < 0) bone = 0;
        GlobalData.instance.traningManager.EnableBuyButtons(); // RELOAD BTN UI
        GlobalData.instance.uiController.SetTxtBone(bone, 0); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsBone(bone); // set save data
    }
    public void PayDice(long value)
    {
        diceCount -= value;
        if (diceCount < 0) diceCount = 0;
        GlobalData.instance.uiController.SetTxtDice(diceCount); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsDice(diceCount); // set save data
    }
    public void PayGem(long value)
    {
        gem -= value;
        if (gem < 0) gem = 0;
        GlobalData.instance.uiController.SetTxtGem(gem, 0); // RELOAD UI
        GlobalData.instance.saveDataManager.SaveDataGoodsGem(gem); // set save data

        // button interactable check    
        GlobalData.instance.uiController.ButtonInteractableCheck(EnumDefinition.RewardType.gem);
    }

    public void PayCoal(long value)
    {
        coal -= value;
        if (coal < 0) coal = 0;

        // set ui
        // ui update
        GlobalData.instance.uiController.SetTxtCoal(coal);
        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsCoal(coal);
    }

    public void PayClearTicekt(long value)
    {
        clearTicket -= value;
        if (clearTicket < 0) clearTicket = 0;

        // set ui 
        GlobalData.instance.dungeonEnterPopup.SetTxtClierTicket(clearTicket);
        GlobalData.instance.uiController.SetTxtDungeonClearTicketCount(clearTicket);


        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsClearTicket(clearTicket);
    }

    public void AddDungeonKey(GoodsType goodsType, long addKeyCount)
    {
        dungeonKeys[goodsType] += addKeyCount;
        //if (dungeonKeys[goodsType] > 2) dungeonKeys[goodsType] = 2; // 던전 키 최대 보유수 2개 제한

        // RELOAD UI
        switch (goodsType)
        {
            case GoodsType.gold: GlobalData.instance.uiController.SetTxtDungeonGoldKeyCount(dungeonKeys[goodsType]); break;
            case GoodsType.bone: GlobalData.instance.uiController.SetTxtDungeonBoneKeyCount(dungeonKeys[goodsType]); break;
            case GoodsType.dice: GlobalData.instance.uiController.SetTxtDungeonDiceKeyCount(dungeonKeys[goodsType]); break;
            case GoodsType.coal: GlobalData.instance.uiController.SetTxtDungeonCoalKeyCount(dungeonKeys[goodsType]); break;
        }


        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonKey(goodsType, dungeonKeys[goodsType]);
    }

    public void PayDungeonKey(GoodsType goodsType, long keyCount)
    {
        dungeonKeys[goodsType] -= keyCount;
        if (dungeonKeys[goodsType] < 0) dungeonKeys[goodsType] = 0;

        // RELOAD UI
        switch (goodsType)
        {
            case GoodsType.gold: GlobalData.instance.uiController.SetTxtDungeonGoldKeyCount(dungeonKeys[goodsType]); break;
            case GoodsType.bone: GlobalData.instance.uiController.SetTxtDungeonBoneKeyCount(dungeonKeys[goodsType]); break;
            case GoodsType.dice: GlobalData.instance.uiController.SetTxtDungeonDiceKeyCount(dungeonKeys[goodsType]); break;
            case GoodsType.coal: GlobalData.instance.uiController.SetTxtDungeonCoalKeyCount(dungeonKeys[goodsType]); break;
        }

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonKey(goodsType, dungeonKeys[goodsType]);
    }

    // ADD DUNGEON AD KEY BY GOODS TYPE
    public void AddDungeonADKey(GoodsType goodsType, long addKeyCount)
    {
        dungeonADKeys[goodsType] += addKeyCount;
        if (dungeonADKeys[goodsType] > 1) dungeonADKeys[goodsType] = 1; // 던전 키 최대 보유수 1개 제한
        // RELOAD UI
        // ...

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonADKey(goodsType, dungeonADKeys[goodsType]);
    }

    // PAY DUNGEON AD KEY BY GOODS TYPE
    public void PayDungeonADKey(GoodsType goodsType, long keyCount)
    {
        dungeonADKeys[goodsType] -= keyCount;
        if (dungeonADKeys[goodsType] < 0) dungeonADKeys[goodsType] = 0;

        // RELOAD UI
        // ...

        // set save data;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonADKey(goodsType, dungeonADKeys[goodsType]);
    }

    public void ResetDungeonADKeys()
    {
        dungeonADKeys[GoodsType.gold] = 1;
        dungeonADKeys[GoodsType.bone] = 1;
        dungeonADKeys[GoodsType.dice] = 1;
        dungeonADKeys[GoodsType.coal] = 1;
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonADKey(GoodsType.gold, 1);
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonADKey(GoodsType.bone, 1);
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonADKey(GoodsType.dice, 1);
        GlobalData.instance.saveDataManager.SaveDataGoodsDungeonADKey(GoodsType.coal, 1);
    }

    public long GetCurrentDungeonKeyCount(MonsterType monsterType)
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

    public void PayDungeonKeyByMonsterType(MonsterType monsterType, long count)
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
    public long GetDungeonADKeyCountByMonsterType(MonsterType monsterType)
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
    public void PayDungeonADKeyByMonsterType(MonsterType monsterType, long count)
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
    public int goldLv = 0;
    public int boneLv = 0;
    public int diceLv = 0;
    public int coalLv = 0;

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

