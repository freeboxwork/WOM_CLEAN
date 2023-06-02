using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class UnionSpwanManager : MonoBehaviour
{
    /// <summary> 0: y , 1 : x min , 2 : x max </summary>
    public Transform[] randomPoint;
    public List<UnionSpwanTimer> spwanTimerList = new List<UnionSpwanTimer>();
    public SpriteFileData spriteFileData;


    void Start()
    {

    }

    public IEnumerator Init()
    {
        CreateTimer();
        yield return null;
    }

    // 비활성화 타이머 가지고 오기
    UnionSpwanTimer GetSpwanTimer(int index)
    {
         return spwanTimerList.FirstOrDefault(f=> f.timerIndex == index);
    }

    void CreateTimer()
    {
        var birthCount = GlobalData.instance.evolutionManager.evolutionSlots.Count;
        for (int i = 0; i < birthCount; i++)
        {
            var timer = Instantiate(new GameObject() , transform).AddComponent<UnionSpwanTimer>();
            timer.spwanManager = this;
            timer.timerIndex = i;
            timer.name = $"union spwan timer {i}";
            spwanTimerList.Add(timer);
        }
    }

    public void UnionSpwan(UnionSlot unionSlot, int equipSlotIndex)
    {
        StartCoroutine(StartUnionSpwan(unionSlot, equipSlotIndex));
    }

    public IEnumerator StartUnionSpwan(UnionSlot unionSlot, int equipSlotIndex)
    {
        GetSpwanTimer(equipSlotIndex).TimerStop();

        yield return new WaitForEndOfFrame();
        
        // start spawn
        GetSpwanTimer(equipSlotIndex).TimerStart( unionSlot);
    }

    


    #region UTILITY METHOD
    public Vector3 GetRandomPos()
    {
        return new Vector3(GetPosX(), GetPosY(), 0);
    }
    float GetPosY()
    {
        return randomPoint[0].position.y;
    }

    float GetPosX()
    {
        var xMinx = randomPoint[1].position.x;
        var xMax = randomPoint[2].position.x;
        return Random.Range(xMinx, xMax);   
    }
    #endregion

}
