using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class BattlePassSlot : MonoBehaviour
{
    public Image imgRewardIcon;
    public TextMeshProUGUI txtRewardValue;
    public TextMeshProUGUI txtStage;
    public Image blockImage;
    public Image blockPassImage;
    public Image imgPassRewardIcon;
    public TextMeshProUGUI txtPassRewardValue;


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

    public void SetBlockPassImage(bool isBlock)
    {
        blockPassImage.gameObject.SetActive(isBlock);
    }


}
