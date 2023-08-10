using System.Collections;
using UnityEngine;

public class UnionSpwanTimer : MonoBehaviour
{
    public UnionSpwanManager spwanManager;
    public float spwanTime;
    public bool isTimerReady;
    public int timerIndex;

    UnionSlot unionSlot;


    void Start()
    {

    }

    public void SetSpwanTime(float time)
    {
        spwanTime = time;
    }

    public void TimerStart(UnionSlot unionSlot)
    {
        this.unionSlot = unionSlot;
        isTimerReady = true;
        var _spwanTime = GlobalData.instance.statManager.GetUnionSpwanSpeed(unionSlot.inGameData.unionIndex);
        //SetSpwanTime(unionSlot.inGameData.spawnTime);
        SetSpwanTime((float)_spwanTime);
        StartCoroutine(SpwanTimer());
    }

    public void TimerStop()
    {
        isTimerReady = false;
        unionSlot = null;
        StopAllCoroutines();
    }

    IEnumerator SpwanTimer()
    {
        while (isTimerReady)
        {

            // yield return new WaitUntil(() => GlobalData.instance.attackController.GetAttackableState() == true);

            // set union data
            var union = GlobalData.instance.insectManager.GetDisableUnion();
            union.inGameData = unionSlot.inGameData;

            // set union face
            var sprite = spwanManager.spriteFileData.GetSpriteData(unionSlot.inGameData.unionIndex);
            union.SetInsectFace(sprite);

            // set position
            var randomPos = spwanManager.GetRandomPos();
            union.gameObject.transform.position = randomPos;

            // spwan time 대기
            yield return new WaitForSeconds(spwanTime);

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
    }



}
