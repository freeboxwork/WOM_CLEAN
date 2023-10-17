using UnityEngine;
using TMPro;

public class DungeonSlot : MonoBehaviour
{
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtBestScore;

    //public TextMeshProUGUI txtKeyCount;
    public EnumDefinition.MonsterType monsterType;


    public void UpdateUI()
    {
        UpdateTxtKeyCount();
        SetTextBestScore();
    }

    public void UpdateTxtKeyCount()
    {
        var usingKey = GlobalData.instance.monsterManager.GetMonsterDungeon().monsterToDataMap[monsterType].usingKeyCount;
        var haveKeyCount = GlobalData.instance.player.GetCurrentDungeonKeyCount(monsterType);
        //var txt = $"{haveKeyCount}/{usingKey}";
        //txtKeyCount.text = txt;
    }
    public void SetTextBestScore()
    {
        var score = GlobalData.instance.player.dungeonMonsterClearLevel.GetLeveByDungeonMonType(monsterType);
        txtBestScore.text = $"{score} ´Ü°è";
    }

}
