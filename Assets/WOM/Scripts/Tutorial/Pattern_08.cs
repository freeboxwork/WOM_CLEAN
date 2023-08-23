/// <summary>
/// 유니온 뽑기 완료 대기
/// </summary>
public class Pattern_08 : PatternBase
{

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
        EventManager.instance.AddCallBackEvent(CallBackEventType.TYPES.OnTutorialUnionGamblingEnd, OnUnionGamblingEnd);
    }
    void RemoveEvent()
    {
        EventManager.instance.RemoveCallBackEvent(CallBackEventType.TYPES.OnTutorialUnionGamblingEnd, OnUnionGamblingEnd);
    }

    void OnUnionGamblingEnd()
    {
        if (enableEvent)
        {
            StepClear();
        }
    }


    public override void EventStart(TutorialStep stepData)
    {
        //Debug.Log("투토리얼 패턴 이벤트 시작 " + stepData.patternType);
        //EnableEvent(true);
        StepClear();
    }

    public override void ResetGoalData()
    {

    }

    public override void SetGoalData(TutorialStep stepData)
    {

    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }
}
