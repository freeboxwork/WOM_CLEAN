/// <summary>
/// 특정 투토리얼게임 오브젝트 활성/비활성
/// </summary>

using UnityEngine;

public class Pattern_16 : PatternBase
{
    TutorialGameObject targetObj;
    bool isActive = false;
    void Start()
    {

    }

    void ActiveTutorialGameObject()
    {
        if (enableEvent)
        {
            targetObj.gameObject.SetActive(isActive);
            StepClear();
        }
    }

    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        SetGoalData(stepData);
        EnableEvent(true);
        ActiveTutorialGameObject();
    }

    public override void ResetGoalData()
    {
        targetObj = null;
        isActive = false;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        Debug.Log("투토리얼 버튼 아이디 " + stepData.tutorialBtnId);
        targetObj = tutorialManager.GetTutorialGameObjectById(stepData.tutorialBtnId);
        isActive = stepData.customConditions == 1 ? true : false;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
