/// <summary>
/// 곤충 자동 생성 실행
/// </summary>
using UnityEngine;
public class Pattern_10 : PatternBase
{

    void Start()
    {

    }

    void InsectSpwanStart()
    {
        if (enableEvent)
        {
            if (IsTypeTextAnimEnd())
            {
                GlobalData.instance.insectSpwanManager.AllTimerStart();
                StepClear();
            }
        }
    }

    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        EnableEvent(true);
        InsectSpwanStart();
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
