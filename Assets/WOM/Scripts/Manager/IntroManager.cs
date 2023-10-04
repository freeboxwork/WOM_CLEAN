using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using DG.Tweening;
using BackEnd;

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
    public GameObject loadingObj;

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
            //���ξ� �̸� �ε�
            StartCoroutine(SceneLoad());
            StartCoroutine(Init());

        }
        else
        {
            loadingObj.SetActive(false);

            // PLAY INTRO
            //��Ʈ�ΰ� ���� �� ĵ���� Ȱ��ȭ
        }



    }

    IEnumerator Init()
    {
        yield return new WaitForSeconds(0.3f);
        //�� ������Ʈ üũ
        yield return StartCoroutine(wOMAppUpdateManager.AppUpdateCheck());
        //���� �α���
        yield return StartCoroutine(googleLogin.LogIn((bool isSuccess) =>
        {
            if (isSuccess)
            {

                StartCoroutine(WaitSceneLoad());

            }
            else
            {
                
            }
        }));

        //�����ε��� ����ɶ����� ���
        //StartCoroutine(WaitSceneLoad());
    }

    public void EndIntro()
    {
        StartCoroutine(SceneLoad());
        StartCoroutine(Init());
        ShowMainCanvas(true);
    }

    IEnumerator WaitSceneLoad()
    {
        while(!isLoad)
        {
            yield return null;
        }
        yield return new WaitForSeconds(1.5f);
        loadingObj.SetActive(false);
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
