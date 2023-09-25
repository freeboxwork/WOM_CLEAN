/// <summary>
/// 특정 게임 오브젝트 닫기  ( camp popup 닫기 )
/// </summary>
using UnityEngine;
using System.Collections.Generic;

public class Pattern_13 : PatternBase
{
    int? closeObjId;
    public List<GameObject> closeObjects = new List<GameObject>();
    void Start()
    {

    }

    void DisableObject()
    {
        if (enableEvent)
        {
            GlobalData.instance.uiController.ShowFadeCanvasGroup(EnumDefinition.CanvasGroupTYPE.CAMP, false);
            //closeObjects[(int)closeObjId].SetActive(false);
            StepClear();
        }
    }

    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        SetGoalData(stepData);
        EnableEvent(true);
        DisableObject();
    }

    public override void ResetGoalData()
    {
        closeObjId = null;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        closeObjId = stepData.customConditions;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
