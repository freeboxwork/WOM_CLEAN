using UnityEngine;
using UnityEngine.UI;

public class BtnScreenClick : MonoBehaviour
{
    public Button btnScreen;


    void Start()
    {
        SetBtnEvent();
    }

    void SetBtnEvent()
    {
        btnScreen.onClick.AddListener(() =>
        {
            Debug.Log("스크린 클릭");
            EventManager.instance.RunEvent(CallBackEventType.TYPES.OnTutorialScreenClick);
        });
    }

}
