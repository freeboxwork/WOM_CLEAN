using UnityEngine;
using BackEnd;

public class BackEndMakeNickName : MonoBehaviour
{


    void Start()
    {

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


}
