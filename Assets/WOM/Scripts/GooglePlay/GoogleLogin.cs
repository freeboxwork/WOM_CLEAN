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
        //�ػ� ����
        //Screen.SetResolution(1080, 1920, true);
        //��Ƽ��ġ ��Ȱ��ȭ
        Input.multiTouchEnabled = false;
        //������ ����
        Application.targetFrameRate = StaticDefine.FRAME_RATE;
        //ȭ�� ���� ��Ȱ��ȭ
        Screen.sleepTimeout = SleepTimeout.NeverSleep;
        DontDestroyOnLoad(this);
    }


    void Start()
    {
        // PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        // PlayGamesPlatform.DebugLogEnabled = true;
        // PlayGamesPlatform.Activate();

         // GPGS �÷����� ����
        PlayGamesClientConfiguration config = new PlayGamesClientConfiguration
        .Builder()
        .RequestServerAuthCode(false)
        .RequestEmail()
        .RequestIdToken()
        .Build();
        //Ŀ���� �� ������ GPGS �ʱ�ȭ
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
