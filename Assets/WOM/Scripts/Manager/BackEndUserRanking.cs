using UnityEngine;
using System.Collections.Generic;
using BackEnd;


public class BackEndUserRanking : MonoBehaviour
{

    string rankDataTable = "userRankingInfo_a";
    string uuid = "c4e4c3e0-699e-11ee-866d-39853ca909e5";
    string rowIndate = "none";
    bool isValidIndate = false;

    List<RankingData> rankingListTotal;
    RankingData myRankInfo;


    void Start()
    {

    }


    public void SetRankingData()
    {
        if (rowIndate == "none")
        {
            rowIndate = GetIndate(rankDataTable, out bool success);
            isValidIndate = success;
        }

        if (isValidIndate)
        {
            Param param = new Param();
            param.Add("score", 100);
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
        rankingListTotal = new List<RankingData>();
        var rankingList = Backend.URank.User.GetRankList(uuid);
        if (rankingList.IsSuccess())
        {

            var jsonData = rankingList.GetFlattenJSON();

            for (int i = 0; i < jsonData["rows"].Count; i++)
            {
                RankingData data = new RankingData();
                var rank = jsonData["rows"][i]["rank"].ToString();
                var nickName = jsonData["rows"][i]["nickname"].ToString();
                var score = jsonData["rows"][i]["score"].ToString();
                var result = $"{rank} 위! {nickName} 님의 점수는 {score} 점 입니다.";

                data.rank = rank;
                data.nickName = nickName;
                data.score = score;
                rankingListTotal.Add(data);

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
            myRankInfo = new RankingData();
            var jsonData = myRank.GetFlattenJSON();
            var rank = jsonData["rows"][0]["rank"].ToString();
            var nickName = jsonData["rows"][0]["nickname"].ToString();
            var score = jsonData["rows"][0]["score"].ToString();
            var result = $" {nickName} 님의 점수는 {score} 점 이고 현재 {rank}위 입니다.";

            myRankInfo.rank = rank;
            myRankInfo.nickName = nickName;
            myRankInfo.score = score;

            Debug.Log(result);
        }
        else
        {
            Debug.Log("get my rank fail" + myRank.GetErrorCode() + " " + myRank.GetMessage());
        }

    }

}


[System.Serializable]
public class RankingData
{
    public string rank;
    public string nickName;
    public string score;

}