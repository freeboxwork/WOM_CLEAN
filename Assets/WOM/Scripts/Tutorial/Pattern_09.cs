/// <summary>
/// 금광 몬스터 도달
/// </summary>
using UnityEngine;

public class Pattern_09 : PatternBase
{
    //int? targetStage;
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
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnStageInGoldMonster, OnStageInGoldMonster);
    }
    void RemoveEvent()
    {
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnStageInGoldMonster, OnStageInGoldMonster);
    }

    void OnStageInGoldMonster()
    {
        if (enableEvent)
        {
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

    }

    public override void SetGoalData(TutorialStep stepData)
    {

    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
