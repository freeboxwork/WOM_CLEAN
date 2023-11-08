using UnityEngine;

public abstract class QusetPatternBase : MonoBehaviour
{
    public EnumDefinition.QusetPatternType patternType;
    public bool enableEvent = false;
    public abstract void EventStart(QusetStepData stepData);
    public abstract void SetGoalData(QusetStepData stepData);
    public abstract void ResetGoalData();
    public abstract void StepClear();
    public QusetManager qusetManager;

    public void EnableEvent(bool value)
    {
        enableEvent = value;
    }

}
