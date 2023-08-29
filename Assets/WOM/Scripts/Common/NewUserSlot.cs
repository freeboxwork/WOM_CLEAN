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

    [SerializeField] Color enableColor;
    [SerializeField] Color disableColor;

    private void Start()
    {
        SetBtnEvents();
    }

    public void SetUI(NewUserData data, int unlockCount)
    {
        newUserData = data;
        var values = new int[] { newUserData.rewardValue_1, newUserData.rewardValue_2, newUserData.rewardValue_3 };
        var icons = new Sprite[] { GetRewardIcon(newUserData.rewardType_1, values[0]), GetRewardIcon(newUserData.rewardType_2, values[1]), GetRewardIcon(newUserData.rewardType_3, values[2]) };



        var isLock = data.id > unlockCount;
        SetBlockImage(isLock);

        SetRewardIcon(icons);
        SetTxtRewardValue(values);
        SetTxtDayCount(data.day.ToString());

        // 리워드 사용 확인
        if (HasRewardKey())
        {
            SetBtnRewardInteractable(NotUsingReward());
        }

    }

    public bool HasRewardKey()
    {
        var hasKey = PlayerPrefs.HasKey(GetKey());
        return hasKey;
    }

    public string GetKey()
    {
        return $"{GlobalData.instance.questManager.keyNewUserEventUsedReward}_{newUserData.id}";
    }

    public bool NotUsingReward()
    {
        return PlayerPrefs.GetInt(GetKey()) == 0 ? true : false;
    }


    Sprite GetRewardIcon(string name, int value)
    {
        var icon = GlobalData.instance.spriteDataManager.GetRewardIcon(UtilityMethod.GetRewardTypeByTypeName(name), value);

        //Debug.Log($"name : {name} , icon : {icon} ");

        if (icon == null)
            icon = null;
        return icon;
    }

    void SetBtnEvents()
    {
        btnReward.onClick.AddListener(() =>
        {
            if(NotUsingReward() == false) return;
            // 보상 지급
            EventManager.instance.RunEvent<string[], int[]>(CallBackEventType.TYPES.OnUsingRewardNewUserEvent, newUserData.GetRewardTypes(), newUserData.GetRewardValues());
            PlayerPrefs.SetInt(GetKey(), 1);
            btnReward.interactable = false;
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
        btnReward.interactable = !isActive;
    }


    // btn_reward 의interactable 를 설정합니다.
    public void SetBtnRewardInteractable(bool isActive)
    {
        if(isActive)
            btnReward.image.color = enableColor;
        else
            btnReward.image.color = disableColor;
    }






}
