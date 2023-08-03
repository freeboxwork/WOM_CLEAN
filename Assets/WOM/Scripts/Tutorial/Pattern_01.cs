/// <summary>
/// 화면 터치 N회 ( N = customConditions)
/// </summary>
using UnityEngine;
public class Pattern_01 : PatternBase
{
    int touchCount = 0;
    int targetTouchCount = 0;
    public TutorialManager tutorialManager;

    void Start()
    {

    }

    void Update()
    {
        if (enableEvent)
        {
            if (Input.GetMouseButtonDown(0))
            {
                touchCount++;
                if (touchCount >= targetTouchCount)
                {
                    StepClear();
                }
            }
        }
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
        touchCount = 0;
        targetTouchCount = 0;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        targetTouchCount = stepData.customConditions;
    }

}
