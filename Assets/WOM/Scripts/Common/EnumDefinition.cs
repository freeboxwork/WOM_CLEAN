
public class EnumDefinition
{

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
        questDataRepeat,
    }

    /// <summary> ?õà?†® Î©îÎâ¥ </summary>
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


    /// <summary> ?û¨?ôî????ûÖ </summary>
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
        none,
    }


    public enum MenuPanelType
    {
        training,
        evolution,
        skill,
        castle,
        dungeon,
        shop,
        union,
        dna,
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
        insectBossDamage
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
        /// <summary> Í≥§Ï∂© Í≥µÍ≤©?†• Ï¶ùÍ?? </summary>
        insectDamageUp,
        /// <summary> ?ú†?ãà?ò® Í≥µÍ≤©?†• Ï¶ùÍ?? </summary>
        unionDamageUp,
        /// <summary> ?ïÑÍµ? ?†ÑÏ≤? ?Üç?èÑ Ï¶ùÍ?? </summary>
        allUnitSpeedUp,
        /// <summary> Í≥®Îìú ?öç?ìù?üâ Ï¶ùÍ?? </summary>
        glodBonusUp,
        /// <summary> Í¥¥Ïàò Î™¨Ïä§?Ñ∞ ?ì±?û• </summary>
        monsterKing,
        /// <summary> ?ïÑÍµ? ?†ÑÏ≤? ÏπòÎ™Ö???  </summary>
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
        BGM_01,

    }
    public enum SFX_TYPE
    {
        MONSTER_HIT,
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

}
