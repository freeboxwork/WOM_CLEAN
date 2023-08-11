/// <summary>
/// 모든 UI 닫기
/// </summary>
using UnityEngine;

public class Pattern_15 : PatternBase
{
    void Start()
    {

    }

    void AllColseUI()
    {
        if (enableEvent)
        {
            GlobalData.instance.uiController.AllDisableUI();
            StepClear();
        }
    }

    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        EnableEvent(true);
        AllColseUI();
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