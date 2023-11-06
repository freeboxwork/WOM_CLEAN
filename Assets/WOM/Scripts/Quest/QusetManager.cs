using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System.Linq;

public class QusetManager : MonoBehaviour
{
    public QuestDataManager questDatas;
    public List<QusetPatternBase> qusetPatterns = new List<QusetPatternBase>();
    public QusetStepDatas qusetStepDatas;
    public TextAsset questJsonData;

    int currentQuestStepId = 0;

    void Start()
    {

    }

    public IEnumerator Init()
    {
        yield return null;
        // set data
        SetQuestData();

        //TODO: load current quest step id ( save data )
        currentQuestStepId = 0;

    }

    public void SetQuestData()
    {
        qusetStepDatas = JsonUtility.FromJson<QusetStepDatas>(questJsonData.text);
    }




    QusetPatternBase GetPatternByType(string type)
    {
        var patternType = (EnumDefinition.QusetPatternType)System.Enum.Parse(typeof(EnumDefinition.QusetPatternType), type);
        var pattern = qusetPatterns.FirstOrDefault(f => f.patternType == patternType);

        // 예외처리
        if (pattern == null)
        {
            Debug.LogError("QusetPatternBase is null");
            return null;
        }
        return pattern;
    }





}

[System.Serializable]
public class QusetStepDatas
{
    public List<QusetStepData> data = new List<QusetStepData>();
}

[System.Serializable]
public class QusetStepData
{
    public int id;
    public string guideName;
    public string questType;
    public int targetCount;
    public string questParternType;
    public int optionValue_1;
    public string optionValue_2;
    public string targetPanelId;
    public string targetGuideBtnId;
    public string targetGuideButton;
    public string targetPanel;
    public string resetCounting;
    public string rewardType;
    public int rewardAmount;
    public string unLockContents;
}