/// <summary>
/// 투토리얼 버튼 클릭 했을때
/// </summary>
using UnityEngine;
public class Pattern_03 : PatternBase
{
    int? btnIndex = null;

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
        EventManager.instance.AddCallBackEvent<int>(CallBackEventType.TYPES.OnTutorialBtnClick, OnClickTutoBtn);
    }
    void RemoveEvent()
    {
        EventManager.instance.RemoveCallBackEvent<int>(CallBackEventType.TYPES.OnTutorialBtnClick, OnClickTutoBtn);
    }

    void OnClickTutoBtn(int btnIndex)
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
        SetGoalData(stepData);
        EnableText(stepData);
        EnableEvent(true);

    }

    public override void ResetGoalData()
    {
        btnIndex = null;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        btnIndex = stepData.tutorialBtnId;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }

    void EnableText(TutorialStep stepData)
    {
        var tutoBtn = tutorialManager.GetTutorialButtonById(stepData.tutorialBtnId);
        tutorialManager.tutorialUiCont.EnableTutorialMask(stepData.description, tutoBtn.image, tutoBtn.button);
    }
}
