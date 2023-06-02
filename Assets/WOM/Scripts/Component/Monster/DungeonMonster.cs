using ProjectGraphics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumDefinition;

public class DungeonMonster : DungeonMonsterBase
{
    [HideInInspector]
    public DungenMonsterFileData curMonsterData;

    public int curLevel = 1;
    public float curMonsterHP;
    public DungeonMonsterData curData;
    public SpriteLibraryChanged spriteLibraryChanged;
    public MonsterInOutAnimator inOutAnimator;


    public DungenMonsterFileData[] dungenMonsterFileDatas;

    public Dictionary<MonsterType, DungenMonsterFileData> monsterToDataMap;

    void Start()
    {
        SetDataMap();
    }

    void SetDataMap()
    {
        monsterToDataMap = new Dictionary<MonsterType, DungenMonsterFileData>()
        {
            { MonsterType.dungeonGold, dungenMonsterFileDatas[0] },
            { MonsterType.dungeonDice, dungenMonsterFileDatas[1] },
            { MonsterType.dungeonBone, dungenMonsterFileDatas[2] },
            { MonsterType.dungeonCoal, dungenMonsterFileDatas[3] }
        };
    }

    public IEnumerator Init(MonsterType monsterType)
    {
        // SET TYPE
        SetMonsterType(monsterType);

        // SET DATA
        curData = GlobalData.instance.dataManager.GetDungeonMonsterDataByTypeLevel(monsterType, curLevel).CloneInstance();

        curMonsterHP = curData.monsterHP;

        yield return new WaitForEndOfFrame();

        // SET FACE
        SetMonsterFace(monsterToDataMap[monsterType].monsterFaceId);

        // 몬스터 hp text
        GlobalData.instance.uiController.SetTxtMonsterHp(curMonsterHP);

        // 몬스터 hp slider
        GlobalData.instance.uiController.SetSliderDungeonMonsterHP(curMonsterHP);

        yield return new WaitForEndOfFrame();
    }

    // 현재 던전 몬스터의 타입과 재화 타입 설정
    public void SetMonsterType(MonsterType monsterType)
    {
        curMonsterData = monsterToDataMap[monsterType];
        //this.monsterType = monsterType;
        //goodsType = monsterToDataMap[monsterType].goodsType;
    }

    void SetMonsterFace(int faceId)
    {
        spriteLibraryChanged.ChangedSpriteAllImage(faceId);
    }

    public void SetNextLevelData()
    {
        curLevel++;
        curData = GlobalData.instance.dataManager.GetDungeonMonsterDataByTypeLevel(curMonsterData.monsterType, curLevel).CloneInstance();
        curMonsterHP = curData.monsterHP;

        // 레벨업 데이터 저장
        GlobalData.instance.player.dungeonMonsterClearLevel.SetLevelFromMonsterType(curMonsterData.monsterType, curLevel);
    }

    public void DungeonMonsterOut()
    {
        curLevel = 0;
    }


}
