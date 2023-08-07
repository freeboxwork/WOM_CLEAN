/// <summary>
///  GEM N개 추가 ( 추가 조건 )
/// </summary>
using UnityEngine;
public class Pattern_11 : PatternBase
{
    int addGem;
    void Start()
    {

    }


    void OnAddGem()
    {
        if (enableEvent)
        {
            GlobalData.instance.player.AddGem(addGem);
            StepClear();
        }
    }


    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        SetGoalData(stepData);
        EnableEvent(true);
        OnAddGem();
    }

    public override void ResetGoalData()
    {
        addGem = 0;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        addGem = stepData.customConditions;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
