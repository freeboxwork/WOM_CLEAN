using UnityEngine;
using UnityEngine.UI;

public class UiButtonClickSfx : MonoBehaviour
{

    public EnumDefinition.SFX_TYPE sfxType;
    public Button btn;

    void Start()
    {
        if (TryGetComponent<Button>(out btn))
        {
            btn.onClick.AddListener(OnClick);
        }
    }

    public void OnClick()
    {
        if (sfxType == EnumDefinition.SFX_TYPE.Upgrade)
        {
            GlobalData.instance.soundManager.PlayUpgradeSound();

        }
        else
        {
            GlobalData.instance.soundManager.PlaySfxUI(sfxType);

        }
    }


}
