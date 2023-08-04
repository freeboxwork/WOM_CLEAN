/// <summary>
/// 골드 획득 했을때 
/// </summary>
using UnityEngine;

public class Pattern_04 : PatternBase
{

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
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnTutorialAddGold, OnAddGole);
    }
    void RemoveEvent()
    {
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnTutorialAddGold, OnAddGole);
    }

    void OnAddGole()
    {
        if (enableEvent)
        {
            if (IsTypeTextAnimEnd())
                StepClear();
        }
    }


    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
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
