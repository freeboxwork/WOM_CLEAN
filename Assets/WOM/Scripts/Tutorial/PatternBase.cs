using UnityEngine;

public abstract class PatternBase : MonoBehaviour
{

    public EnumDefinition.PatternType patternType;
    public bool enableEvent = false;
    public abstract void EventStart(TutorialStep stepData);
    public abstract void SetGoalData(TutorialStep stepData);
    public abstract void ResetGoalData();
    public abstract void StepClear();
    public TutorialManager tutorialManager;

    public void EnableEvent(bool value)
    {
        enableEvent = value;
    }
    public bool IsTypeTextAnimEnd()
    {
        return !tutorialManager.tutorialUiCont.isTypeAnim;
    }
}
