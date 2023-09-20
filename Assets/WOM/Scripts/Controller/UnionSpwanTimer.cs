using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnionSpwanTimer : MonoBehaviour
{
    public UnionSpwanManager spwanManager;
    float spwanTime;
    float currentTime;
    public bool isTimerReady;
    public int timerIndex;

    UnionSlot unionSlot;

    public void TimerStart(UnionSlot unionSlot, float startTime)
    {
        this.unionSlot = unionSlot;
        isTimerReady = true;
        spwanTime = (float)GlobalData.instance.statManager.GetUnionSpwanSpeed(unionSlot.inGameData.unionIndex);
        currentTime = startTime;
        StartCoroutine(SpwanTimer());
    }

    public void TimerStop()
    {
        isTimerReady = false;
        unionSlot = null;
        StopAllCoroutines();
    }

    public float GetLeftTime()  
    {
        return currentTime;
    }

    IEnumerator SpwanTimer()
    {
        while (isTimerReady)
        {

            currentTime -= Time.deltaTime;

            if(currentTime <= 0)
            {
                spwanTime = (float)GlobalData.instance.statManager.GetUnionSpwanSpeed(unionSlot.inGameData.unionIndex);

                currentTime = spwanTime;

                // set union data
                var union = GlobalData.instance.insectManager.GetDisableUnion();
                union.inGameData = unionSlot.inGameData;

                // set union face
                var sprite = spwanManager.spriteFileData.GetSpriteData(unionSlot.inGameData.unionIndex);
                union.SetInsectFace(sprite);

                // set order layer
                if (IsFlying(unionSlot.inGameData.unionIndex))
                    union.spriteRenderer.sortingOrder = 31;
                else
                    union.spriteRenderer.sortingOrder = 10;

                // set position
                var randomPos = spwanManager.GetRandomPos();
                union.gameObject.transform.position = randomPos;

                // enable insect
                union.gameObject.SetActive(true);
                GlobalData.instance.insectManager.AddEnableInsects(union);

                // skill effect
                yield return new WaitForEndOfFrame();

                if (GlobalData.instance.skillManager.IsUsingSkillByType(EnumDefinition.SkillType.insectDamageUp))
                    union.effectContoller.AuraEffect(true);
                if (GlobalData.instance.skillManager.IsUsingSkillByType(EnumDefinition.SkillType.unionDamageUp))
                    union.effectContoller.FireEffect(true);
                if (GlobalData.instance.skillManager.IsUsingSkillByType(EnumDefinition.SkillType.allUnitSpeedUp))
                    union.effectContoller.TrailEffect(true);
                if (GlobalData.instance.skillManager.IsUsingSkillByType(EnumDefinition.SkillType.glodBonusUp))
                    union.effectContoller.GoldEffect(true);
                if (GlobalData.instance.skillManager.IsUsingSkillByType(EnumDefinition.SkillType.allUnitCriticalChanceUp))
                    union.effectContoller.ThunderEffect(true);
                // Debug.Log($"타이머 인덱스 : {timerIndex} _ 스폰 유니온 : {sprite[0].name} _ 스폰 타임 : {spwanTime}");
            }

            yield return new WaitForEndOfFrame();

        }
    }

    List<int> flyIndexList = new List<int>() { 0, 1, 2, 3, 4, 5, 6, 7,8,9,10,11,12,14,16,17,18,
    19,21,22,24,25,26,27,28,29,30,32,33,34,35,26,27,28,40,41,42,43,44,45,46 };

    bool IsFlying(int index)
    {
        return flyIndexList.Contains(index);
    }

}
