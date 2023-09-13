using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BattlePassSlot : MonoBehaviour
{

    public BattlePassData battlePassData;

    public Image imgRewardIcon;
    public TextMeshProUGUI txtRewardValue;
    public TextMeshProUGUI txtStage;
    public Image blockImage;
    public Image blockPassImage;
    public Image imgPassRewardIcon;
    public TextMeshProUGUI txtPassRewardValue;

    public GameObject blockCurrent;
    public GameObject blockPass;


    public Button btnReward;
    public Button btnPassReward;


    private void Start()
    {
        SetBtnEvents();
    }

    void SetBtnEvents()
    {
        btnReward.onClick.AddListener(() =>
        {
            EventManager.instance.RunEvent<string, int>(CallBackEventType.TYPES.OnQuestCompleteBattlePassStage, battlePassData.commonRewardType, battlePassData.commonRewardCount);
            var saveKey = $"{GlobalData.instance.questManager.keyBattlePassUsedReward}_{battlePassData.targetStage}";
            PlayerPrefs.SetInt(saveKey, 1);
            SetBtnRewardInteractable(false);
        });

        btnPassReward.onClick.AddListener(() =>
        {
            EventManager.instance.RunEvent<string, int>(CallBackEventType.TYPES.OnQuestCompleteBattlePassStageBuyitem, battlePassData.passRewardType, battlePassData.passRewardCount);
            var saveKey = $"{GlobalData.instance.questManager.keyBuyBattlePass}_{battlePassData.targetStage}";
            PlayerPrefs.SetInt(saveKey, 1);
            SetBtnPassRewardInteractable(false);
        });
    }

    public void SetRewardIcon(Sprite sprite) // 보상 아이콘의 스프라이트를 설정합니다.
    {
        imgRewardIcon.sprite = sprite;
    }

    public void SetPassRewardIcon(Sprite sprite) // 보상 아이콘의 스프라이트를 설정합니다.
    {
        imgPassRewardIcon.sprite = sprite;
    }

    public void SetTxtRewardValue(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        txtRewardValue.text = value;
    }

    public void SetTxtPassRewardValue(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        txtPassRewardValue.text = value;
    }

    public void SetTxtStage(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        txtStage.text = value;
    }

    public void SetBlockImage(bool isBlock)
    {
        blockImage.gameObject.SetActive(isBlock);
    }
    public bool IsActiveHideBlockImage()
    {
        return blockImage.gameObject.activeSelf;
    }
    public void SetBlockPassImage(bool isBlock)
    {
        blockPassImage.gameObject.SetActive(isBlock);
    }

    public void SetBtnRewardInteractable(bool isActive)
    {
        btnReward.interactable = isActive;
        ShowBlockCurrentImage(!isActive);

    }

    public void SetBtnPassRewardInteractable(bool isActive)
    {
        btnPassReward.interactable = isActive;
        ShowBlockPassImage(!isActive);

    }

    void ShowBlockCurrentImage(bool show)
    {
        blockCurrent.SetActive(show);
    }
    void ShowBlockPassImage(bool show)
    {
        blockPass.SetActive(show);
    }

}
