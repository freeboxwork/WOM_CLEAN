using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;

public class IntroManager : MonoBehaviour
{
    public GoogleLogin googleLogin;
    public IntroTimeLineController introTimeLineController;

    const string isSecondConnetKey = "isFirstConnetKey";
    [SerializeField]bool isSecondConnect = false;


    public GameObject mainCanvas;
    //Start��ư
    public Button startButton;
    public TextMeshProUGUI startButtonText;
    //�ε���
    public Image fillImage;
    public TextMeshProUGUI progressText;
    private float loadProgress;

    private bool isLoad = false;
    public Color endColor;
    private AsyncOperation async; // �ε�
    public WOMAppUpdateManager wOMAppUpdateManager;

    void Awake()
    {
        startButton.onClick.AddListener(GoMainScene); 
        ShowMainCanvas(false);
        ShowStartButton(false);

    }

    void Start()
    {

        FirstConectCheck();

        if (isSecondConnect)
        {
            // SKIP INTRO
            introTimeLineController.SkipIntro();
            ShowMainCanvas(true);
#if UNITY_ANDROID && !UNITY_EDITOR
            StartCoroutine(wOMAppUpdateManager.AppUpdateCheck());
#endif
            //�� �����ε�
            StartCoroutine(SceneLoad());
        }
        else
        {
            // PLAY INTRO
            //��Ʈ�ΰ� ���� �� ĵ���� Ȱ��ȭ
        }


#if !UNITY_EDITOR && UNITY_ANDROID
        googleLogin.LogIn();
#endif
        //�����ε��� ����ɶ����� ���
        StartCoroutine(WaitSceneLoad());

    }


    IEnumerator WaitSceneLoad()
    {
        while(!isLoad)
        {
            yield return null;
        }

        yield return new WaitForSeconds(2f);
        ShowStartButton(true);

        startButtonText.DOColor(endColor, 1f).SetLoops(-1, LoopType.Yoyo);

    }

    public void ShowStartButton(bool active)
    {

        startButton.gameObject.SetActive(active);
    }
    public void ShowMainCanvas(bool active)
    {

        mainCanvas.gameObject.SetActive(active);
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

    public void GoMainScene()
    {
        if (isLoad)
        {
            async.allowSceneActivation = true;
        }
    }




    public IEnumerator SceneLoad()
    {
        async = SceneManager.LoadSceneAsync("Main");

        async.allowSceneActivation = false;

        //allowSceneActivation�� false �� isDone�� true�� �ٲ��� �ʴ´�.
        while (!async.isDone)
        {
            if (loadProgress >= 0.8f)
            {
                isLoad = true;
            }

            loadProgress = async.progress / 0.9f;

            progressText.text = string.Format("{0:0} %", loadProgress * 100f);

            fillImage.fillAmount = (float)(loadProgress / 0.9f);

            yield return null;
        }
    }







}
