using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 재화에 따른 버튼 Interactable 체크
/// </summary>
public class ButtonInteractableCheck : MonoBehaviour
{
    public EnumDefinition.RewardType type;

    public int enableCount;
    public Button btn;

    [SerializeField] Sprite enableSprite;
    [SerializeField] Sprite disableSprite;

    void Awake()
    {
        btn = GetComponent<Button>();
    }

    public void InteractableCheck()
    {
        if (btn != null)
        {
            if(IsEnable())
            {
                btn.image.sprite = enableSprite;
            }
            else
            {
                btn.image.sprite = disableSprite;
            }
        }
    }

    bool IsEnable()
    {
        var checkNull = GlobalData.instance.player;

        if (checkNull != null)
        {
            if(enableCount <= GlobalData.instance.player.GetGoodsByRewardType(type))
            {
                return true;
            }
        }

        return false;

    }

}
