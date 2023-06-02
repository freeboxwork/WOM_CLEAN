using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static EnumDefinition;

public class MonsterManager : MonoBehaviour
{
    public MonsterNormal monsterNormal;
    public MonsterGold monsterGold;
    public MonsterBoss monsterBoss;
    public MonsterEvolution monsterEvolution;
    public DungeonMonster monsterDungeon;

    public int monsterId_normal;
    public int monsterId_gold;
    public int monsterId_boss;
         

    // normal gold boss evolution
    public List<MonsterBase> monsters = new List<MonsterBase>();

    void Start()
    {
        AddMonsterList();
    }

    void AddMonsterList()
    {
        // add monsters list 
        // 순서 : normal ,  gold,  boss , evolution
        monsters.Add(monsterNormal);
        monsters.Add(monsterGold);
        monsters.Add(monsterBoss);
        monsters.Add(monsterEvolution);
    }

    public IEnumerator Init(int stageId)
    {
        StageData stageData = GlobalData.instance.dataManager.GetStageDataById(stageId);

        yield return null;

        var monNormalData = GetMonsterData(MonsterType.normal, stageData.monsterNormalId);
        var monGoldData =   GetMonsterData(MonsterType.gold,   stageData.monsterGoldId);
        var monBossData =   GetMonsterData(MonsterType.boss,   stageData.monsterBossId);

        SetMonsterData(MonsterType.normal, monNormalData);
        SetMonsterData(MonsterType.gold, monGoldData);
        SetMonsterData(MonsterType.boss, monBossData);

        // set sprite image
        SetMonsterBodyImage(monNormalData, monsterNormal);
        SetMonsterBodyImage(monGoldData, monsterGold);
        SetMonsterBodyImage(monBossData, monsterBoss);
    }

    public void SetMonsterData(MonsterType monsterType, int stageId)
    {
        StageData stageData = GlobalData.instance.dataManager.GetStageDataById(stageId);
        switch (monsterType)
        {
            case MonsterType.normal:
                var monNormalData = GetMonsterData(monsterType, stageData.monsterNormalId);
                SetMonsterData(monsterType, monNormalData);
                break;
            
            case MonsterType.gold:
                var monGoldData = GetMonsterData(monsterType, stageData.monsterGoldId);
                SetMonsterData(monsterType, monGoldData);
                break;
            
            case MonsterType.boss:
                var monBossData = GetMonsterData(monsterType, stageData.monsterBossId); 
                SetMonsterData(monsterType , monBossData);  
                break;
        }
    }

    /// <summary> 기본 몬스터 ( 노멀,골드,보스 제외 ) 이외의 몬스터 데이터 세팅시  </summary>
    public void SetMonsterDataOther(MonsterType monsterType, int dataId)
    {

        switch (monsterType)
        {
            case MonsterType.evolution:
                var monsterData = GetMonsterData(monsterType, dataId);
                SetMonsterData(monsterType, monsterData);
                break;
        }
    }

    //int GetMonsterID(StageData stageData, MonsterType monsterType)
    //{
    //    switch (monsterType)
    //    {
    //        case MonsterType.normal: return stageData.monsterNormalId);
    //    }
    //}

    void SetMonsterBodyImage(MonsterData monData, MonsterBase monster)
    {
        var spriteData = GlobalData.instance.dataManager.GetMonsterSpriteDataById(monData.imageId);

        // set tail
        monster.spriteLibraryChanged.ChangedSpritePartImage("tail", spriteData.tail);
        // set hand
        monster.spriteLibraryChanged.ChangedSpritePartImage("hand", spriteData.hand);
        // set finger
        monster.spriteLibraryChanged.ChangedSpritePartImage("finger", spriteData.finger);
        // set foreArm
        monster.spriteLibraryChanged.ChangedSpritePartImage("foreArm", spriteData.foreArm);
        // set upperArm
        monster.spriteLibraryChanged.ChangedSpritePartImage("upperArm", spriteData.upperArm);
        // set head
        monster.spriteLibraryChanged.ChangedSpritePartImage("head", spriteData.head);
        // set body
        monster.spriteLibraryChanged.ChangedSpritePartImage("body", spriteData.body);
        // set leg_0
        monster.spriteLibraryChanged.ChangedSpritePartImage("leg_0", spriteData.leg_0);
        // set leg_1
        monster.spriteLibraryChanged.ChangedSpritePartImage("leg_1", spriteData.leg_1);
        // set leg_2
        monster.spriteLibraryChanged.ChangedSpritePartImage("leg_2", spriteData.leg_2);
    }


    public void EnableMonster(MonsterType monsterType)
    {
        for (int i = 0; i < monsters.Count; i++)
        {
            if ((int)monsterType == i)
            {
                //TODO : 꺼야 하는지 확인 필요
                monsters[i].gameObject.SetActive(true);
            }
            else
            {
                // TODO: 
                //monsters[i].gameObject.SetActive(false);
            }
        }
    }

    MonsterData GetMonsterData(MonsterType monsterType, int monsterId)
    {
        var data = GlobalData.instance.dataManager.GetMonsterDataById(monsterType,monsterId); 
        if(data == null)
        {
            //TODO : ERROR PRINT 관리자 만들기
            Debug.LogError("");
            return null;
        }
        else
        {
            return data;    
        }
    }

    void SetMonsterData(MonsterType monsterType , MonsterData monsterData)
    {
        var monster = GetMonsterData(monsterType);
        
        monster.id = monsterData.id;
        monster.hp = monsterData.hp;
        monster.exp = monsterData.exp;
        monster.bone = monsterData.bone;
        monster.gold = monsterData.gold;
        monster.boneCount = monsterData.boneCount;
        monster.goldCount = monsterData.goldCount;
        monster.imageId = monsterData.imageId;
        monster.bgId = monsterData.bgId;
        monster.monsterType = monsterData.monsterType;
        monster.attackType = monsterData.attackType;

    }



    public MonsterBase GetMonsterData(MonsterType monsterType)
    {
        switch (monsterType)
        {
            case MonsterType.normal: return monsterNormal;
            case MonsterType.gold: return monsterGold;
            case MonsterType.boss: return monsterBoss;
            case MonsterType.evolution: return monsterEvolution; 
        }
        return null;
    }

    public DungeonMonster GetMonsterDungeon()
    {
        return monsterDungeon;
    }

}
