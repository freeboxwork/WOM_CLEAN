using System.Collections;
using Coffee.UIExtensions;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TutorialUiController : MonoBehaviour
{
    public GameObject tutoSet;
    public Unmask unmask;
    public Image imgUnmask;
    public TextMeshProUGUI txtDesc;
    public Button tutoBtn;
    public TutorialManager tutorialManager;
    public bool isTypeAnim = false;
    public Image imgBlackBg;

    public Image imgMaskParent;
    public Image txtImageBg;


    float typingSpeed = 0.01f;
    private string fullText;
    private string currentText = "";

    //public GameObject circleHighLight;
    //public RectTransform circleRect;
    // 스크린 터치 버튼
    public GameObject btnScreen;


 
    public void EnableTutorialMask(string message, Image image, Button button, int btnId)
    {
        ShowOnlyText(false);
        tutoSet.gameObject.SetActive(true);

        SetTxtDesc(message);
        imgUnmask.sprite = image.sprite;
        unmask.fitTarget = image.rectTransform;
        //circleRect.position = unmask.fitTarget.position;
        


        tutoBtn.onClick.RemoveAllListeners();
        tutoBtn.onClick.AddListener(() =>
        {
            EventManager.instance.RunEvent<int>(CallBackEventType.TYPES.OnTutorialBtnClick, btnId);
            button.onClick.Invoke();
            //tutorialManager.CompleteStep();
        });
    }
    
    bool skipText;
    IEnumerator TypeText()
    {
        isTypeAnim = true;
        for (int i = 0; i < fullText.Length; i++)
        {
            if (fullText[i] == '<') // < 문자를 만나면 skipText를 true로 설정하여 스킵
            {
                skipText = true;
            }
            else if (fullText[i] == '>') // > 문자를 만나면 skipText를 false로 설정하여 스킵 종료
            {
                skipText = false;
            }
            currentText += fullText[i];
            if (!skipText)
            {
                txtDesc.text = currentText;
                yield return new WaitForSeconds(typingSpeed);
            }

        }

        yield return new WaitForSeconds(0.5f);
        isTypeAnim = false;
    }

    public void EnableDiscriptionText(string text)
    {
        ShowOnlyText(true);
        tutoSet.gameObject.SetActive(true);
        // imgUnmask.sprite = null;
        // unmask.fitTarget = null;
        SetTxtDesc(text);
    }

    void ShowOnlyText(bool value)
    {
        txtImageBg.enabled = true;
        imgBlackBg.gameObject.SetActive(!value);
        imgUnmask.gameObject.SetActive(!value);
        //circleHighLight.SetActive(!value);
    }

    public void SetTxtDesc(string value)
    {
        txtImageBg.enabled = true;
        fullText = value;
        currentText = "";
        //txtDesc.text = value;   
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    public void DisableTutorial()
    {
        txtImageBg.enabled = false;
        tutoSet.gameObject.SetActive(false);
    }

    public void ActiveScreenBtn(bool value)
    {
        btnScreen.SetActive(value);
    }

    public void SetMaskParentImgRaycastTarget(bool value)
    {
        imgMaskParent.raycastTarget = value;
    }


}
