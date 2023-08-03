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

    float typingSpeed = 0.01f;
    private string fullText;
    private string currentText = "";

    public void EnableTutorialMask(string message, Image image, Button button)
    {
        tutoSet.gameObject.SetActive(true);

        SetTxtDesc(message);
        imgUnmask.sprite = image.sprite;
        unmask.fitTarget = image.rectTransform;

        tutoBtn.onClick.RemoveAllListeners();
        tutoBtn.onClick.AddListener(() =>
        {
            button.onClick.Invoke();
            tutorialManager.CompleteStep();
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

        yield return new WaitForSeconds(0.2f);
        isTypeAnim = false;
    }

    public void EnableDiscriptionText(string text)
    {
        tutoSet.gameObject.SetActive(true);
        imgUnmask.sprite = null;
        unmask.fitTarget = null;
        SetTxtDesc(text);
    }

    void SetTxtDesc(string value)
    {
        fullText = value;
        currentText = "";
        //txtDesc.text = value;   
        StopAllCoroutines();
        StartCoroutine(TypeText());
    }

    public void DisableTutorial()
    {
        tutoSet.gameObject.SetActive(false);
    }
}
