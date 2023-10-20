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
        //���� ������Ʈ�� ��Ȱ��ȭ ��Ŵ
    }

    public void ShowGuideText()
    {
        //guideMessage ��ǥ ���̵� �ؽ�Ʈ�� ������
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
