using UnityEngine;
using System;
using System.Linq;

public class NewUserEventTimer : MonoBehaviour
{
    public string testCurDateValue = "2023-06-12";
    public string testPrevDateValue = "2023-06-12";
    const string LAST_DATE_KEY = "last_newUserEvent_date";
    const string UNLOCKED_COUNT_KEY = "unlocked_newUserEvent_count";

    // 지급 완료 여부
    const string REWARD_COMPLETE_KEY = "newUserEvent_reward_complete";

    public int unLockCount;


    void Start()
    {

    }



    public void CalcTimer()
    {

        if (HasLastCount() == false)
        {
            PlayerPrefs.SetInt(UNLOCKED_COUNT_KEY, 0);
            PlayerPrefs.SetString(LAST_DATE_KEY, DateTime.Now.ToString("yyyy-MM-dd"));
        }
        else
        {
            CalcCount();
        }
    }

    // void Update()
    // {
    //     if (Input.GetKeyDown(KeyCode.Z))
    //     {
    //         CalcAttendTimer();
    //     }
    // }

    void CalcCount()
    {
        var now = DateTime.Now.ToString("yyyy-MM-dd");

        // test code
        var nowDate = DateTime.Parse(testCurDateValue);
        var lastDate = DateTime.Parse(testPrevDateValue);

        //var nowDate = DateTime.Parse(now);
        //var lastDate = DateTime.Parse(PlayerPrefs.GetString(LAST_DATE_KEY));

        TimeSpan timeSpan = nowDate.Subtract(lastDate);
        var totalDays = timeSpan.TotalDays;

        if (totalDays > 0)
        {
            var count = PlayerPrefs.GetInt(UNLOCKED_COUNT_KEY);
            var countValue = count + 1;
            PlayerPrefs.SetInt(UNLOCKED_COUNT_KEY, countValue);

            // set date
            PlayerPrefs.SetString(LAST_DATE_KEY, now);

            var maxCount = GlobalData.instance.dataManager.newUserDatas.data.Max(m => m.day);

            // 리워드 지급 완료
            if (countValue >= maxCount)
            {
                PlayerPrefs.SetInt(REWARD_COMPLETE_KEY, 1);
                countValue = maxCount;
            }
            else
            {
                PlayerPrefs.SetInt(REWARD_COMPLETE_KEY, 0);
            }
        }
        else if (totalDays == 0)
        {
            Debug.Log("동일한 날");
        }

        unLockCount = PlayerPrefs.GetInt(UNLOCKED_COUNT_KEY);
        Debug.Log("count : " + unLockCount);
    }

    bool HasLastDate()
    {
        return PlayerPrefs.HasKey(LAST_DATE_KEY);
    }

    bool HasLastCount()
    {
        return PlayerPrefs.HasKey(UNLOCKED_COUNT_KEY);
    }
}
