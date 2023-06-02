using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonManager : MonoBehaviour
{
    public List<DungeonSlot> dungeonSlots;

    void Start()
    {
         
    }

    public IEnumerator Init()
    {
        UpdateDunslotKeyUI(EnumDefinition.MonsterType.dungeonGold);
        UpdateDunslotKeyUI(EnumDefinition.MonsterType.dungeonBone);
        UpdateDunslotKeyUI(EnumDefinition.MonsterType.dungeonDice);
        UpdateDunslotKeyUI(EnumDefinition.MonsterType.dungeonCoal);

        yield return null;
    }

    public void UpdateDunslotKeyUI(EnumDefinition.MonsterType monsterType)
    {
        GetDungeonSlotByMonsterType(monsterType).UpdateTxtKeyCount();
    }

    DungeonSlot GetDungeonSlotByMonsterType(EnumDefinition.MonsterType monsterType)
    {
        return dungeonSlots.FirstOrDefault(f=> f.monsterType== monsterType);    
    }



}
