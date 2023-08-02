using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StageNameSetManager : MonoBehaviour
{
    public TextMeshProUGUI txtStageNameNormal;
    public TextMeshProUGUI txtStageNameEvolution;
    public TextMeshProUGUI txtStageNameDungeon;

    Dictionary<EnumDefinition.StageNameType, TextMeshProUGUI> dicStageName = new Dictionary<EnumDefinition.StageNameType, TextMeshProUGUI>();

    void Start()
    {
        SetDicStageName();
    }

    void SetDicStageName()
    {
        dicStageName.Add(EnumDefinition.StageNameType.normal, txtStageNameNormal);
        dicStageName.Add(EnumDefinition.StageNameType.evolution, txtStageNameEvolution);
        dicStageName.Add(EnumDefinition.StageNameType.dungeon, txtStageNameDungeon);
    }
    public void SetTxtStageName(EnumDefinition.StageNameType stageNameType, string stageName)
    {
        dicStageName[stageNameType].text = stageName;
    }

    public void EnableStageName(EnumDefinition.StageNameType stageNameType)
    {
        foreach (var item in dicStageName)
            item.Value.gameObject.SetActive(item.Key == stageNameType);
    }




}
