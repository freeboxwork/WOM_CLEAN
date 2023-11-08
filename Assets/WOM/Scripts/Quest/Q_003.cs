/// <summary>
/// 현재 스테이지가 tagetStage 보다 크면 완료
/// </summary>

public class Q_003 : QusetPatternBase
{
    int targetStage = 0;

    void Start()
    {
        AddEvent();
    }

    void AddEvent()
    {
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnQuestPattern_003, CheckEvent);
    }

    void CheckEvent()
    {

        if (enableEvent)
        {
            if (GlobalData.instance.player.stageIdx >= targetStage)
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
        targetStage = int.Parse(stepData.optionValue_1);
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        qusetManager.CompletQusetStep();
    }

    public override void ResetGoalData()
    {
        targetStage = 0;
    }
}