using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsectSpwanTimer : MonoBehaviour
{
    public EnumDefinition.InsectType insectType;
    public bool isTimerReady;
    public InsectManager insectManager;
    public InsectSpwanManager insectSpwanManager;

    void Start()
    {
        insectManager = GlobalData.instance.insectManager;
    }

    public void TimerStart()
    {
        isTimerReady = true;
        StartCoroutine(SpwanTimer());
    }

    public void TimerStop()
    {
        isTimerReady = false;
        StopAllCoroutines();
    }

    IEnumerator SpwanTimer()
    {
        while (isTimerReady)
        {
            var insect = insectManager.GetDisableST_InsectByType(insectType);
            var waitTime = GlobalData.instance.statManager.GetInsectSpwanTime(insectType);

            //Debug.Log(" T Y P E " + insectType + " -- " +waitTime);

            if (insect != null)
            {
                // set position
                var randomPos = insectSpwanManager.GetRandomPos();
                insect.gameObject.transform.position = randomPos;
                
                yield return new WaitForSeconds(waitTime);
                insect.gameObject.SetActive(true);
            }
            else
                yield return new WaitForSeconds(waitTime);

        }
    }
}
