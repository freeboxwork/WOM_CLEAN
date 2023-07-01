using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Google.Play.Review;

public class ReviewController : MonoBehaviour
{
    public MainToDonnieNest mainToDonnieNest;
    public Button btnReview;

    void Start()
    {
        AddBtnEvnet();
    }


    void AddBtnEvnet()
    {
        btnReview.onClick.AddListener(() =>
        {
            // two button popup 띄우기
            GlobalData.instance.globalPopupController.EnableMessageTwoBtnPopup(19, BtnApply_GoToReview, BtnCancel_GoToMail);
        });
    }


    void BtnApply_GoToReview()
    {
        StartCoroutine(EnableReview());
    }

    IEnumerator EnableReview()
    {
        ReviewManager reviewManager = new ReviewManager();
        var reqFlow = reviewManager.RequestReviewFlow();

        // 리뷰 플로우 구성을 기다림 (?)
        yield return reqFlow;

        // 에러가 있을경우 에러 출력
        if (reqFlow.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError($"Review flow error : {reqFlow.Error}");
            yield break;
        }

        // 리뷰 플로우 구성이 완료되면 리뷰 플로우를 실행
        var playReviewInfo = reqFlow.GetResult();

        // 리뷰 플로우 실행
        var launchFlow = reviewManager.LaunchReviewFlow(playReviewInfo);

        // 리뷰 플로우 실행을 기다림
        yield return launchFlow;

        // 에러가 있을경우 에러 출력
        if (launchFlow.Error != ReviewErrorCode.NoError)
        {
            Debug.LogError($"Review flow error : {launchFlow.Error}");
            yield break;
        }

        // 리뷰 플로우 실행이 완료되면 리뷰 플로우 실행 결과를 출력
        Debug.Log($"Review flow success : {launchFlow.GetResult()}");
    }



    void BtnCancel_GoToMail()
    {
        mainToDonnieNest.EnableMailWindow();
    }


}
