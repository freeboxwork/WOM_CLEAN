using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class GoogleLogin : MonoBehaviour
{
    public Button btnLogIn;
    public Button btnLogOut;

    void Start()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SetBtnEvents();
    }

    void SetBtnEvents()
    {
        btnLogIn.onClick.AddListener(LogIn);
        btnLogOut.onClick.AddListener(LogOut);
    }
    

    void LogIn()
    {
        Debug.Log("�α��� �õ�");
        //�α����� �ȵǾ� ������
        if (!Social.localUser.authenticated)
        {

            Social.localUser.Authenticate((bool isSuccess) =>
            {
                if (isSuccess)
                {
                    Debug.Log("���� �α��� ����");
                    SceneManager.LoadScene("Main");
                }
                else
                {
                    Debug.Log("���� �α��� ����");

                }
            });

        }
        
    }

    void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("���� �α׾ƿ�"); 
    }
}
