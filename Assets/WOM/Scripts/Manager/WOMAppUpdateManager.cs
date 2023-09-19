using System.Collections;
using UnityEngine;
using Google.Play.Common;
using Google.Play.AppUpdate;
using TMPro;


public class WOMAppUpdateManager : MonoBehaviour
{

    public TextMeshProUGUI versionText;
    public IEnumerator AppUpdateCheck()
    {
        // �Ŵ��� ����
        AppUpdateManager appUpdateManager = new AppUpdateManager();
        // ������Ʈ üũ
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoTask = appUpdateManager.GetAppUpdateInfo();
        // ������Ʈ üũ �Ϸ���� ��ٸ�
        yield return appUpdateInfoTask;
        // ������Ʈ ������ ���������� �޾ƿ�
        if (appUpdateInfoTask.IsSuccessful)
        {
            AppUpdateInfo appUpdateResult = appUpdateInfoTask.GetResult();
            //�����ڵ� ��������
            var code = appUpdateResult.AvailableVersionCode;
            versionText.text = $"version : {code}";
            // ������Ʈ�� �ʿ��� ���
            if (appUpdateResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                // ������Ʈ�� ����
                var appUpdateOption = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateReq = appUpdateManager.StartUpdate(appUpdateResult, appUpdateOption);
                // ������Ʈ �Ϸ���� ��ٸ�
                yield return startUpdateReq;
                // ������Ʈ�� ���������� �Ϸ��
                if (startUpdateReq.IsDone)
                {
                    // ������Ʈ �Ϸ�
                    Debug.Log("APP ������Ʈ �Ϸ�");

                }
                // ������Ʈ�� ������ ���
                else
                {
                    // ������Ʈ ����
                    Debug.Log("APP ������Ʈ ����");
                }
            }
            // ������Ʈ�� �ʿ����� ���� ���
            else
            {
                // ������Ʈ �ʿ� ����
                Debug.Log("APP ������Ʈ �ʿ����� ����");
            }
        }
        else
        {
            Debug.Log("APP ������Ʈ ���� �ޱ� ����");
        }

    }


}
