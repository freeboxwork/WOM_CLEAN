/// <summary>
/// 보스 도전 성공 
/// </summary>
using UnityEngine;

public class Pattern_07 : PatternBase
{
    int targetKillCount;
    int curremtKillCount;

    void Start()
    {
        AddEvent();
    }
    void OnDestroy()
    {
        RemoveEvent();
    }

    void AddEvent()
    {
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnMonsterKillBossMonster, OnBossMonsterKill);
    }
    void RemoveEvent()
    {
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnMonsterKillBossMonster, OnBossMonsterKill);
    }

    void OnBossMonsterKill()
    {
        if (enableEvent)
        {
            ++curremtKillCount;
            if (curremtKillCount >= targetKillCount)
                StepClear();
        }
    }

    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        SetGoalData(stepData);
        EnableEvent(true);
    }

    public override void ResetGoalData()
    {
        targetKillCount = 0;
        curremtKillCount = 0;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        targetKillCount = stepData.customConditions;
        curremtKillCount = 0;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
