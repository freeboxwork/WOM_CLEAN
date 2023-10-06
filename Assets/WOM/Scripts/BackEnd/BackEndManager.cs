using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BackEnd;
using System.Threading.Tasks; // [����] async ����� �̿��ϱ� ���ؼ��� �ش� namepsace�� �ʿ��մϴ�.  
using GooglePlayGames;
using GooglePlayGames.BasicApi;

public class BackEndManager : MonoBehaviour
{
        private static BackEndManager _instance = null;
    bool isInitSuccess = false;
    public static BackEndManager Instance {
        get {
            if (_instance == null) {
                _instance = new BackEndManager();
            }

            return _instance;
        }
    }

    public bool isLoadComplete = false;

    void Awake()
    {
        _instance = this;
        DontDestroyOnLoad(this);
    }

    void Start()
    {
        var bro = Backend.Initialize(true); // �ڳ� �ʱ�ȭ

        // �ڳ� �ʱ�ȭ�� ���� ���䰪
        if (bro.IsSuccess())
        {
            isInitSuccess = true;
            Debug.Log("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 204 Success
        }
        else
        {
            isInitSuccess = false;
            Debug.LogError("�ʱ�ȭ ���� : " + bro); // ������ ��� statusCode 400�� ���� �߻� 
        }

        //Test();
    }

    // ���� �Լ��� �񵿱⿡�� ȣ���ϰ� ���ִ� �Լ�(����Ƽ UI ���� �Ұ�) 
    // async void Test()
    // {
    //     await Task.Run(() =>
    //     {
    //         BackendLogin.Instance.CustomLogin("user1", "1234");

    //         BackendGameLog.Instance.GameLogInsert(); // [�߰�] ���ӷα� ���� ���


    //         Debug.Log("�׽�Ʈ�� �����մϴ�.");
    //     });
    // }


    public IEnumerator BackEndLogin()
    {
        // �ڳ� �ʱ�ȭ�� ������ ��쿡�� �α��� �õ�
        if(!isInitSuccess)
        {
            yield break;
        }
        // �̹� ���� �α��� �� ���
        if (Social.localUser.authenticated == true)
        {
            //������ ���� ��ū���� �ڳ������� �α��� ��û
            BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
            // ������̼� ���� ���� ó��
            if (BRO.IsSuccess())
            {
                Debug.Log("�ڳ� ������̼� ���� ����");
                isLoadComplete = true;

                StartCoroutine(RefreshToken());
                //GetUserData();
                //GetData();
            }
            else
            {
                Debug.Log("�ڳ� ������̼� ���� ����");
            }
        }
        else
        {
            //�ٽ� �ѹ� ���� �α����� �õ��մϴ�.
            Social.localUser.Authenticate((bool success) =>
            {
                if (success)
                {
                    // �α��� ���� -> �ڳ� ������ ȹ���� ���� ��ū���� ���� ��û
                    BackendReturnObject BRO = Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "gpgs");
                    // ������̼� ���� ���� ó��
                    if (BRO.IsSuccess())
                    {
                        Debug.Log("�ڳ� ������̼� ���� ����");
                        isLoadComplete = true;

                        StartCoroutine(RefreshToken());
                        //GetUserData();
                        //GetData();
                    }
                    else
                    {
                        Debug.Log("�ڳ� ������̼� ���� ����");
                    }
                }
                else
                {
                    // �α��� ����
                    Debug.Log("Login failed for some reason");
                }
            });
        }
        yield return null;
    }
    // void FederationLogIn()
    // {
    //     var tokenId = GetTokens();
    //     Debug.Log("tokenId:"+tokenId);
    //     Backend.BMember.AuthorizeFederation(GetTokens(), FederationType.Google, "GPGS�� ������", callback =>
    //     {

    //     });
    // }

    // ���� ��ū �޾ƿ�
    string GetTokens()
    {
        if (PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // ���� ��ū �ޱ� ù ��° ���
            //string _IDtoken = PlayGamesPlatform.Instance.GetIdToken();
            // �� ��° ���
            string _IDtoken = ((PlayGamesLocalUser)Social.localUser).GetIdToken();
            Debug.Log(_IDtoken);

            return _IDtoken;
        }
        else
        {
            Debug.Log("���ӵǾ� ���� �ʽ��ϴ�. PlayGamesPlatform.Instance.localUser.authenticated :  fail");
            return null;
        }
    }

    // �α��� ���� �Լ��� ȣ�����ݴϴ�.
    //8�ð����� �������� �ϴ� ����
    IEnumerator RefreshToken()
    {
        WaitForSeconds waitForRefreshTokenCycle = new WaitForSeconds(60 * 60 * 8); // 60�� x 60�� x 8�ð�
        WaitForSeconds waitForRetryCycle = new WaitForSeconds(15f);
        // ù ȣ��ÿ��� �������� ��ū���� �ʵ��� 8�ð��� ��ٸ��� ���ش�.
        bool isStart = false;
        if (!isStart)
        {
            isStart = true;
            yield return waitForRefreshTokenCycle; // 8�ð� ��ٸ� �� �ݺ��� ����
        }
        BackendReturnObject bro = null;
        bool isRefreshSuccess = false;
        // ���ĺ��ʹ� �ݺ����� ���鼭 8�ð����� �ִ� 3���� �������� ��ū�� �����ϰ� �ȴ�.
        while (true)
        {
            for (int i = 0; i < 3; i++)
            {
                bro = Backend.BMember.RefreshTheBackendToken();
                Debug.Log("�������� ��ū ���� ���� : " + bro);
                if (bro.IsSuccess())
                {
                    Debug.Log("��ū ��߱� �Ϸ�");
                    isRefreshSuccess = true;
                    break;
                }
                else
                {
                    if (bro.GetMessage().Contains("bad refreshToken"))
                    {
                        Debug.LogError("�ߺ� �α��� �߻�");
                        isRefreshSuccess = false;
                        break;
                    }
                    else
                    {
                        Debug.LogWarning("15�� �ڿ� ��ū ��߱� �ٽ� �õ�");
                    }
                }
                yield return waitForRetryCycle; // 15�� �ڿ� �ٽýõ�
            }
            // 3�� �̻� ��õ��� ������ �߻��� ���, �������� ��ū ���� �ܿ��� ��Ʈ��ũ �Ҿ������� ������ ���Ŀ��� �α��ο� ������ ���ɼ��� �����ϴ�. 
            // �������� ������ ��ū �������ø� �Ҽ� �ֵ��� �������ֽðų� ���� �α����� �ϵ��� �������ֽñ� �ٶ��ϴ�.
            if (bro == null)
            {
                Debug.LogError("��ū ��߱޿� ������ �߻��߽��ϴ�. ���� �α����� �õ����ּ���");
                //ShowUI("��ū�� ��߱޿� �����߽��ϴ�. �ٽ� �α������ֽñ� �ٶ��ϴ�.");
            }
            if (!bro.IsSuccess())
            {
                Debug.LogError("��ū ��߱޿� ������ �߻��Ͽ����ϴ� : " + bro);
                //ShowUI("��ū ��߱޿� ������ �߻��Ͽ����ϴ� : ���� �α����� �õ����ֽñ� �ٶ��ϴ�." + bro);
            }
            // ������ ��� �� �ʱ�ȭ �� 8�ð��� wait�մϴ�.
            if (isRefreshSuccess)
            {
                Debug.Log("8�ð� ��ū �� ȣ��");
                isRefreshSuccess = false;
                yield return waitForRefreshTokenCycle;
            }
        }
    }




    // public void GetData()
    // {
    //     var bro = Backend.GameData.Get("table", new Where());

    //     if (!bro.IsSuccess())
    //     {
    //         if (bro.GetStatusCode() == "401")
    //         {
    //             if (bro.GetMessage().Contains("accessToken"))
    //             {
    //                 StartCoroutine(RefreshToken());
    //             }
    //         }
    //     }
    // }

    // //�ڳ��Լ� ���� �߻� �� ��ū ��߱� �ϴ� ����
    // IEnumerator RefreshToken()
    // {
    //     int count = 0;

    //     WaitForSeconds waitForRetryCycle = new WaitForSeconds(15f);

    //     BackendReturnObject bro = null;

    //     while (count < 3)
    //     {
    //         bro = Backend.BMember.RefreshTheBackendToken();

    //         Debug.Log("�������� ��ū : " + bro);

    //         if (bro.IsSuccess())
    //         {
    //             Debug.Log("��ū ��߱� �Ϸ�");
    //             break;
    //         }
    //         else
    //         {
    //             if (bro.GetMessage().Contains("bad refreshToken"))
    //             {
    //                 Debug.LogError("�ߺ� �α��� �߻�");
    //                 //ShowUI("�ߺ� �α����� �߻��߽��ϴ�. �ٽ� �α������ֽñ� �ٶ��ϴ�." + bro);
    //                 break;
    //             }
    //             else
    //             {
    //                 Debug.LogWarning("15�� �ڿ� ��ū ��߱� �ٽ� �õ�");
    //             }
    //         }
    //         count++;
    //         yield return waitForRetryCycle;
    //     }
    //     if (bro == null)
    //     {
    //         Debug.LogError("��ū ��߱޿� ������ �߻��߽��ϴ�. ���� �α����� �õ����ּ���");
    //         //ShowUI("��ū�� ��߱޿� �����߽��ϴ�. �ٽ� �α������ֽñ� �ٶ��ϴ�.");
    //     }
    //     // 3�� �̻� ��õ��� ������ �߻��� ���, �������� ��ū ���� �ܿ��� ��Ʈ��ũ �Ҿ������� ������ ���Ŀ��� �α��ο� ������ ���ɼ��� �����ϴ�. �������� ������ ��ū �������ø� �Ҽ� �ֵ��� �������ֽðų� ���� �α����� �ϵ��� �������ֽñ� �ٶ��ϴ�.
    //     if (!bro.IsSuccess())
    //     {
    //         Debug.LogError("��ū ��߱޿� ������ �߻��߽��ϴ�. ���� �α����� �õ����ּ���");
    //         //ShowUI("��ū�� ��ȿ���� �ʽ��ϴ�. �ٽ� �α������ֽñ� �ٶ��ϴ�.\n" + bro);
    //     }
    // }
















}
