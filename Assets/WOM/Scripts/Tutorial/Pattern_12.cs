using UnityEngine;

public class Pattern_12 : PatternBase
{
    bool isGambleing;
    void Start()
    {

    }


    void SetUnionGambleingState()
    {
        if (enableEvent)
        {
            tutorialManager.SetUnionGambleingState(isGambleing);
            StepClear();
        }
    }


    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        SetGoalData(stepData);
        EnableEvent(true);
        SetUnionGambleingState();
    }

    public override void ResetGoalData()
    {
        isGambleing = false;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        isGambleing = stepData.customConditions == 1 ? true : false;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
