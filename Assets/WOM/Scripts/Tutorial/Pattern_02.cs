/// <summary>
/// 화면 터치 1회 
/// </summary>
using UnityEngine;
public class Pattern_02 : PatternBase
{
    int touchCount = 0;
    int targetTouchCount = 1;

    bool isTextTypeEnd = false;

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
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnTutorialScreenClick, OnBtnCkickScreen);
    }
    void RemoveEvent()
    {
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnTutorialScreenClick, OnBtnCkickScreen);
    }

    void OnBtnCkickScreen()
    {
        if (enableEvent)
        {
            if (IsTypeTextAnimEnd())
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
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
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
        tutorialManager.tutorialUiCont.ActiveScreenBtn(false);
        touchCount = 0;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        tutorialManager.tutorialUiCont.ActiveScreenBtn(true);
        touchCount = 0;
    }

}
