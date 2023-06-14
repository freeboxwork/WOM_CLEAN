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

    bool isUsingReward = false;



    private void Start()
    {
        SetBtnEvents();
    }

    void SetBtnEvents()
    {
        btnReward.onClick.AddListener(() =>
        {
            // var rewardType = UtilityMethod.GetRewardTypeByTypeName(attendData.rewardType);
            // 보상 지급
            EventManager.instance.RunEvent<string, int>(CallBackEventType.TYPES.OnUsingRewardAttend, attendData.rewardType, attendData.rewardValue);

            var saveKey = $"{GlobalData.instance.questManager.keyAttendUsedReawrd}_{attendData.day}";

            Debug.Log($"saveKey : {saveKey}");

            PlayerPrefs.SetInt(saveKey, 1);

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


    // btn_showQuestPopup 의interactable 를 설정합니다.
    public void SetBtnRewardInteractable(bool isActive)
    {
        btnReward.interactable = isActive;
    }





}
