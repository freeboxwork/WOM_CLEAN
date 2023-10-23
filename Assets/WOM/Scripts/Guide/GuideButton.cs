using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class GuideButton : MonoBehaviour
{
    public int id;
    public Button button;
    TextMeshProUGUI guideText;
    GuideManager guideManager;
    public TextMeshProUGUI text;
    Color enableColor = new Color(1,1,1,1);
    Color disableColor = new Color(0.4f,0.4f,0.4f,1);
    
    void Awake()
    {
        button = GetComponent<Button>(); 
        guideManager = FindAnyObjectByType<GuideManager>(); 
        guideText = GetComponentInChildren<TextMeshProUGUI>();
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
    public void ShowGuideIndexText(string dataText)
    {
        //아이콘에 표시 될 오픈 조건 Text 표시
        guideText.text = string.Format("{0}", dataText);
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
