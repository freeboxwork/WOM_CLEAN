using UnityEngine;
using UnityEditor;
using BackEnd;


public class Tools_BackEndDataViewer : EditorWindow
{
    [MenuItem("GM_TOOLS/BackEndDataViewer")]
    public static void ShowWindow()
    {
        GetWindow<Tools_BackEndDataViewer>("GM_TOOLS/BackEndDataViewer");
    }

    void OnEnable()
    {
        Backend.Initialize(true);

    }


    void OnGUI()
    {
        if (GUILayout.Button("SignUp"))
        {
            SignUp();
        }

        if (GUILayout.Button("LogIn"))
        {
            LogIn();
        }

        if (GUILayout.Button("GetServerData"))
        {
            //GetServerData();
            //UpdateAdViewCount();
            //UpDateOneDay();
            UpdateStageInfo();
        }
    }

    void SignUp()
    {
        var bro = Backend.BMember.CustomSignUp("admin_2", "admin_2", "test");
        if (bro.IsSuccess())
        {
            Debug.Log("Success");
        }
    }

    void LogIn()
    {
        var bro = Backend.BMember.CustomLogin("admin_2", "admin_2");
        if (bro.IsSuccess())
        {
            Debug.Log("Success");
            LoadOneDayADViewCountTableData();
            // LoadADCountTableData();
        }
    }

    void GetServerData()
    {
        string[] select = { "stageId" };

        var bro = Backend.GameData.Get("stageInfo", new Where());
        var jsonData = bro.GetFlattenJSON();

        if (bro.IsSuccess())
        {
            Debug.Log("Success");
            Debug.Log(jsonData["rows"].Count);

        }
        else
        {
            Debug.Log(bro.GetErrorCode());
        }
    }

    BackendReturnObject adViewInfoTable;
    string adViewIndate;
    int adViewCount = 0;
    bool isValidAdViewTable = false;

    void LoadADCountTableData()
    {
        adViewInfoTable = Backend.GameData.Get("adViewInfo", new Where());
        if (adViewInfoTable.IsSuccess())
        {
            isValidAdViewTable = true;
            if (adViewInfoTable.GetReturnValuetoJSON()["rows"].Count == 0)
            {
                adViewCount = 0;
                // insert
                Param param = new Param();
                param.Add("adViewCount", adViewCount);
                var bro2 = Backend.GameData.Insert("adViewInfo", param);
                if (bro2.IsSuccess())
                {
                    Debug.Log("insert Success");
                    // get inDate value ( reload )
                    adViewInfoTable = Backend.GameData.Get("adViewInfo", new Where());
                    if (adViewInfoTable.IsSuccess())
                    {

                        adViewIndate = adViewInfoTable.FlattenRows()[0]["inDate"].ToString();
                        Debug.Log(" indate " + adViewIndate + "  " + adViewCount);
                    }
                    else
                    {
                        isValidAdViewTable = false;
                        Debug.Log(adViewInfoTable.GetErrorCode());
                    }
                }
                else
                {
                    isValidAdViewTable = false;
                    Debug.Log(bro2.GetErrorCode());
                }
            }
            else
            {
                // get inDate value
                adViewIndate = adViewInfoTable.FlattenRows()[0]["inDate"].ToString();
                adViewCount = int.Parse(adViewInfoTable.FlattenRows()[0]["adViewCount"].ToString());
                Debug.Log(" indate " + adViewIndate + "  " + adViewCount);
            }
        }
        else
        {
            isValidAdViewTable = false;
        }

    }
    void UpdateAdViewCount()
    {
        if (isValidAdViewTable)
        {
            ++adViewCount;
            Param param = new Param();
            param.Add("adViewCount", adViewCount);
            var bro2 = Backend.GameData.UpdateV2("adViewInfo", adViewIndate, Backend.UserInDate, param);
            if (bro2.IsSuccess())
            {
                Debug.Log("count Success");
            }
            else
            {
                Debug.Log(bro2.GetErrorCode());
            }
        }
        else
        {
            Debug.Log("adViewInfo 테이블 조회 및 row 생성 실패");
        }

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
                UpDateOneDay();
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

    void UpDateOneDay()
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

    void UpdateStageInfo()
    {
        int stageId = 1;//GlobalData.instance.player.stageIdx;
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
}
