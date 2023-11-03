using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class QusetManager : MonoBehaviour
{
    public QuestDataManager questDatas;
    public List<QusetPatternBase> qusetPatterns = new List<QusetPatternBase>();

    void Start()
    {

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
