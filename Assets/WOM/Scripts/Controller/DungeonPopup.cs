using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;

public class DungeonPopup : MonoBehaviour
{
    public TextMeshProUGUI txtRewardAmount;
    public Button btnApply;
    public Image imageCurrencyIcon;
    public SerializableDictionary<EnumDefinition.GoodsType, Sprite> goodsToIconMap;
    public EnumDefinition.CanvasGroupTYPE canvasGroupType;

    public Action OnButtonClick;

    public GameObject particle;

    private void Awake()
    {
        btnApply.onClick.AddListener(ButtonClickEvent);

    }

    public void SetDungeonPopup(EnumDefinition.GoodsType goodsType, float reward)
    {
        ShowPopup();
        txtRewardAmount.text = UtilityMethod.ChangeSymbolNumber(reward);
        imageCurrencyIcon.sprite = goodsToIconMap[goodsType];//???? ????? ?????? ????
    }

    //??? ??? ???
    public void ButtonClickEvent()
    {
        OnButtonClick.Invoke();
        HidePopup();
    }



    public void SetBtnApplyEvent(UnityAction action)
    {
        btnApply.onClick.RemoveAllListeners();
        btnApply.onClick.AddListener(() =>
        {
            action.Invoke();

            HidePopup();
        });
    }

    
    public void ShowPopup()
    {
        GlobalData.instance.uiController.ShowFadeCanvasGroup(canvasGroupType, true);
        particle.SetActive(true);
    }
    public void HidePopup()
    {
        GlobalData.instance.uiController.ShowFadeCanvasGroup(canvasGroupType, false);
        particle.SetActive(false);
    }


}
