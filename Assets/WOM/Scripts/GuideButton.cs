using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GuideButton : MonoBehaviour
{
    public int id;
    public Button button;
    GuideManager guideManager;
    public TextMeshProUGUI text;
    Color enableColor = new Color(1,1,1,1);
    Color disableColor = new Color(0.4f,0.4f,0.4f,1);
    
    void Awake()
    {
        button = GetComponent<Button>(); 
        guideManager = FindAnyObjectByType<GuideManager>(); 
    }   

    public void ClearGuide()
    {
        //게임 오브젝트를 비활성화 시킴
    }

    public void ShowGuideText()
    {
        //guideMessage 목표 가이드 텍스트를 보여줌
        //GlobalData.instance.globalPopupController.
    }


    void OnEnable()
    {
        text.color = disableColor;
    }
    void OnDisable()
    {
        text.color = enableColor;
    }




}
