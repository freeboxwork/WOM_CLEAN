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

    public Action OnButtonClick;

    private void Start()
    {

    }

    private void Awake()
    {
        btnApply.onClick.AddListener(ButtonClickEvent);

    }

    public void SetDungeonPopup(EnumDefinition.GoodsType goodsType, float reward)
    {
        
        txtRewardAmount.text = UtilityMethod.ChangeSymbolNumber(reward);
        imageCurrencyIcon.sprite = goodsToIconMap[goodsType];//???? ????? ?????? ????
    }

    //??? ??? ???
    public void ButtonClickEvent()
    {
        OnButtonClick.Invoke();
        gameObject.SetActive(false);
    }



    public void SetBtnApplyEvent(UnityAction action)
    {
        btnApply.onClick.RemoveAllListeners();
        btnApply.onClick.AddListener(() =>
        {
            action.Invoke();
            gameObject.SetActive(false);
        });
    }

}
