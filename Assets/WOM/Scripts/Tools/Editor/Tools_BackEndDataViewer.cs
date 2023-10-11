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
            UpdateAdViewCount();
        }
    }

    void SignUp()
    {
        var bro = Backend.BMember.CustomSignUp("admin", "admin", "test");
        if (bro.IsSuccess())
        {
            Debug.Log("Success");
        }
    }

    void LogIn()
    {
        var bro = Backend.BMember.CustomLogin("admin", "admin");
        if (bro.IsSuccess())
        {
            Debug.Log("Success");
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

    int adViewCount = 0;
    void UpdateAdViewCount()
    {
        var bro = Backend.GameData.GetMyData("adViewInfo", new Where(), 0);
        if (bro.IsSuccess())
        {
            Debug.Log("Success");
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
                    Debug.Log(bro2.GetErrorCode());
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
                    Debug.Log("count Success");
                }
                else
                {
                    Debug.Log(bro2.GetErrorCode());
                }
            }
        }
        else
        {
            Debug.Log(bro.GetErrorCode());
        }
    }

}
