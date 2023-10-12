using UnityEngine;
using System.Collections;
using BackEnd;

public class BackEndDataManager : MonoBehaviour
{
    /* user stage info */
    int stageId;

    /* counting ad view one day */
    int adViewCount;


    void Start()
    {

    }
    public IEnumerator Init()
    {
        LoadOneDayADViewCountTableData();
        yield return null;
    }

    void LoadOneDayADViewCountTableData()
    {
        Where where = new Where();
        var value = Backend.UserInDate;
        where.Equal("key", value);

        var bro = Backend.GameData.GetMyData("adViewInfoOneDay", where);
        if (bro.IsSuccess())
        {
            if (bro.GetReturnValuetoJSON()["rows"].Count == 0)
            {
                adViewCount = -1;
                UpdateAdViewCount();
            }
            else
            {
                adViewCount = int.Parse(bro.FlattenRows()[0]["adViewCount"].ToString());
                Debug.Log("adViewCount : " + adViewCount);
            }
        }
        else
        {
            adViewCount = 0;
        }
    }


    public void SaveUserStageInfoData()
    {
        stageId = GlobalData.instance.player.stageIdx;
        string key = Backend.UserInDate;

        Param param = new Param();
        param.Add("key", key);
        param.Add("stageId", stageId);

        Where where = new Where();
        where.Equal("key", key);

        var bro = Backend.GameData.Update("stageInfoV2", where, param);
        if (bro.IsSuccess())
        {
            Debug.Log("update Success");
            return;
        }

        var insert = Backend.GameData.Insert("stageInfoV2", param);
        if (insert.IsSuccess())
        {
            Debug.Log("insert Success");
        }
        else
        {
            Debug.Log("insert Fail : " + bro.GetMessage());
        }

    }

    public void ResetAdViewCount()
    {
        adViewCount = -1;
        UpdateAdViewCount();
    }


    public void UpdateAdViewCount()
    {
        ++adViewCount;

        Where where = new Where();
        var value = Backend.UserInDate;
        where.Equal("key", value);

        Param param = new Param();
        param.Add("adViewCount", adViewCount);


        var bro = Backend.GameData.Update("adViewInfoOneDay", where, param);
        if (bro.IsSuccess())
        {
            Debug.Log("update Success");
        }
        else
        {
            Param param2 = new Param();
            param2.Add("key", value);
            param2.Add("adViewCount", adViewCount);
            var insert = Backend.GameData.Insert("adViewInfoOneDay", param2);
            if (insert.IsSuccess())
            {
                Debug.Log("insert Success");
            }
            else
            {

                Debug.Log("insert Fail : " + bro.GetMessage());
            }
        }
    }
}
