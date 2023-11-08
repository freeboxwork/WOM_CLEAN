/// <summary>
/// 금광 몬스터를 optionValue_1(targetCount) 만큼 처치 하고 완료. 완료시 targetCount 리셋
/// </summary>

public class Q_002 : QusetPatternBase
{
    int targetCount = 0;

    void Start()
    {
        AddEvent();
    }

    void AddEvent()
    {
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnQuestPattern_002, CheckEvent);
    }

    void CheckEvent()
    {
        if (enableEvent)
        {
            targetCount--;
            //TODO: UI UPDATE
            if (targetCount <= 0)
            {
                StepClear();
            }
        }
    }

    public override void EventStart(QusetStepData stepData)
    {
        SetGoalData(stepData);
        EnableEvent(true);
    }

    public override void SetGoalData(QusetStepData stepData)
    {
        targetCount = int.Parse(stepData.optionValue_1);
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        qusetManager.CompletQusetStep();
    }

    public override void ResetGoalData()
    {
        targetCount = 0;
    }
}
