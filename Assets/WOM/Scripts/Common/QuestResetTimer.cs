using UnityEngine;
using System;
using BackEnd;

public class QuestResetTimer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        // PlayerPrefs �� CURRENT_TIME_KEY �� ����Ǿ� ���� �ʴٸ� SaveCurrentTime() �Լ��� �����Ѵ�.
        if (!HasMidnightTime())
        {
            //SaveCurrentTime();
            SaveMidnightTime();
        }
        // else
        // {
        //     if (HasCrossedMidnight())
        //     {
        //         Debug.Log("������ �������ϴ�.");

        //         // reset timer
        //         // SaveCurrentTime();
        //         // SaveMidnightTime();

        //         // reset one day quest
        //     }
        // }
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.A))
    //     {
    //         // SaveCurrentTime();
    //         SaveMidnightTime();
    //     }
    // }

    private const string CURRENT_TIME_KEY = "current_time";
    private const string MIDNIGHT_TIME_KEY = "midnight_time";

    public bool HasMidnightTime()
    {
        return PlayerPrefs.HasKey(MIDNIGHT_TIME_KEY);
    }

    void SaveCurrentTime()
    {
        // ���� �ð��� �����´�.
        var now = DateTime.Now.ToString("yyyy-MM-dd");

        // PlayerPrefs�� ���� �ð��� ���ڿ��� �����Ѵ�.
        PlayerPrefs.SetString(CURRENT_TIME_KEY, now);

        Debug.Log(now.ToString());

        //PlayerPrefs.Save();.
    }

    void SaveMidnightTime()
    {
        // ���� ���� �ð��� ����Ѵ�.
        // DateTime midnight = DateTime.Today.AddDays(1);
        var midnight = DateTime.Now.ToString("yyyy-MM-dd");

        // PlayerPrefs�� ���� ���� �ð��� ���ڿ��� �����Ѵ�.
        PlayerPrefs.SetString(MIDNIGHT_TIME_KEY, midnight);

        //Debug.Log(midnight.ToString());

        //PlayerPrefs.Save();
    }

    DateTime LoadCurrentTime()
    {
        // PlayerPrefs���� ����� �ð� ������ �ҷ��´�.
        string currentTimeStr = PlayerPrefs.GetString(CURRENT_TIME_KEY);
        //string currentTimeStr = "2023-06-19";

        // �ҷ��� ���ڿ� �ð� ������ DateTime �������� ��ȯ�Ѵ�.
        DateTime currentTime = DateTime.Parse(currentTimeStr);

        return currentTime;
    }

    public string testCurDateValue;
    public string testPrevDateValue;
    DateTime midnight;
    public bool HasCrossedMidnight()
    {

        var now = DateTime.Now.ToString("yyyy-MM-dd");

        // test code
        // DateTime currentTime = DateTime.Parse(testCurDateValue);
        // DateTime midnight = DateTime.Parse(testPrevDateValue);

        // ���� �ð��� ����� ���� ���� �ð��� ���Ѵ�.
        DateTime currentTime = DateTime.Parse(now);

#if !UNITY_EDITOR && UNITY_ANDROID
        BackendReturnObject servertime = Backend.Utils.GetServerTime();

        string time = servertime.GetReturnValuetoJSON()["utcTime"].ToString();
        midnight = DateTime.Parse(time);
        Debug.Log(midnight.ToString());
#endif
#if UNITY_EDITOR
        DateTime midnight = DateTime.Parse(PlayerPrefs.GetString(MIDNIGHT_TIME_KEY));
#endif
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
        //SaveCurrentTime();
        SaveMidnightTime();
    }














}
