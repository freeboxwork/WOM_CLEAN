

public class CallBackEventType
{
    public enum TYPES
    {
        OnMonsterHit,                   /// <summary> 곤충이 몬스터를 타격 했을때  </summary>
        OnBossMonsterChallenge,         /// <summary> 보스 몬스터 도전 버튼을 눌렀을때  </summary>
        OnBossMonsterChallengeTimeOut,  /// <summary> 보스 몬스터 도전 타이머 종료 되었을때  </summary>
        OnEvolutionMonsterChallenge,    /// <summary> 진화전 버튼 눌렀을때  </summary>
        OnReloaUITranuing,              /// <summary> 훈련 UI 금액에 따라 버튼 활성 , 비활성화 시킬때  </summary>
        OnReloaUISkill,                 /// <summary> 스킬 UI 금액에 따라 버튼 활성 , 비활성화 시킬때  </summary>
        OnDungeonMonsterChallenge,      /// <summary> 던전 몬스터 도전 버튼을 눌렀을때  </summary>
        OnDungeonMonsterHit,            /// <summary> 곤충이 던전 몬스터를 타격 했을때  </summary>
        OnQusetClearOneDayCounting,     /// <summary> 일일 퀘스트 완료 하여 카운트 할때  </summary>
        OnQusetUsingRewardOneDay,       /// <summary> 일일 퀘스트 보상 받았을때  </summary>
        OnQusetUsingRewardOneDayAD,     /// <summary> 일일 퀘스트 광고 보상 받았을때  </summary>
        OnQusetClearRepeatCounting,     /// <summary> 반복 퀘스트 완료 했을때  </summary>
        OnQuestCompleteBattlePassStage, /// <summary> 배틀패스 - 스테이지 퀘스트 완료 했을때  </summary>
        OnUsingRewardAttend,            /// <summary> 출석 보상 받았을때  </summary>
        OnUsingRewardNewUserEvent,      /// <summary> 신규 유저 이벤트 보상 받았을때  </summary>
        OnGoldPigEvent,                 /// <summary> 황금 돼지 이벤트 획득 했을때 </summary>
        OnMonsterKingHit,               /// <summary> 몬스터 킹 스킬 사용 후 몬스터를 타격 했을때 ( 콜라이더가 부딪혔을때 ) </summary>
        OnTutoInsectCreate,             /// <summary> 튜토리얼 - 곤충 생성 했을때 </summary>
        OnTutorialAddGold,              /// <summary> 튜토리얼 - 골드 획득 했을때 </summary>
        OnTutorialBtnClick,             /// <summary> 튜토리얼 - 버튼 클릭 했을때 </summary>
        OnTutorialScreenClick,          /// <summary> 튜토리얼 - Pattern2에 해당하는 바탕화면(버튼) 클릭 했을때 </summary>
        OnStageInGoldMonster,           /// <summary> 튜토리얼 - 골드 몬스터 도달 했을때 </summary>
        OnMonsterKillGoldMonster,       /// <summary> 튜토리얼 - 골드 몬스터를 처치 했을때 </summary>
        OnMonsterKillBossMonster,       /// <summary> 튜토리얼 - 보스 몬스터를 처치 했을때 </summary>
        OnMonsterKillFailedBossMonster,  /// <summary> 튜토리얼 - 보스 몬스터를 처치 실패 했을때 </summary>   
        OnTutorialUnionGamblingEnd,      /// <summary> 튜토리얼 - 유니온 갬블링 종료 했을때 </summary>
        None,
    }
}
