/// <summary>
/// 현재 optionValue_1(훈련 타입- > SaleStatType )의 공격력 레벨이 optionValue_2(targetLevel) 보다 크면 완료
/// </summary>

public class Q_004 : QusetPatternBase
{

    EnumDefinition.SaleStatType targetType;
    int targetLevel = 0;

    void Start()
    {
        AddEvent();
    }

    void AddEvent()
    {
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnQuestPattern_004, CheckEvent);
    }

    void CheckEvent()
    {
        if (enableEvent)
        {
            if (IsTargetLevel())
            {
                StepClear();
            }
        }
    }

    bool IsTargetLevel()
    {
        return GetTraningLevelByType(targetType) > targetLevel;
    }

    int GetTraningLevelByType(EnumDefinition.SaleStatType statType)
    {
        return GlobalData.instance.traningManager.GetInGameStatLevel(statType);
    }

    public override void EventStart(QusetStepData stepData)
    {
        SetGoalData(stepData);
        EnableEvent(true);
        CheckEvent();
    }

    public override void SetGoalData(QusetStepData stepData)
    {
        targetType = UtilityMethod.GetEnumType<EnumDefinition.SaleStatType>(stepData.optionValue_1);
        targetLevel = int.Parse(stepData.optionValue_2);
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        qusetManager.CompletQusetStep();
    }

    public override void ResetGoalData()
    {
        targetLevel = 0;
    }
}