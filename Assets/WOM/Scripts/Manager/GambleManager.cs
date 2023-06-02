using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using UnityEngine;
using UnityEngine.Events;

public class GambleManager : MonoBehaviour
{
    public int curSummonGrade = 0; // 소환 등급 데이터 
    public int roundCount = 0;     // 뽑기 시도 카운트


    void Start()
    {
        
    }

    
    void Update()
    {
        
    }


    IEnumerator Gaembeulling(int gambleCount, UnityAction endEvent)
    {

        for (int i = 0; i < gambleCount; i++)
        {



            yield return null;  
        }

        endEvent.Invoke();
    }
}
