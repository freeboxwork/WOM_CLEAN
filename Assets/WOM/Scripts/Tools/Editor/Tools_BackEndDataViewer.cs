using UnityEngine;
using UnityEditor;
using BackEnd;
using System.Collections.Generic;


public class Tools_BackEndDataViewer : EditorWindow
{
    [MenuItem("GM_TOOLS/BackEndDataViewer")]
    public static void ShowWindow()
    {
        GetWindow<Tools_BackEndDataViewer>("GM_TOOLS/BackEndDataViewer");
    }

    string userId;
    string userPw;
    string nickName;
    public int score;

    void OnEnable()
    {
        Backend.Initialize(true);

    }


    void OnGUI()
    {
        userId = EditorGUILayout.TextField("userId", userId);
        userPw = EditorGUILayout.TextField("userPw", userPw);

        if (GUILayout.Button("SignUp"))
        {
            SignUp();
        }

        if (GUILayout.Button("LogIn"))
        {
            LogIn();
        }

        nickName = EditorGUILayout.TextField("nickName", nickName);
        if (GUILayout.Button("MakeNickName"))
        {
            MakeNickName(nickName);
        }


        score = EditorGUILayout.IntField("score", score);
        if (GUILayout.Button("SetServerData"))
        {
            //GetServerData();
            //UpdateAdViewCount();
            //UpDateOneDay();
            //UpdateStageInfo();
            // UpdateRankingType_1();
        }

        if (GUILayout.Button("GetRankingList"))
        {
            GetTotalRakingList();
            GetMyRankInfo();
        }

        if (GUILayout.Button("UpdateRanking"))
        {
            SetRankingData();
        }
    }

    void SignUp()
    {
        var bro = Backend.BMember.CustomSignUp(userId, userPw, "test");
        if (bro.IsSuccess())
        {
            Debug.Log("Success");
        }
    }

    void LogIn()
    {
        var bro = Backend.BMember.CustomLogin(userId, userPw);
        if (bro.IsSuccess())
        {
            Debug.Log("Success");
            // LoadOneDayADViewCountTableData();
            // LoadADCountTableData();
        }
    }

    public void MakeNickName(string nickName)
    {
        var bro = Backend.BMember.CreateNickname(nickName);
        if (bro.IsSuccess())
        {
            Debug.Log("Nick Name Make Success");
            // insert row -> ranking table 
            Param param = new Param();
            param.Add("score", 0);
            var bro2 = Backend.GameData.Insert("userRankingInfo_a", param);
            if (bro2.IsSuccess())
            {
                Debug.Log("userRankingInfo_a table Insert Success");
            }
            else
            {
                Debug.Log("userRankingInfo_a table Insert Fail");
            }
        }
        else
        {
            var statusCode = bro.GetStatusCode();
            if (statusCode == "409")
            {
                Debug.Log("이미 존재하는 닉네임입니다. 다른 닉네임을 입력해주세요.");
            }
            else
            {
                Debug.Log("Fail");
            }
            Debug.Log(bro.GetStatusCode() + " " + bro.GetMessage());
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

    void UpdateRankingType_1()
    {
        string key = Backend.UserInDate;

        Param param = new Param();
        param.Add("key", key);
        param.Add("score", score);

        Where where = new Where();
        where.Equal("key", key);

        var bro = Backend.GameData.Update("ranking_type_1", where, param);
        if (bro.IsSuccess())
        {
            Debug.Log("update Success");
            return;
        }

        var insert = Backend.GameData.Insert("ranking_type_1", param);
        if (insert.IsSuccess())
        {
            Debug.Log("insert Success");
        }
        else
        {
            Debug.Log("insert Fail : " + bro.GetMessage());
        }
    }

    //99f07c40-6987-11ee-b9b7-293ad03fc165


    void GetRankingType_1()
    {
        var bro = Backend.URank.User.GetRankList("99f07c40-6987-11ee-b9b7-293ad03fc165");
        if (bro.IsSuccess())
        {
            Debug.Log("Success");
            Debug.Log(bro.GetReturnValuetoJSON()["rows"].Count);
            var row = bro.GetReturnValuetoJSON()["rows"][0];
            Debug.Log(row);
            Debug.Log(row["score"].ToString());
        }
        else
        {
            Debug.Log(bro.GetErrorCode());
        }
    }


    public void GetMyRankTest()
    {
        string userUuid = "99f07c40-6987-11ee-b9b7-293ad03fc165";

        int limit = 100;

        List<RankItem> rankItemList = new List<RankItem>();

        BackendReturnObject bro = Backend.URank.User.GetRankList(userUuid, limit);

        if (bro.IsSuccess())
        {
            var rankListJson = bro.GetFlattenJSON();

            string extraName = string.Empty;

            for (int i = 0; i < rankListJson["rows"].Count; i++)
            {
                RankItem rankItem = new RankItem();

                rankItem.gamerInDate = rankListJson["rows"][i]["gamerInDate"].ToString();
                rankItem.nickname = rankListJson["rows"][i]["nickname"].ToString();
                rankItem.score = rankListJson["rows"][i]["score"].ToString();
                rankItem.index = rankListJson["rows"][i]["index"].ToString();
                rankItem.rank = rankListJson["rows"][i]["rank"].ToString();
                rankItem.totalCount = rankListJson["totalCount"].ToString();

                if (rankListJson["rows"][i].ContainsKey(rankItem.extraName))
                {
                    rankItem.extraData = rankListJson["rows"][i][rankItem.extraName].ToString();
                }

                rankItemList.Add(rankItem);
                Debug.Log(rankItem.ToString());
            }
        }
        else
        {
            Debug.Log(bro.GetErrorCode());
        }
    }

    void UpdateRanking()
    {
        string key = Backend.UserInDate;

        Param param = new Param();
        param.Add("score", score);

        string userUuid = "99f07c40-6987-11ee-b9b7-293ad03fc165";

        var r = Backend.URank.User.UpdateUserScore(userUuid, "ranking_type_1", "key", param);
        if (r.IsSuccess())
        {
            Debug.Log("update Success");
            return;
        }
        else
        {
            Debug.Log(r.GetErrorCode());
        }
    }



    // ranking --- test....

    string rankDataTable = "userRankingInfo_a";
    string uuid = "c4e4c3e0-699e-11ee-866d-39853ca909e5";
    string rowIndate = "none";
    bool isValidIndate = false;

    // Start is called before the first frame update


    public void SetRankingData()
    {
        rowIndate = "none";
        if (rowIndate == "none")
        {
            rowIndate = GetIndate(rankDataTable, out bool success);
            isValidIndate = success;
        }

        if (isValidIndate)
        {
            Param param = new Param();
            param.Add("score", score);
            var broUpdate = Backend.URank.User.UpdateUserScore(uuid, rankDataTable, rowIndate, param);
            if (broUpdate.IsSuccess())
            {
                Debug.Log(" ranking data update success");
            }
            else
            {
                Debug.Log("ranking data update fail" + broUpdate.GetStatusCode() + " " + broUpdate.GetMessage());
            }
        }
        else
        {
            Debug.Log("ranking data indate is not valid");
        }
    }

    string GetIndate(string tableName, out bool success)
    {
        var bro = Backend.GameData.GetMyData(tableName, new Where());
        if (bro.IsSuccess())
        {
            if (bro.GetReturnValuetoJSON()["rows"].Count == 0)
            {
                success = false;
                return "none";

            }
            else
            {
                success = true;
                return bro.FlattenRows()[0]["inDate"].ToString();
            }
        }
        else
        {
            success = false;
            return "none";
        }
    }


    void GetTotalRakingList()
    {
        var rankingList = Backend.URank.User.GetRankList(uuid);
        if (rankingList.IsSuccess())
        {

            var jsonData = rankingList.GetFlattenJSON();

            for (int i = 0; i < jsonData["rows"].Count; i++)
            {
                var rank = jsonData["rows"][i]["rank"].ToString();
                var nickName = jsonData["rows"][i]["nickname"].ToString();
                var score = jsonData["rows"][i]["score"].ToString();
                var result = $"{rank} 위! {nickName} 님의 점수는 {score} 점 입니다.";
                Debug.Log(result);
            }
        }
        else
        {
            Debug.Log("get ranking falid" + rankingList.GetErrorCode() + " " + rankingList.GetMessage());
        }
    }

    void GetMyRankInfo()
    {
        var myRank = Backend.URank.User.GetMyRank(uuid);
        if (myRank.IsSuccess())
        {
            var jsonData = myRank.GetFlattenJSON();
            var rank = jsonData["rows"][0]["rank"].ToString();
            var nickName = jsonData["rows"][0]["nickname"].ToString();
            var score = jsonData["rows"][0]["score"].ToString();
            var result = $" {nickName} 님의 점수는 {score} 점 이고 현재 {rank}위 입니다.";
            Debug.Log(result);

        }
        else
        {
            Debug.Log("get my rank fail" + myRank.GetErrorCode() + " " + myRank.GetMessage());
        }

    }


}


[System.Serializable]
public class RankItem
{

    public string gamerInDate;
    public string nickname;
    public string score;
    public string index;
    public string rank;
    public string totalCount;
    public string extraName;
    public string extraData;

    public override string ToString()
    {
        return string.Format("gamerInDate : {0}, nickname : {1}, score : {2}, index : {3}, rank : {4}, totalCount : {5}, extraName : {6}, extraData : {7}", gamerInDate, nickname, score, index, rank, totalCount, extraName, extraData);
    }

}