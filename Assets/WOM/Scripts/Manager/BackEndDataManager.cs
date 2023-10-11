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


    /* user ad view count info */
    int adViewCount = 0;

    void Start()
    {

    }
    public IEnumerator Init()
    {
        if (HasFirestCreateKey())
        {
            GetUserInDate();
        }
        yield return null;
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

    public void UpdateAdViewCount()
    {
        var bro = Backend.GameData.GetMyData("adViewInfo", new Where(), 0);
        if (bro.IsSuccess())
        {
            Debug.Log("get my table data - adViewInfo  Success");
            if (bro.GetReturnValuetoJSON()["rows"].Count == 0)
            {
                adViewCount = 1;
                // insert
                Param param = new Param();
                param.Add("adViewCount", adViewCount);
                var bro2 = Backend.GameData.Insert("adViewInfo", param);
                if (bro2.IsSuccess())
                {
                    Debug.Log("insert Success");
                }
                else
                {
                    Debug.Log("insert false " + bro2.GetErrorCode());
                }

            }
            else
            {
                // update
                var indate = bro.FlattenRows()[0]["inDate"].ToString();
                adViewCount = int.Parse(bro.FlattenRows()[0]["adViewCount"].ToString());
                ++adViewCount;

                Param param = new Param();
                param.Add("adViewCount", adViewCount);
                var bro2 = Backend.GameData.UpdateV2("adViewInfo", indate, Backend.UserInDate, param);
                if (bro2.IsSuccess())
                {
                    Debug.Log("counting Success");
                }
                else
                {
                    Debug.Log("counting False " + bro2.GetErrorCode());
                }
            }
        }
        else
        {
            Debug.Log(" get my table data - adViewInfo false " + bro.GetErrorCode());
        }
    }
}
