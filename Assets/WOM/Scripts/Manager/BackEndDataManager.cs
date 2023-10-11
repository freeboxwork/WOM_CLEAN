using UnityEngine;
using System.Collections;
using BackEnd;

public class BackEndDataManager : MonoBehaviour
{
    /* user stage info */
    string inData = "none";
    const string dbFirstCreateKey = "stageInfoDbCreateKey";
    const string tableName = "stageInfo";
    const string columeName = "stageId";
    int stageId;

    /* counting ad view one day */
    int adViewCount;



    void Start()
    {

    }
    public IEnumerator Init()
    {
        if (HasFirestCreateKey())
        {
            GetUserInDate();
        }

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

    bool HasFirestCreateKey()
    {
        return PlayerPrefs.HasKey(dbFirstCreateKey);
    }

    public void SaveUserStageInfoData()
    {
        stageId = GlobalData.instance.player.stageIdx;
        if (HasFirestCreateKey() == false)
        {
            PlayerPrefs.SetInt(dbFirstCreateKey, 1);
            InsertStageData();
        }
        else
        {
            UpdateStageData();
        }
    }
    void GetUserInDate()
    {
        var bro = Backend.GameData.GetMyData("stageInfo", new Where(), 0);
        if (bro.IsSuccess())
        {
            inData = bro.FlattenRows()[0]["inDate"].ToString();
            Debug.Log("inDate : " + inData);
        }
        else
        {
            Debug.Log(bro.GetErrorCode());
            Debug.Log("inDate 정보 가져오기 실패");
        }
    }

    void InsertStageData()
    {

        Param param = new Param();
        param.Add(columeName, GlobalData.instance.player.stageIdx);

        var bro = Backend.GameData.Insert(tableName, param);
        if (bro.IsSuccess())
        {
            Debug.Log("스테이지 정보 저장 성공");
        }
        else
        {
            Debug.Log(bro.GetErrorCode());
            Debug.Log("스테이지 정보 저장 실패");
        }
    }

    void UpdateStageData()
    {
        if (inData == "none")
        {
            return;
        }

        Param param = new Param();
        param.Add(columeName, stageId);

        var bro = Backend.GameData.UpdateV2(tableName, inData, Backend.UserInDate, param);
        if (bro.IsSuccess())
        {
            Debug.Log("스테이지 정보 업데이트 성공");
        }
        else
        {
            Debug.Log(bro.GetErrorCode());
            Debug.Log("스테이지 정보 업데이트 실패");
        }
    }

    //TODO: 초기 한번만 게임 데이터 불러 오도록 수정


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
