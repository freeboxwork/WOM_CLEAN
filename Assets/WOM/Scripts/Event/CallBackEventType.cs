

public class CallBackEventType
{
    public enum TYPES
    {
        OnMonsterHit,                   /// <summary> ������ ���͸� Ÿ�� ������  </summary>
        OnBossMonsterChallenge,         /// <summary> ���� ���� ���� ��ư�� ��������  </summary>
        OnBossMonsterChallengeTimeOut,  /// <summary> ���� ���� ���� Ÿ�̸� ���� �Ǿ�����  </summary>
        OnEvolutionMonsterChallenge,    /// <summary> ��ȭ�� ��ư ��������  </summary>
        OnReloaUITranuing,              /// <summary> �Ʒ� UI �ݾ׿� ���� ��ư Ȱ�� , ��Ȱ��ȭ ��ų��  </summary>
        OnReloaUISkill,                 /// <summary> ��ų UI �ݾ׿� ���� ��ư Ȱ�� , ��Ȱ��ȭ ��ų��  </summary>
        OnDungeonMonsterChallenge,      /// <summary> ���� ���� ���� ��ư�� ��������  </summary>
        OnDungeonMonsterHit,            /// <summary> ������ ���� ���͸� Ÿ�� ������  </summary>
        OnQusetClearOneDayCounting,     /// <summary> ���� ����Ʈ �Ϸ� �Ͽ� ī��Ʈ �Ҷ�  </summary>
        OnQusetUsingRewardOneDay,       /// <summary> ���� ����Ʈ ���� �޾�����  </summary>
        OnQusetUsingRewardOneDayAD,     /// <summary> ���� ����Ʈ ���� ���� �޾�����  </summary>
        OnQusetClearRepeatCounting,     /// <summary> �ݺ� ����Ʈ �Ϸ� ������  </summary>
        OnQuestCompleteBattlePassStage, /// <summary> ��Ʋ�н� - �������� ����Ʈ �Ϸ� ������  </summary>
        OnQuestCompleteBattlePassStageBuyitem, /// <summary> ��Ʋ�н� - ���� ������ ��� ������  </summary>
        OnUsingRewardAttend,            /// <summary> �⼮ ���� �޾�����  </summary>
        OnUsingRewardNewUserEvent,      /// <summary> �ű� ���� �̺�Ʈ ���� �޾�����  </summary>
        OnGoldPigEvent,                 /// <summary> Ȳ�� ���� �̺�Ʈ ȹ�� ������ </summary>
        OnMonsterKingHit,               /// <summary> ���� ŷ ��ų ��� �� ���͸� Ÿ�� ������ ( �ݶ��̴��� �ε������� ) </summary>
        OnTutoInsectCreate,             /// <summary> Ʃ�丮�� - ���� ���� ������ </summary>
        OnTutorialAddGold,              /// <summary> Ʃ�丮�� - ��� ȹ�� ������ </summary>
        OnTutorialBtnClick,             /// <summary> Ʃ�丮�� - ��ư Ŭ�� ������ </summary>
        OnTutorialScreenClick,          /// <summary> Ʃ�丮�� - Pattern2�� �ش��ϴ� ����ȭ��(��ư) Ŭ�� ������ </summary>
        OnStageInGoldMonster,           /// <summary> Ʃ�丮�� - ��� ���� ���� ������ </summary>
        OnMonsterKillGoldMonster,       /// <summary> Ʃ�丮�� - ��� ���͸� óġ ������ </summary>
        OnMonsterKillBossMonster,       /// <summary> Ʃ�丮�� - ���� ���͸� óġ ������ </summary>
        OnMonsterKillFailedBossMonster,  /// <summary> Ʃ�丮�� - ���� ���͸� óġ ���� ������ </summary>   
        OnTutorialUnionGamblingEnd,      /// <summary> Ʃ�丮�� - ���Ͽ� ���� ���� ������ </summary>


        /* QUEST EVENTS */
        OnQuestPattern_001,             /// <summary> ����Ʈ - ���� ���� ������ </summary>
        OnQuestPattern_002,             /// <summary> ����Ʈ - �ݱ� ���� óġ ������ </summary>  
        OnQuestPattern_003,             /// <summary> ����Ʈ - �������� ���� ������ </summary>
        OnQuestPattern_004,             /// <summary> ����Ʈ - �Ʒ� ���� ���� ������ </summary>
        OnQuestPattern_005,             /// <summary> ����Ʈ - ���� óġ ������ </summary>




        None,
    }
}
