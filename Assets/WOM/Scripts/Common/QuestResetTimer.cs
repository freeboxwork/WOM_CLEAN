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
        // PlayerPrefs �� CURRENT_TIME_KEY �� ����Ǿ� ���� �ʴٸ� SaveCurrentTime() �Լ��� �����Ѵ�.
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
        // ���� ���� �ð��� ����Ѵ�.
        BackendReturnObject servertime = Backend.Utils.GetServerTime();
        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        var timeData = DateTime.Parse(time);
        var midnight = timeData.ToString("yyyy-MM-dd");

        // PlayerPrefs�� ���� ���� �ð��� ���ڿ��� �����Ѵ�.
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

        // ���� �ð��� ����� ���� ���� �ð��� ���Ѵ�.
        DateTime currentTime = DateTime.Parse(now);
        DateTime midnight = DateTime.Parse(PlayerPrefs.GetString(MIDNIGHT_TIME_KEY));

        var timeSpan = currentTime.Subtract(midnight);
        //Debug.Log("������ �� : " + timeSpan.TotalDays.ToString());
        var totalDays = timeSpan.TotalDays;

        if (totalDays > 0)
        {
            // ����� ���� ���� �ð� ���Ķ�� true�� ��ȯ�Ѵ�.
            return true;
        }
        else
        {
            // ����� ���� ���� �ð� �����̶�� false�� ��ȯ�Ѵ�.
            return false;
        }
    }

    public void ResetTimer()
    {
        SaveMidnightTime();
    }


}
