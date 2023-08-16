using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class GoogleLogin : MonoBehaviour
{

    void Awake()
    {
        //�ػ� ����
        Screen.SetResolution(1080, 1920, true);
        //��Ƽ��ġ ��Ȱ��ȭ
        Input.multiTouchEnabled = false;
        //������ ����
        Application.targetFrameRate = 60;
        //ȭ�� ���� ��Ȱ��ȭ
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
        //?��????? ???? ??????
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
