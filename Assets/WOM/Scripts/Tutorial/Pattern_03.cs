/// <summary>
/// 투토리얼 버튼 클릭 했을때
/// </summary>
using UnityEngine;
public class Pattern_03 : PatternBase
{
    int? btnIndex = null;
    //bool isMenuClose = false;


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
            // if (isMenuClose)
            // {
            //     GlobalData.instance.uiController.CloseMainMenuPanel();
            // }

            Debug.Log("투토리얼 버튼 클릭 " + btnIndex + " " + this.btnIndex);

            if (btnIndex == this.btnIndex)
            {
                Debug.Log("투토리얼 패턴 이벤트 종료 " + patternType);
                StepClear();
            }

        }
    }

    void Update()
    {
        if (enableEvent == true)
        {
            GlobalData.instance.attackController.SetAttackableState(false);
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
        //isMenuClose = false;
        tutorialManager.tutorialUiCont.SetMaskParentImgRaycastTarget(false);
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        btnIndex = stepData.tutorialBtnId;
        // isMenuClose = stepData.customConditions == 1 ? true : false;
        // 버튼 이외의 영역을 클릭 못하게 막음
        tutorialManager.tutorialUiCont.SetMaskParentImgRaycastTarget(true);
    }
    public override void StepClear()
    {
        Debug.Log("투토리얼 패턴 이벤트 종료 " + patternType);
        ResetGoalData();
        EnableEvent(false);
        GlobalData.instance.attackController.SetAttackableState(true);
        tutorialManager.CompleteStep();
    }

    void EnableText(TutorialStep stepData)
    {
        var tutoBtn = tutorialManager.GetTutorialButtonById(stepData.tutorialBtnId);
        tutorialManager.tutorialUiCont.EnableTutorialMask(stepData.description, tutoBtn.image, tutoBtn.button, stepData.tutorialBtnId);
    }
}
