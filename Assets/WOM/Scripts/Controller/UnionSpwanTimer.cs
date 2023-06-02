using System.Collections;
using System.Collections.Generic;
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
    
    public void TimerStart( UnionSlot unionSlot )
    {
        this.unionSlot = unionSlot;
        isTimerReady = true;
        SetSpwanTime(unionSlot.inGameData.spawnTime);
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

            Debug.Log($"타이머 인덱스 : {timerIndex} _ 스폰 유니온 : {sprite[0].name} _ 스폰 타임 : {spwanTime}");

        
        }
    }



}
