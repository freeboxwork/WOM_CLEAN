/// <summary>
/// 현재 skillType을 획득 한 상태 혹은 스킬을 획득하면 완료
/// </summary>

public class Q_006 : QusetPatternBase
{
    EnumDefinition.SkillType targetType;

    void Start()
    {
        AddEvent();
    }

    void AddEvent()
    {
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnQuestPattern_006, CheckEvent);
    }

    void CheckEvent()
    {
        var skillData = GlobalData.instance.skillManager.GetSkill_InGameDataByType(targetType);
        if (skillData.level > 0)
        {
            StepClear();
        }
    }

    public override void EventStart(QusetStepData stepData)
    {
        SetGoalData(stepData);
        EnableEvent(true);
        CheckEvent();
    }

    public override void SetGoalData(QusetStepData stepData)
    {
        targetType = UtilityMethod.GetEnumType<EnumDefinition.SkillType>(stepData.optionValue_1);
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        qusetManager.CompletQusetStep();
    }

    public override void ResetGoalData()
    {
        // foreach 사용하는 곳이 있어서 none 추가 못함.
        targetType = EnumDefinition.SkillType.insectDamageUp;
    }
}