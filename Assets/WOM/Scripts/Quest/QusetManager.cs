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

        StartQuestStep();
    }

    void StartQuestStep()
    {
        var data = GetCurrentQuestStepData();

        // UI SETTING

        // Pattern Start
        var pattern = GetQusetPatternBase(data);
        pattern.EventStart(data);
    }

    public void SetQuestData()
    {
        qusetStepDatas = JsonUtility.FromJson<QusetStepDatas>(questJsonData.text);
    }

    public QusetStepData GetCurrentQuestStepData()
    {
        return qusetStepDatas.data.Where(a => a.id == currentQuestStepId).FirstOrDefault();
    }

    QusetPatternBase GetQusetPatternBase(QusetStepData data)
    {
        return GetPatternByType(data.questParternType);
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

    public void CompletQusetStep()
    {
        // 보상 지급 이후 다음 스텝으로
        // TODO: UI SETTING
    }

    void Reward()
    {

        QuestNextStep();
    }

    void QuestNextStep()
    {
        currentQuestStepId++;
        // 다음 스텝이 있는지 확인
        if (qusetStepDatas.data.Any(a => a.id == currentQuestStepId))
        {
            // 다음 스텝 실행
            StartQuestStep();
        }
        else
        {
            // 퀘스트 완료
            Debug.Log("퀘스트 완료");
        }
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
    public string optionValue_1;
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