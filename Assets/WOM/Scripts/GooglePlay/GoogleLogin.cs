using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class GoogleLogin : MonoBehaviour
{

    void Awake()
    {
        //해상도 고정
        Screen.SetResolution(1080, 1920, true);
        //멀티터치 비활성화
        Input.multiTouchEnabled = false;
        //프레임 고정
        Application.targetFrameRate = 60;
        //화면 슬립 비활성화
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this);
    }


    void Start()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void LogIn()
    {
        Debug.Log("Try Login");
        //?α????? ???? ??????
        if (!Social.localUser.authenticated)
        {

            Social.localUser.Authenticate((bool isSuccess) =>
            {
                if (isSuccess)
                {
                    Debug.Log("Login Success");
                    //SceneManager.LoadScene("Main");
                }
                else
                {
                    Debug.Log("Fail Login");

                }
            });

        }

    }

    void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("LogOut");
    }
}
