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
        button.onClick.AddListener(HideButton);
    }   
    void OnEnable()
    {
        text.color = disableColor;
    }
    void OnDisable()
    {
        text.color = enableColor;
    }

    public void GetButton()
    {
        button = GetComponent<Button>();
    }

   //컨텐츠가 해금되어 게임 오브젝트를 비활성화 시킴
    public void ClearGuide()
    {
        gameObject.SetActive(false);   
    }

    //해금 조건 텍스트 세팅
    public void ShowGuideIndexText(string dataText)
    {
        guideText.text = string.Format("{0}", dataText);
    }
    
    void HideButton()
    {
        //TEST
        //FindAnyObjectByType<GuideManager>().ClearGuide(id);
    }




}
