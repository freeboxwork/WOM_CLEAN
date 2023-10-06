using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Threading.Tasks; // [변경] async 기능을 이용하기 위해서는 해당 namepsace가 필요합니다.  
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class BackEndManager : MonoBehaviour
{
        private static BackEndManager _instance = null;
    bool isInitSuccess = false;
    public static BackEndManager Instance {
        get {
            if (_instance == null) {
                _instance = new BackEndManager();
            }

            return _instance;
        }
    }

    public bool isLoadComplete = false;

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        var bro = Backend.Initialize(true); // 뒤끝 초기화

        // 뒤끝 초기화에 대한 응답값
        if (bro.IsSuccess())
        {
            isInitSuccess = true;
            Debug.Log("초기화 성공 : " + bro); // 성공일 경우 statusCode 204 Success
        }
        else
        {
            isInitSuccess = false;
            Debug.LogError("초기화 실패 : " + bro); // 실패일 경우 statusCode 400대 에러 발생 
        }

        //Test();
    }

    // 동기 함수를 비동기에서 호출하게 해주는 함수(유니티 UI 접근 불가) 
    // async void Test()
    // {
    //     await Task.Run(() =>
    //     {
    //         BackendLogin.Instance.CustomLogin("user1", "1234");

    //         BackendGameLog.Instance.GameLogInsert(); // [추가] 게임로그 저장 기능


    //         Debug.Log("테스트를 종료합니다.");
    //     });
    // }


    public IEnumerator BackEndLogin()
    {
        // 뒤끝 초기화가 성공한 경우에만 로그인 시도
        if(!isInitSuccess)
        {
            yield break;
        }
        // 이미 구글 로그인 된 경우
        if (Social.localUser.authenticated == true)
        {
            //유저의 구글 토큰으로 뒤끝서버에 로그인 요청
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
            // 페더레이션 인증 이후 처리
            if (BRO.IsSuccess())
            {
                Debug.Log("뒤끝 페더레이션 인증 성공");
                isLoadComplete = true;

                StartCoroutine(RefreshToken());
                //GetUserData();
                //GetData();
            }
            else
            {
                Debug.Log("뒤끝 페더레이션 인증 실패");
            }
        }
        else
        {
            //다시 한번 구글 로그인을 시도합니다.
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // 로그인 성공 -> 뒤끝 서버에 획득한 구글 토큰으로 가입 요청
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                    // 페더레이션 인증 이후 처리
                    if (BRO.IsSuccess())
                    {
                        Debug.Log("뒤끝 페더레이션 인증 성공");
                        isLoadComplete = true;

                        StartCoroutine(RefreshToken());
                        //GetUserData();
                        //GetData();
                    }
                    else
                    {
                        Debug.Log("뒤끝 페더레이션 인증 실패");
                    }
                }
                else
                {
                    // 로그인 실패
                    Debug.Log("Login failed for some reason");
                }
            });
        }
        yield return null;
    }
    // void FederationLogIn()
    // {
    //     var tokenId = GetTokens();
    //     Debug.Log("tokenId:"+tokenId);
    //     Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "GPGS로 가입함", callback =>
    //     {

    //     });
    // }

    // 구글 토큰 받아옴
    string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // 유저 토큰 받기 첫 번째 방법
            //string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // 두 번째 방법
            string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            Debug.Log(_IDtoken);

            return _IDtoken;
        }
        else
        {
            Debug.Log("접속되어 있지 않습니다. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }

    // 로그인 이후 함수를 호출해줍니다.
    //8시간마다 리프레시 하는 로직
    IEnumerator RefreshToken()
    {
        WaitForSeconds waitForRefreshTokenCycle = new WaitForSeconds(60 * 60 * 8); // 60초 x 60분 x 8시간
        WaitForSeconds waitForRetryCycle = new WaitForSeconds(15f);
        // 첫 호출시에는 리프레시 토큰하지 않도록 8시간을 기다리게 해준다.
        bool isStart = false;
        if (!isStart)
        {
            isStart = true;
            yield return waitForRefreshTokenCycle; // 8시간 기다린 후 반복문 시작
        }
        BackendReturnObject bro = null;
        bool isRefreshSuccess = false;
        // 이후부터는 반복문을 돌면서 8시간마다 최대 3번의 리프레시 토큰을 수행하게 된다.
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                bro = Backend.BMember.RefreshTheBackendToken();
                Debug.Log("리프레시 토큰 성공 여부 : " + bro);
                if (bro.IsSuccess())
                {
                    Debug.Log("토큰 재발급 완료");
                    isRefreshSuccess = true;
                    break;
                }
                else
                {
                    if (bro.GetMessage().Contains("bad refreshToken"))
                    {
                        Debug.LogError("중복 로그인 발생");
                        isRefreshSuccess = false;
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("15초 뒤에 토큰 재발급 다시 시도");
                    }
                }
                yield return waitForRetryCycle; // 15초 뒤에 다시시도
            }
            // 3번 이상 재시도시 에러가 발생할 경우, 리프레시 토큰 에러 외에도 네트워크 불안정등의 이유로 이후에도 로그인에 실패할 가능성이 높습니다. 
            // 유저들이 스스로 토큰 리프레시를 할수 있도록 구현해주시거나 수동 로그인을 하도록 구현해주시기 바랍니다.
            if (bro == null)
            {
                Debug.LogError("토큰 재발급에 문제가 발생했습니다. 수동 로그인을 시도해주세요");
                //ShowUI("토큰이 재발급에 실패했습니다. 다시 로그인해주시기 바랍니다.");
            }
            if (!bro.IsSuccess())
            {
                Debug.LogError("토큰 재발급에 문제가 발생하였습니다 : " + bro);
                //ShowUI("토큰 재발급에 문제가 발생하였습니다 : 수동 로그인을 시도해주시기 바랍니다." + bro);
            }
            // 성공할 경우 값 초기화 후 8시간을 wait합니다.
            if (isRefreshSuccess)
            {
                Debug.Log("8시간 토큰 재 호출");
                isRefreshSuccess = false;
                yield return waitForRefreshTokenCycle;
            }
        }
    }




    // public void GetData()
    // {
    //     var bro = Backend.GameData.Get("table", new Where());

    //     if (!bro.IsSuccess())
    //     {
    //         if (bro.GetStatusCode() == "401")
    //         {
    //             if (bro.GetMessage().Contains("accessToken"))
    //             {
    //                 StartCoroutine(RefreshToken());
    //             }
    //         }
    //     }
    // }

    // //뒤끝함수 에러 발생 시 토큰 재발급 하는 로직
    // IEnumerator RefreshToken()
    // {
    //     int count = 0;

    //     WaitForSeconds waitForRetryCycle = new WaitForSeconds(15f);

    //     BackendReturnObject bro = null;

    //     while (count < 3)
    //     {
    //         bro = Backend.BMember.RefreshTheBackendToken();

    //         Debug.Log("리프레시 토큰 : " + bro);

    //         if (bro.IsSuccess())
    //         {
    //             Debug.Log("토큰 재발급 완료");
    //             break;
    //         }
    //         else
    //         {
    //             if (bro.GetMessage().Contains("bad refreshToken"))
    //             {
    //                 Debug.LogError("중복 로그인 발생");
    //                 //ShowUI("중복 로그인이 발생했습니다. 다시 로그인해주시기 바랍니다." + bro);
    //                 break;
    //             }
    //             else
    //             {
    //                 Debug.LogWarning("15초 뒤에 토큰 재발급 다시 시도");
    //             }
    //         }
    //         count++;
    //         yield return waitForRetryCycle;
    //     }
    //     if (bro == null)
    //     {
    //         Debug.LogError("토큰 재발급에 문제가 발생했습니다. 수동 로그인을 시도해주세요");
    //         //ShowUI("토큰이 재발급에 실패했습니다. 다시 로그인해주시기 바랍니다.");
    //     }
    //     // 3번 이상 재시도시 에러가 발생할 경우, 리프레시 토큰 에러 외에도 네트워크 불안정등의 이유로 이후에도 로그인에 실패할 가능성이 높습니다. 유저들이 스스로 토큰 리프레시를 할수 있도록 구현해주시거나 수동 로그인을 하도록 구현해주시기 바랍니다.
    //     if (!bro.IsSuccess())
    //     {
    //         Debug.LogError("토큰 재발급에 문제가 발생했습니다. 수동 로그인을 시도해주세요");
    //         //ShowUI("토큰이 유효하지 않습니다. 다시 로그인해주시기 바랍니다.\n" + bro);
    //     }
    // }
















}
