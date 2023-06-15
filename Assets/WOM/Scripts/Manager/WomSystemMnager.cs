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


    void QuitPopupCancel()
    {
        // 팝업 닫기
        Debug.Log("게임종료 팝업 닫기");
    }

    void QuitPopupApply()
    {
        StartCoroutine(GlobalData.instance.saveDataManager.SaveDataToFileCoroutine());
    }

}
