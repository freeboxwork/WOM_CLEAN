

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
        OnQusetClearRepeatCounting,     /// <summary> 반복 퀘스트 완료 했을때  </summary>

        None,
    }
}
