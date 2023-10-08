using UnityEngine;
using System.Collections;
using BackEnd;

public class BackEndDataManager : MonoBehaviour
{
    string inData = "none";

    const string dbFirstCreateKey = "stageInfoDbCreateKey";
    const string tableName = "stageInfo";
    const string columeName = "stageId";
    int stageId;

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


}
