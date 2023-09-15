using System.Collections;
using UnityEngine;
using Google.Play.Common;
using Google.Play.AppUpdate;
using TMPro;


public class WOMAppUpdateManager : MonoBehaviour
{

    public TextMeshProUGUI versionText;

    void Start()
    {

#if UNITY_ANDROID&&!UNITY_EDITOR
        StartCoroutine(AppUpdateCheck());
#endif
    }


    public IEnumerator AppUpdateCheck()
    {
        // 매니저 정의
        AppUpdateManager appUpdateManager = new AppUpdateManager();
        // 업데이트 체크
        PlayAsyncOperation<AppUpdateInfo, AppUpdateErrorCode> appUpdateInfoTask = appUpdateManager.GetAppUpdateInfo();
        // 업데이트 체크 완료까지 기다림
        yield return appUpdateInfoTask;
        // 업데이트 정보를 성공적으로 받아옴
        if (appUpdateInfoTask.IsSuccessful)
        {
            AppUpdateInfo appUpdateResult = appUpdateInfoTask.GetResult();
            //버전코드 가져오기
            var code = appUpdateResult.AvailableVersionCode;
            versionText.text = $"version : {code}";
            // 업데이트가 필요한 경우
            if (appUpdateResult.UpdateAvailability == UpdateAvailability.UpdateAvailable)
            {
                // 업데이트를 시작
                var appUpdateOption = AppUpdateOptions.ImmediateAppUpdateOptions();
                var startUpdateReq = appUpdateManager.StartUpdate(appUpdateResult, appUpdateOption);
                // 업데이트 완료까지 기다림
                yield return startUpdateReq;
                // 업데이트가 성공적으로 완료됨
                if (startUpdateReq.IsDone)
                {
                    // 업데이트 완료
                    Debug.Log("APP 업데이트 완료");

                }
                // 업데이트가 실패한 경우
                else
                {
                    // 업데이트 실패
                    Debug.Log("APP 업데이트 실패");
                }
            }
            // 업데이트가 필요하지 않은 경우
            else
            {
                // 업데이트 필요 없음
                Debug.Log("APP 업데이트 필요하지 않음");
            }
        }
        else
        {
            Debug.Log("APP 업데이트 정보 받기 실패");
        }

    }


}
