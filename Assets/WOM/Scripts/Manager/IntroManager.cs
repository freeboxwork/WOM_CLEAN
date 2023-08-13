using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public GoogleLogin googleLogin;
    public IntroTimeLineController introTimeLineController;

    const string isSecondConnetKey = "isFirstConnetKey";
    bool isSecondConnect = false;

    void Start()
    {
        FirstConectCheck();
        if (isSecondConnect)
        {
            // SKIP INTRO
            introTimeLineController.SkipIntro();
            googleLogin.LogIn();
        }
        else
        {
            // PLAY INTRO
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    void FirstConectCheck()
    {
        if (PlayerPrefs.HasKey(isSecondConnetKey))
        {
            isSecondConnect = PlayerPrefs.GetInt(isSecondConnetKey) == 1 ? true : false;
        }
        else
        {
            isSecondConnect = false;
            PlayerPrefs.SetInt(isSecondConnetKey, 1);
        }

    }

    public void GoogleLogin()
    {
        googleLogin.LogIn();
    }
}
