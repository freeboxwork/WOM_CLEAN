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
        Debug.Log((int)sfxType);
        GlobalData.instance.soundManager.PlaySfxUI(sfxType);
    }


}
