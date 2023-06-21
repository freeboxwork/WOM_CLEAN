using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NewUserSlot : MonoBehaviour
{

    public TextMeshProUGUI txtDay;
    public TextMeshProUGUI[] txtRewardValues;
    public Image[] imgRewardIcons;
    public Image imgBlockImage;
    public Button btnReward;

    NewUserData newUserData;


    private void Start()
    {
        SetBtnEvents();
    }

    public void SetUI(NewUserData data)
    {
        newUserData = data;
        var icons = new Sprite[] { GetRewardIcon(newUserData.rewardType_1), GetRewardIcon(newUserData.rewardType_2), GetRewardIcon(newUserData.rewardType_3) };
        var values = new int[] { newUserData.rewardValue_1, newUserData.rewardValue_2, newUserData.rewardValue_3 };

        SetRewardIcon(icons);
        SetTxtRewardValue(values);
        SetTxtDayCount(data.day.ToString());
    }

    Sprite GetRewardIcon(string name)
    {
        var icon = GlobalData.instance.spriteDataManager.GetRewardIcon(UtilityMethod.GetRewardTypeByTypeName(name));

        Debug.Log($"name : {name} , icon : {icon} ");

        if (icon == null)
            icon = null;
        return icon;
    }

    void SetBtnEvents()
    {
        btnReward.onClick.AddListener(() =>
        {

        });
    }

    public void SetRewardIcon(Sprite[] sprites) // 보상 아이콘의 스프라이트를 설정합니다.
    {
        for (int i = 0; i < imgRewardIcons.Length; i++)
        {
            imgRewardIcons[i].sprite = sprites[i];
        }
    }

    public void SetTxtRewardValue(int[] values) // 보상 가치의 텍스트를 설정합니다.
    {
        for (int i = 0; i < txtRewardValues.Length; i++)
        {
            txtRewardValues[i].text = values[i].ToString();
        }
    }

    public void SetTxtDayCount(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        txtDay.text = $"{value} 일차";
    }

    public void SetBlockImage(bool isActive)
    {
        imgBlockImage.gameObject.SetActive(isActive);
    }


    // btn_reward 의interactable 를 설정합니다.
    public void SetBtnRewardInteractable(bool isActive)
    {
        btnReward.interactable = isActive;
    }






}
