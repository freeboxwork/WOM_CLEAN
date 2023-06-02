using UnityEngine;

public class WomSystemMnager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // 게임 종료 팝업
        if (Input.GetKeyDown(KeyCode.Escape))
        {

            GlobalData.instance.globalPopupController.EnableMessageTwoBtnPopup(18, QuitPopupApply, QuitPopupCancel);

        }
    }


    // 게임 종료
    void QuitGame()
    {
#if UNITY_EDITOR
        // play 종료
        UnityEditor.EditorApplication.isPlaying = false;
#elif UNITY_ANDROID
    // 안드로이드 종료
    // 게임 데이터 저장
    Application.Quit(); 
#endif    
    }


    void QuitPopupCancel()
    {
        // 팝업 닫기
        Debug.Log("게임종료 팝업 닫기");
    }

    void QuitPopupApply()
    {
        QuitGame();
    }

}
