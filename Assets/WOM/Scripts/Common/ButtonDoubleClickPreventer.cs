using UnityEngine;
using UnityEngine.UI;

public class ButtonDoubleClickPreventer : MonoBehaviour
{
    public float disableTime = 1.0f; // 버튼 비활성화 시간

    private Button button;
    private bool isButtonDisabled = false;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    private void OnEnable()
    {
        button.onClick.AddListener(OnClickButton);
    }

    private void OnDisable()
    {
        button.onClick.RemoveListener(OnClickButton);
    }

    private void OnClickButton()
    {
        if (!isButtonDisabled)
        {
            isButtonDisabled = true;
            button.interactable = false;
            Invoke("EnableButton", disableTime);
        }
    }

    private void EnableButton()
    {
        
        isButtonDisabled = false;
        button.interactable = true;
    }
}
