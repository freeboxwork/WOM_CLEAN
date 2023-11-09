/// <summary>
/// 몬스터를 targetCount 만큼 처치 하면 완료. 완료시 targetCount 리셋
/// </summary>

public class Q_005 : QusetPatternBase
{
    int targetCount = 0;

    void Start()
    {
        AddEvent();
    }

    void AddEvent()
    {
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnQuestPattern_005, CheckEvent);
    }

    void CheckEvent()
    {

        if (enableEvent)
        {
            targetCount--;
            if (0 >= targetCount)
            {
                StepClear();
            }
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