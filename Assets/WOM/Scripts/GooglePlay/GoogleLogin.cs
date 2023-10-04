using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;
using BackEnd;
using System.Collections;
using UnityEngine.Events;

public class GoogleLogin : MonoBehaviour
{

    void Awake()
    {
        //해상도 고정
        //Screen.SetResolution(1080, 1920, true);
        //멀티터치 비활성화
        Input.multiTouchEnabled = false;
        //프레임 고정
        Application.targetFrameRate = StaticDefine.FRAME_RATE;
        //화면 슬립 비활성화
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this);
    }


    void Start()
    {
        // PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        // PlayGamesPlatform.DebugLogEnabled = true;
        // PlayGamesPlatform.Activate();

         // GPGS 플러그인 설정
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        .Builder()
        .RequestServerAuthCode(false)
        .RequestEmail()
        .RequestIdToken()
        .Build();
        //커스텀 된 정보로 GPGS 초기화
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();

    }

    public IEnumerator LogIn(UnityAction<bool> callback)
    {
        Debug.Log("Try Login GPGS");
        
        if (!Social.localUser.authenticated)
        {

            Social.localUser.Authenticate((bool isSuccess) =>
            {
                if (isSuccess)
                {
                    Debug.Log("Login Success");
                    //SceneManager.LoadScene("Main");
                    StartCoroutine(BackEndManager.Instance.BackEndLogin());   
                    callback.Invoke(true);

                }
                else
                {
                    Debug.Log("Fail Login");

                }
            });

        }
        
        yield return null;

    }

}
