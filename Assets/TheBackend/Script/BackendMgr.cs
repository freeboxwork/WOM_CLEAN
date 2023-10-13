using UnityEngine;
using UnityEngine.UI;
using BackEnd;

/// <summary>
/// 랭킹을 이용하기 위한 간단한 튜토리얼 입니다.
/// 뒤끝 SDK를 사용하는 방법은 뒤끝 SDK 개발자 문서를 참고해주세요.
/// 회원가입/로그인 : https://developer.thebackend.io/unity3d/guide/bmember/signup_login/
/// 길드 : https://developer.thebackend.io/unity3d/guide/social/guild/guildv3/GetMyGuildInfoV3/
/// 유저 랭킹 : https://developer.thebackend.io/unity3d/guide/uRanking/user/getRankTable
/// 길드 랭킹 : https://developer.thebackend.io/unity3d/guide/uRanking/guild/getRankTable
/// 랭킹 보상은 우편으로 지급됩니다.
/// 랭킹 보상을 수령하기 위해서는 우편 개발자 문서와 우편 튜토리얼을 참고해주세요.
/// 우편 : https://developer.thebackend.io/unity3d/guide/social/GetPostListV2/
/// 우편 튜토리얼 : https://developer.thebackend.io/unity3d/guide/tutorial/tutorial/social/
/// </summary>

public class BackendMgr : MonoBehaviour
{
    public InputField idField = null;
    public InputField pwField = null;

    public string tableName = string.Empty;
    public string userRankUuid = string.Empty;
    public string metaRankUuid = string.Empty;
    public string goodsRankUuid = string.Empty;

    void Awake()
    {
        // 뒤끝 초기화
        var bro = Backend.Initialize(true);

        if (bro.IsSuccess() == false)
        {
            Debug.LogError("뒤끝 초기화 실패\n" + bro);
            return;
        }

        Debug.Log("뒤끝 초기화 성공");
    }

    void Update()
    {
        // 비동기 콜백 함수 풀링
        if (Backend.IsInitialized)
        {
            Backend.AsyncPoll();
        }
    }

    // 커스텀 로그인
    public void CustomLogin()
    {
        if (idField == null || pwField == null)
        {
            throw new System.Exception("idField 혹은 pwField가 null 입니다.");
        }
        Backend.BMember.CustomLogin(idField.text, pwField.text, callback =>
        {
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("로그인 실패: " + callback);
                return;
            }

            Debug.Log("로그인 성공");
            Debug.Log(string.Format("유저 inDate: {0} / 유저 닉네임: {1}", Backend.UserInDate, Backend.UserNickName));

            CheckGuildInfo();
        });
    }

    // 커스텀 회원가입
    public void CustomSignUp()
    {
        if (idField == null || pwField == null)
        {
            throw new System.Exception("idField 혹은 pwField가 null 입니다.");
        }

        Backend.BMember.CustomSignUp(idField.text, pwField.text, callback =>
        {
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("회원가입 실패: " + callback);
                return;
            }

            Debug.Log("회원가입 성공");
            Debug.Log(string.Format("유저 inDate: {0} / 유저 닉네임: {1}", Backend.UserInDate, Backend.UserNickName));

            CheckGuildInfo();
        });
    }

    // 길드에 가입되어 있는지 확인
    void CheckGuildInfo()
    {
        // Backend.Social.Guild.GetMyGuildInfoV3(callback =>
        // {
        //     if (callback.IsSuccess() == true)
        //     {
        //         Debug.Log("길드 정보를 조회 성공했습니다: " + callback);
        //         return;
        //     }

        //     Debug.Log("길드 정보가 존재하지 않습니다. 새로운 길드를 생성합니다.");

        //     CreateGuild();
        // });
    }

    // 길드 생성
    void CreateGuild()
    {
        // System.Random rnd = new System.Random();
        // Backend.Social.Guild.CreateGuildV3("guild" + rnd.Next(1, 10000), 1, callback =>
        // {
        //     if (callback.IsSuccess() == false)
        //     {
        //         Debug.LogError("길드 생성 실패: " + callback);
        //         return;
        //     }

        //     Debug.Log("길드 생성 성공: " + callback);
        // });
    }

    // 유저 랭킹을 갱신하기 위해서는 먼저 테이블의 rowIndate를 알아야 되기 때문에 
    // 테이블 조회를 먼저 수행
    public void GetAndUpdateUserScore()
    {
        // 테이블 명이 존재하는지 확인
        if (string.IsNullOrEmpty(tableName))
        {
            throw new System.Exception("tableName is empty");
        }

        Backend.GameData.Get(tableName, new Where(), callback =>
        {
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("게임 정보 조회 실패: " + callback);
                return;
            }

            Debug.Log("게임 정보 조회 성공: " + callback);

            var data = callback.FlattenRows();

            if (data.Count < 0)
            {
                Debug.LogError("게임정보가 비어있습니다.");
                return;
            }

            var indate = data[0]["inDate"].ToString();

            // 테이블 조회 후 갱신할 테이블의 inDate를 조회했으면,
            // 해당 정보를 이용하여 랭킹 갱신
            UpdateUserScore(indate);
        });
    }

    // 랭킹 점수 갱신
    void UpdateUserScore(string inDate)
    {
        // user rank uuid가 존재하는지 확인
        if (string.IsNullOrEmpty(userRankUuid))
        {
            throw new System.Exception("userRankUuid is empty");
        }

        System.Random rnd = new System.Random();

        // 랜덤한 숫자로 점수 갱신
        Param param = new Param();
        param.Add("score", rnd.Next());

        // tableName 테이블에 inDate row의 score 컬럼을 rnd.Next()의 리턴값으로 갱신하면서,
        // 랭킹을 갱신
        Backend.URank.User.UpdateUserScore(userRankUuid, tableName, inDate, param, bro =>
        {
            if (bro.IsSuccess() == false)
            {
                Debug.LogError("userRank 랭킹 갱신 실패: " + bro);
                return;
            }

            Debug.Log("userRank 랭킹 갱신 성공: " + bro);
        });
    }

    // 길드 메타 랭킹 점수 갱신
    public void UpdateGuildMeta()
    {
        // meta rank uuid가 존재하는지 확인
        if (string.IsNullOrEmpty(metaRankUuid))
        {
            throw new System.Exception("metaRankUuid is empty");
        }

        System.Random rnd = new System.Random();

        // 길드의 score 메타 데이터를 rnd.Next()의 리턴값으로 갱신하면서,
        // 랭킹을 갱신
        Backend.URank.Guild.UpdateGuildMetaData(metaRankUuid, "score", rnd.Next(), bro =>
        {
            if (bro.IsSuccess() == false)
            {
                Debug.LogError("metaRank 랭킹 갱신 실패: " + bro);
                return;
            }

            Debug.Log("metaRank 랭킹 갱신 성공: " + bro);
        });
    }

    // 길드 굿즈 랭킹 점수 갱신
    public void ContirbuteGoods()
    {
        // goods rank uuid가 존재하는지 확인
        if (string.IsNullOrEmpty(goodsRankUuid))
        {
            throw new System.Exception("goodsRankUuid is empty");
        }

        System.Random rnd = new System.Random();

        // 길드 굿즈1에 10 ~ 1000 사이의 값을 기부하면서
        // 랭킹을 갱신
        Backend.URank.Guild.ContributeGuildGoods(goodsRankUuid, goodsType.goods1, rnd.Next(10, 1000), bro =>
        {
            if (bro.IsSuccess() == false)
            {
                Debug.LogError("goodsRank 랭킹 갱신 실패: " + bro);
                return;
            }

            Debug.Log("goodsRank 랭킹 갱신 성공: " + bro);
        });
    }

    // 길드 굿즈 랭킹 점수 갱신
    public void UseGoods()
    {
        // goods rank uuid가 존재하는지 확인
        if (string.IsNullOrEmpty(goodsRankUuid))
        {
            throw new System.Exception("goodsRankUuid is empty");
        }

        System.Random rnd = new System.Random();

        // 길드 굿즈1의 값을 10 사용하면서
        // 랭킹을 갱신
        Backend.URank.Guild.UseGuildGoods(goodsRankUuid, goodsType.goods1, 10, bro =>
        {
            if (bro.IsSuccess() == false)
            {
                Debug.LogError("goodsRank 랭킹 갱신 실패: " + bro);
                return;
            }

            Debug.Log("goodsRank 랭킹 갱신 성공: " + bro);
        });
    }

    void CheckRankUuid()
    {
        // user rank uuid가 존재하는지 확인
        if (string.IsNullOrEmpty(userRankUuid))
        {
            throw new System.Exception("userRankUuid is empty");
        }

        // meta rank uuid가 존재하는지 확인
        if (string.IsNullOrEmpty(metaRankUuid))
        {
            throw new System.Exception("metaRankUuid is empty");
        }

        // goods rank uuid가 존재하는지 확인
        if (string.IsNullOrEmpty(goodsRankUuid))
        {
            throw new System.Exception("goodsRankUuid is empty");
        }
    }

    public void GetRankList()
    {
        CheckRankUuid();

        int limit = 10;
        Backend.URank.User.GetRankList(userRankUuid, limit, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("userRank 조회 실패: " + callback);
                return;
            }

            Debug.Log("userRank 갯수: " + callback.GetFlattenJSON()["totalCount"].ToString());

            var data = callback.FlattenRows();

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("rank: {0} / gamerIndate: {1} / score: {2}",
                    data[i]["rank"].ToString(), data[i]["gamerInDate"].ToString(), data[i]["score"].ToString()));
            }
        });

        Backend.URank.Guild.GetRankList(metaRankUuid, limit, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("metaRank 조회 실패: " + callback);
                return;
            }

            Debug.Log("metaRank 갯수: " + callback.GetFlattenJSON()["totalCount"].ToString());

            var data = callback.FlattenRows();

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("rank: {0} / guildName: {1} / score: {2}",
                    data[i]["rank"].ToString(), data[i]["guildName"].ToString(), data[i]["score"].ToString()));
            }
        });

        Backend.URank.Guild.GetRankList(goodsRankUuid, limit, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("goodsRank 조회 실패: " + callback);
                return;
            }

            Debug.Log("goodsRank 갯수: " + callback.GetFlattenJSON()["totalCount"].ToString());

            var data = callback.FlattenRows();

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("rank: {0} / guildName: {1} / score: {2}",
                    data[i]["rank"].ToString(), data[i]["guildName"].ToString(), data[i]["score"].ToString()));
            }
        });
    }

    public void GetMyRank()
    {
        CheckRankUuid();

        Backend.URank.User.GetMyRank(userRankUuid, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("내 userRank 조회 실패: " + callback);
                return;
            }

            var data = callback.FlattenRows();

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("myRank: {0} / score: {1}",
                    data[i]["rank"].ToString(), data[i]["score"].ToString()));
            }
        });

        Backend.URank.Guild.GetMyGuildRank(metaRankUuid, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("내 metaRank 조회 실패: " + callback);
                return;
            }

            var data = callback.FlattenRows();

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("myGuildRank: {0} / score: {1}",
                    data[i]["rank"].ToString(), data[i]["score"].ToString()));
            }
        });

        Backend.URank.Guild.GetMyGuildRank(goodsRankUuid, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("내 goodsRank 조회 실패: " + callback);
                return;
            }

            var data = callback.FlattenRows();

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("myGuildRank: {0} / score: {1}",
                    data[i]["rank"].ToString(), data[i]["score"].ToString()));
            }
        });
    }

    public void GetRankListByScore()
    {
        CheckRankUuid();

        Backend.URank.User.GetRankListByScore(userRankUuid, 100, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("userRank에서 100점인 유저 조회 실패: " + callback);
                return;
            }
            Debug.Log("userRank에서 100점인 유저");
            var data = callback.FlattenRows();
            if (data.Count <= 0)
            {
                Debug.Log("존재하지 않습니다.");
            }

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("rank: {0} / gamerIndate: {1} / score: {2}",
                    data[i]["rank"].ToString(), data[i]["gamerIndate"].ToString(), data[i]["score"].ToString()));
            }
        });

        Backend.URank.Guild.GetRankListByScore(metaRankUuid, 100, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("metaRank에서 100점인 길드 조회 실패: " + callback);
                return;
            }
            Debug.Log("metaRank에서 100점인 길드");
            var data = callback.FlattenRows();
            if (data.Count <= 0)
            {
                Debug.Log("존재하지 않습니다.");
            }

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("rank: {0} / guildName: {1} / score: {2}",
                    data[i]["rank"].ToString(), data[i]["guildName"].ToString(), data[i]["score"].ToString()));
            }
        });

        Backend.URank.Guild.GetRankListByScore(goodsRankUuid, 100, callback =>
        {
            Debug.Log("-------------------------------------------------------");
            if (callback.IsSuccess() == false)
            {
                Debug.LogError("goodsRank에서 100점인 길드 조회 실패: " + callback);
                return;
            }
            Debug.Log("goodsRank에서 100점인 길드");
            var data = callback.FlattenRows();
            if (data.Count <= 0)
            {
                Debug.Log("존재하지 않습니다.");
            }

            for (int i = 0; i < data.Count; ++i)
            {
                Debug.Log(string.Format("rank: {0} / guildName: {1} / score: {2}",
                    data[i]["rank"].ToString(), data[i]["guildName"].ToString(), data[i]["score"].ToString()));
            }
        });
    }
}
