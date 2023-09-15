using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonsterChallengeTimer : MonoBehaviour
{
    public AnimData animData;
    bool isStop = false;
    public void SetTimeValue(float value)
    {
        animData.animDuration = value;
    }

    public void StartTimer()
    {
        StartCoroutine(CalcTimer());
    }

    public void StopBossTimer(bool isStop)
    {
        this.isStop = isStop;
    }    

    public IEnumerator CalcTimer()
    {
        animData.ResetAnimData();

        while (animData.animTime < 0.999f)
        {
            if(isStop)
            {
                //isStop가 true가 될때까지 대기하기
                yield return new WaitUntil(() => isStop == false);
            }    
                        
            if(GlobalData.instance.eventController.CheckMonsterDie())
            {
                yield break;
            }

            int timeSecond = (int)animData.animDuration - (int)(Time.time - animData.animStartTime);
            animData.animTime = (Time.time - animData.animStartTime) / animData.animDuration;
            //Debug.Log((int)(Time.time - animData.animStartTime));
            animData.animValue = EaseValues.instance.GetAnimCurve(animData.animCurveType, animData.animTime);
        
            GlobalData.instance.uiController.SetTxtBossChallengeTimer(timeSecond);
            GlobalData.instance.uiController.SetImgTimerFilledRaidal(animData.animValue);
            
            yield return null;
        }

        // 타이머 종료 이벤트
        EventManager.instance.RunEvent(CallBackEventType.TYPES.OnBossMonsterChallengeTimeOut);
    }
}
