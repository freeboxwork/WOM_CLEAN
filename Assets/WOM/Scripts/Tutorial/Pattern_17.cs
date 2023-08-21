using UnityEngine;
/// <summary>
/// 골드 키 재화 추가
/// </summary>

public class Pattern_17 : PatternBase
{
    int addValue;
    void Start()
    {

    }

    void AddKey()
    {
        if (enableEvent)
        {
            GlobalData.instance.player.AddDungeonKey(EnumDefinition.GoodsType.gold, addValue);
            StepClear();
        }
    }

    public override void EventStart(TutorialStep stepData)
    {
        Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        SetGoalData(stepData);
        EnableEvent(true);
        AddKey();
    }

    public override void ResetGoalData()
    {
        addValue = 0;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        addValue = stepData.customConditions;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
