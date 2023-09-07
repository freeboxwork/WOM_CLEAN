/// <summary>
/// 곤충생성 N회 ( N = customConditions)
/// </summary>
using UnityEngine;
public class Pattern_01 : PatternBase
{
    public int touchCount = 0;
    public int targetTouchCount = 0;




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
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnTutoInsectCreate, CountingInsectCreate);
    }
    void RemoveEvent()
    {
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnTutoInsectCreate, CountingInsectCreate);
    }

    void CountingInsectCreate()
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
                else
                {
                    var txt = $"화면을 터치하여 곤충을 생성하세요. <#00ff15>{targetTouchCount - touchCount}</color> 회 남음";
                    tutorialManager.tutorialUiCont.SetTxtDesc(txt);
                    Debug.Log(" 투토리얼 1 현재 곤충 생성 횟수 : " + touchCount);
                }
            }
        }
    }



    public override void EventStart(TutorialStep stepData)
    {
        // Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        // UI hide
        GlobalData.instance.uiController.MainMenuHide();
        SideUIMenuHide(true);

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
        GlobalData.instance.uiController.MainMenuShow();
        SideUIMenuHide(false);
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
        touchCount = 0;
        targetTouchCount = stepData.customConditions;
    }


    void SideUIMenuHide(bool isHide)
    {
        UtilityMethod.GetCustomTypeGMById(15).SetActive(!isHide);
        UtilityMethod.GetCustomTypeGMById(16).SetActive(!isHide);
    }

}
