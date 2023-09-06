public class Pattern_18 : PatternBase
{

    int skipSetID = 0;

    public override void EventStart(TutorialStep stepData)
    {
        SetGoalData(stepData);
        EnableEvent(true);
        SkipSet();
    }

    public override void ResetGoalData()
    {
        skipSetID = 0;
    }

    public override void SetGoalData(TutorialStep stepData)
    {
        skipSetID = stepData.customConditions;
    }

    public override void StepClear()
    {
        ResetGoalData();
        EnableEvent(false);
        tutorialManager.CompleteStep();
    }

    void SkipSet()
    {
        if (enableEvent)
        {
            // 버프 구매 이력 확인
            if (GlobalData.instance.inAppPurchaseManager.IsPayBuffInApp())
            {
                tutorialManager.skipSetID = skipSetID;
                tutorialManager.skipSet = true;
            }
            StepClear();
        }
    }
}
