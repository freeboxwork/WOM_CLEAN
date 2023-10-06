using UnityEngine;
using System;
using BackEnd;

public class QuestResetTimer : MonoBehaviour
{
    // Start is called before the first frame update
    void Awake()
    {
        Backend.Initialize(true);
    }

    void Start()
    {
        // PlayerPrefs 에 CURRENT_TIME_KEY 가 저장되어 있지 않다면 SaveCurrentTime() 함수를 실행한다.
        if (!HasMidnightTime())
        {
            SaveMidnightTime();
        }
    }


    private const string CURRENT_TIME_KEY = "current_time";
    private const string MIDNIGHT_TIME_KEY = "midnight_time";

    public bool HasMidnightTime()
    {
        return PlayerPrefs.HasKey(MIDNIGHT_TIME_KEY);
    }


    void SaveMidnightTime()
    {
        // 오늘 자정 시간을 계산한다.
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        var timeData = DateTime.Parse(time);
        var midnight = timeData.ToString("yyyy-MM-dd");

        // PlayerPrefs에 오늘 자정 시간을 문자열로 저장한다.
        PlayerPrefs.SetString(MIDNIGHT_TIME_KEY, midnight);

        //Debug.Log(midnight.ToString());
    }

    public bool HasCrossedMidnight()
    {

        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        var timeData = DateTime.Parse(time);
        var now = timeData.ToString("yyyy-MM-dd");

        Debug.Log("backend : " + now);

        // test code
        // DateTime currentTime = DateTime.Parse(testCurDateValue);
        // DateTime midnight = DateTime.Parse(testPrevDateValue);

        // 현재 시간과 저장된 오늘 자정 시간을 비교한다.
        DateTime currentTime = DateTime.Parse(now);
        DateTime midnight = DateTime.Parse(PlayerPrefs.GetString(MIDNIGHT_TIME_KEY));

        var timeSpan = currentTime.Subtract(midnight);
        //Debug.Log("지나간 날 : " + timeSpan.TotalDays.ToString());
        var totalDays = timeSpan.TotalDays;

        if (totalDays > 0)
        {
            // 저장된 오늘 자정 시간 이후라면 true를 반환한다.
            return true;
        }
        else
        {
            // 저장된 오늘 자정 시간 이전이라면 false를 반환한다.
            return false;
        }
    }

    public void ResetTimer()
    {
        SaveMidnightTime();
    }


}
