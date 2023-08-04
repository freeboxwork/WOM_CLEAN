
public class EnumDefinition
{

    public enum PatternType
    {
        pattern_01,
        pattern_02,
        pattern_03,
        pattern_04,
        pattern_05,
        pattern_06,
        pattern_07,
        pattern_08,
        pattern_09,
        pattern_10,
    }

    public enum CustomDataType
    {
        button,
        text,
        image,
        animCont,
        tr,
        gm // gameobject
    }

    public enum InsectType
    {
        mentis,
        bee,
        beetle,
        union,
        none
    }
    public enum MonsterType
    {
        normal,
        gold,
        boss,
        evolution,
        dungeon,
        dungeonGold,
        dungeonDice,
        dungeonBone,
        dungeonCoal,
        none,
    }

    public enum AttackType
    {
        normal,
    }

    public enum SheetDataType
    {
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
        buindingDataLab,
        questDataOneDay,
        battlePassData,
        attendData,
        newUserData,
        labBuildingData,
        rewardAdGemData,
    }

    /// <summary> ?????? ??? </summary>
    public enum SaleStatType
    {
        trainingDamage,
        trainingCriticalChance,
        trainingCriticalDamage,
        talentDamage,
        talentCriticalChance,
        talentCriticalDamage,
        talentMoveSpeed,
        talentSpawnSpeed,
        talentGoldBonus
    }

    public enum AnimCurveType
    {
        EaseInQuad,
        EaseOutQuad,
        EaseInOutQuad,
        EaseOutCubic,
        EaseInOutCubic,
        Spring,
        EaseInQuint,
        EaseInOutSine,
        EaseOutQuint,
        Linear
    }



    public enum GoldPosType
    {
        START_POINT_Y,
        END_POOINT_Y_MIN,
        END_POOINT_Y_MAX,
        END_POOINT_X_MIN,
        END_POOINT_X_MAX,
        SCREEN_UI_POINT_GOLD,
        SCREEN_UI_POINT_BONE
    }

    public enum UnionGradeType
    {
        normal,
        high,
        rare,
        hero,
        legend,
        unique
    }


    /// <summary> ???????????? </summary>
    public enum GoodsType
    {
        gold,
        bone,
        gem,
        dice,
        coal
    }

    public enum RewardType
    {
        gold,
        bone,
        gem,
        dice,
        coal,
        clearTicket,
        goldKey,
        boneKey,
        diceKey,
        coalKey,
        union,
        dna,
        unionTicket,
        dnaTicket,
        dungeonPass,
        none,
    }

    public enum TrainingSubPanelType
    {
        training,
        skill,
        evolution,
        none
    }

    public enum MenuPanelType
    {
        training = 0,
        castle = 1,
        dungeon = 2,
        shop = 3,
        union = 4,
        dna = 5,
        none,
    }

    public enum EvolutionDiceStatType
    {
        insectDamage,
        insectCriticalChance,
        insectCriticalDamage,
        goldBonus,
        insectMoveSpeed,
        insectSpawnTime,
        insectBossDamage,
        none
    }

    public enum EvolutionRewardGrade
    {
        NONE = 0,
        S = 1,
        A = 2,
        B = 3,
        C = 4,
        D = 5,
    }

    public enum SkillType
    {
        /// <summary> ???? ??????? ????? </summary>
        insectDamageUp,
        /// <summary> ????????? ??????? ????? </summary>
        unionDamageUp,
        /// <summary> ????? ????? ?????? ????? </summary>
        allUnitSpeedUp,
        /// <summary> ??? ????????? ????? </summary>
        glodBonusUp,
        /// <summary> ???? ????? ?????? </summary>
        monsterKing,
        /// <summary> ????? ????? ??????  </summary>
        allUnitCriticalChanceUp,
    }

    public enum UnionEquipType
    {
        Equipped,
        NotEquipped

    }

    public enum DNAType
    {
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
    }

    public enum ShopSlotType
    {
        UnionSummon,
        DNASummon,
        FreeGem,
        FreeUnionSummon,
        FreeDNASummon
    }

    public enum LotteryPageType
    {
        UNION,
        DNA
    }


    public enum BGM_TYPE
    {
        BGM_Main,
        BGM_EvolutionBoss,
        BGM_DungeonBoss,
        BGM_Castle,

    }
    public enum SFX_TYPE
    {
        MonsterHit,
        BossDie,
        ButtonUIClick,
        Coins,
        CustomPopup_Item,
        Dice,
        End_Batle,
        Evolution_Victory,
        Gamble,
        IntroPop,
        PurchaseShopProduct,
        Reward,
        S_Grade,
        Skill_1,
        Skill_2,
        Skill_3,
        Skill_4,
        Skill_5,
        Skill_6,
        Slot,
        Transition,
        Typewriter
    }

    public enum CastlePopupType
    {
        mine,
        factory,
        camp,
        lab
    }

    public enum QuestTypeOneDay
    {
        allComplete,
        showAd,
        clearDungeon,
        useSkill,
        summonUnion,
        progressStage,
        killGoldBoss,
        takeGoldPig,
        none
    }

    public enum QuestTypeRepeat
    {
        none
    }

    public enum PowerSavingMode
    {
        none,
        on,
        off
    }



    public enum RewardTypeAD
    {
        adGoldPig,
        adGem,
        adBuffDamage,
        adBuffSpeed,
        adBuffGold,
        adOffline,
        adDungeon,
    }

    public enum StageNameType
    {
        normal,
        evolution,
        dungeon,
    }

}
