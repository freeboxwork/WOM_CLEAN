using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class AttendSlot : MonoBehaviour
{
    public AttendData attendData;
    public Image imgRewardIcon;
    public TextMeshProUGUI txtRewardValue;
    public TextMeshProUGUI txtDayCount;
    public Image blockImage;

    public Button btnReward;

    private void Start()
    {
        SetBtnEvents();
    }

    void SetBtnEvents()
    {
        btnReward.onClick.AddListener(() =>
        {
            //EventManager.instance.RunEvent<int>(CallBackEventType.TYPES.OnQuestCompleteAttend, int.Parse(txtDayCount.text));
            btnReward.interactable = false;
        });
    }

    public void SetRewardIcon(Sprite sprite) // 보상 아이콘의 스프라이트를 설정합니다.
    {
        imgRewardIcon.sprite = sprite;
    }

    public void SetTxtRewardValue(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        txtRewardValue.text = value;
    }

    public void SetTxtDayCount(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        txtDayCount.text = $"{value} Day";
    }

    public void SetBlockImage(bool isActive)
    {
        blockImage.gameObject.SetActive(isActive);
    }








}
