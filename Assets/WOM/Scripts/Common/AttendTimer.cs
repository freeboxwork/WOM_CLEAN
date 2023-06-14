using UnityEngine;
using System;
using System.Linq;

public class AttendTimer : MonoBehaviour
{

    public string attendTestCurDateValue = "2023-06-12";
    public string attendTestPrevDateValue = "2023-06-12";
    const string LAST_ATTEND_DATE_KEY = "last_attend_date";
    const string UNLOCKED_ATTEND_COUNT_KEY = "unlocked_attend_count";

    public int unLockCount;


    void Start()
    {

    }



    public void CalcAttendTimer()
    {

        if (HasLastAttendanceCount() == false)
        {
            PlayerPrefs.SetInt(UNLOCKED_ATTEND_COUNT_KEY, 0);
            PlayerPrefs.SetString(LAST_ATTEND_DATE_KEY, DateTime.Now.ToString("yyyy-MM-dd"));
        }
        else
        {
            CalcAttendCount();
        }
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         CalcAttendTimer();
    //     }
    // }

    void CalcAttendCount()
    {
        var now = DateTime.Now.ToString("yyyy-MM-dd");

        // test code
        var nowDate = DateTime.Parse(attendTestCurDateValue);
        var lastDate = DateTime.Parse(attendTestPrevDateValue);

        //var nowDate = DateTime.Parse(now);
        //var lastDate = DateTime.Parse(PlayerPrefs.GetString(LAST_ATTEND_DATE_KEY));

        TimeSpan timeSpan = nowDate.Subtract(lastDate);
        var totalDays = timeSpan.TotalDays;

        if (totalDays > 0)
        {
            var count = PlayerPrefs.GetInt(UNLOCKED_ATTEND_COUNT_KEY);
            var countValue = count + 1;
            PlayerPrefs.SetInt(UNLOCKED_ATTEND_COUNT_KEY, countValue);

            // set attend date
            PlayerPrefs.SetString(LAST_ATTEND_DATE_KEY, now);

            var maxCount = GlobalData.instance.dataManager.attendDatas.data.Max(x => x.day);

            if (countValue >= maxCount)
            {
                PlayerPrefs.SetInt(UNLOCKED_ATTEND_COUNT_KEY, 0);


                // 리워드 지급 여부 리셋
                var resetData = GlobalData.instance.dataManager.attendDatas.data;
                for (int i = 0; i < resetData.Count; i++)
                {
                    var loadKey = $"{GlobalData.instance.questManager.keyAttendUsedReawrd}_{resetData[i].day}";
                    var hasKey = PlayerPrefs.HasKey(loadKey);
                    if (hasKey)
                    {
                        PlayerPrefs.SetInt(loadKey, 0);
                    }
                }
            }
        }
        else if (totalDays == 0)
        {
            Debug.Log("동일한 날");
        }

        unLockCount = PlayerPrefs.GetInt(UNLOCKED_ATTEND_COUNT_KEY);
        Debug.Log("count : " + unLockCount);
    }

    bool HasLastAttendanceDate()
    {
        return PlayerPrefs.HasKey(LAST_ATTEND_DATE_KEY);
    }

    bool HasLastAttendanceCount()
    {
        return PlayerPrefs.HasKey(UNLOCKED_ATTEND_COUNT_KEY);
    }

}
