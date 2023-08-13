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


    public void LogIn()
    {
        Debug.Log("?メ??? ???");
        //?メ????? ???? ??????
        if (!Social.localUser.authenticated)
        {

            Social.localUser.Authenticate((bool isSuccess) =>
            {
                if (isSuccess)
                {
                    Debug.Log("???? ?メ??? ????");
                    SceneManager.LoadScene("Main");
                }
                else
                {
                    Debug.Log("???? ?メ??? ????");

                }
            });

        }

    }

    void LogOut()
    {
        ((PlayGamesPlatform)Social.Active).SignOut();
        Debug.Log("???? ?メ???");
    }
}
