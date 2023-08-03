/// <summary>
/// 화면 터치 1회 
/// </summary>
using UnityEngine;
public class Pattern_02 : PatternBase
{
    int touchCount = 0;
    int targetTouchCount = 1;
    public TutorialManager tutorialManager;

    bool isTextTypeEnd = false;

    void Start()
    {

    }

    void Update()
    {
        if (enableEvent)
        {
            if (Input.GetMouseButtonDown(0) && IsTypeTextAnimEnd())
            {
                touchCount++;
                if (touchCount >= targetTouchCount)
                {
                    StepClear();
                }
            }
        }
    }

    bool IsTypeTextAnimEnd()
    {
        return tutorialManager.tutorialUiCont.isTypeAnim;
    }

    public override void EventStart(TutorialStep stepData)
    {
        SetGoalData(stepData);
        EnableText(stepData);
        EnableEvent(true);
    }
    void EnableText(TutorialStep stepData)
    {
        // enable description text
        tutorialManager.tutorialUiCont.EnableDiscriptionText(stepData.description);
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }

    public override void ResetGoalData()
    {

    }

    public override void SetGoalData(TutorialStep stepData)
    {

    }

}
