using UnityEngine;
using UnityEngine.UI;

public class CastlePopupBase : MonoBehaviour
{
    public Button closeButton;
    public EnumDefinition.CastlePopupType popupType;
    public EnumDefinition.CanvasGroupTYPE canvasGroupType;

    protected virtual void Awake()
    {
               closeButton.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }


    public virtual void ShowPopup()
    {
        GlobalData.instance.uiController.ShowFadeCanvasGroup(canvasGroupType);
    }
    public virtual void HidePopup()
    {
        GlobalData.instance.uiController.HideFadeCanvasGroup(canvasGroupType);
    }

}
