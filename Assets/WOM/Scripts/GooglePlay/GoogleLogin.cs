using UnityEngine;
using UnityEngine.UI;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.SceneManagement;

public class GoogleLogin : MonoBehaviour
{
    void Start()
    {
        PlayGamesPlatform.InitializeInstance(new PlayGamesClientConfiguration.Builder().Build());
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
    }

    public void LogIn()
    {
        Debug.Log("Try Login");
        //?ес????? ???? ??????
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
