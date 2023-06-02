using UnityEngine;
using TMPro;

public class DungeonSlot : MonoBehaviour
{
    public TextMeshProUGUI txtTitle;
    public TextMeshProUGUI txtKeyCount;
    public EnumDefinition.MonsterType monsterType;

    void Start()
    {

    }

    public void UpdateTxtKeyCount()
    {
        var usingKey = GlobalData.instance.monsterManager.GetMonsterDungeon().monsterToDataMap[monsterType].usingKeyCount;
        var haveKeyCount = GlobalData.instance.player.GetCurrentDungeonKeyCount(monsterType);
        var txt = $"{haveKeyCount}/{usingKey}";
        txtKeyCount.text = txt;
    }


}
