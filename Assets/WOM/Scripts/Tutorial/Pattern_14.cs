/// <summary>
/// 특정 게임 오브젝트 닫기  ( camp popup 닫기 )
/// </summary>
using UnityEngine;

public class Pattern_14 : PatternBase
{
    bool adPass = false;

    void Start()
    {

    }

    void AdPass()
    {
        if (enableEvent)
        {
            tutorialManager.isAdPass = adPass;
            StepClear();
        }
    }

    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        SetGoalData(stepData);
        EnableEvent(true);
        AdPass();
    }

    public override void ResetGoalData()
    {
        adPass = false;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        adPass = stepData.customConditions == 1 ? true : false;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
