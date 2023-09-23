using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class PopUpGiveUpDungeon : MonoBehaviour
{
    //받게 될 보상량
    public TextMeshProUGUI txtRewardAmount;
        public EnumDefinition.CanvasGroupTYPE canvasGroupType;

    //수락 버튼
     public Button btnAccept;
    //거절 버튼
    public Button btnRefuse;
    //재화 아이콘
    public Image imageCurrencyIcon;
    public SerializableDictionary<EnumDefinition.GoodsType, Sprite> goodsToIconMap;

    Action<EnumDefinition.GoodsType, float> callBack;
    EnumDefinition.GoodsType goodsType;
    float reward;

    public bool isShowPopup = false;
    private void Awake()
    {
        btnAccept.onClick.AddListener(ButtonAccept);
        btnRefuse.onClick.AddListener(ButtonRefuse);
    }

    public void ShowGiveUpDungeonPopup(EnumDefinition.GoodsType goodsType, float reward, Action<EnumDefinition.GoodsType, float> callBack)
    {
        isShowPopup = true;
        this.callBack = callBack;
        this.goodsType = goodsType;
        this.reward = reward;
        ShowPopup();
        txtRewardAmount.text = UtilityMethod.ChangeSymbolNumber(this.reward);
        imageCurrencyIcon.sprite = goodsToIconMap[this.goodsType];
    }


    void ButtonAccept()
    {
        isShowPopup = false;
        this.callBack.Invoke(this.goodsType, Mathf.Round(this.reward));
        this.callBack = null;
        HidePopup();
    }
    public void ButtonRefuse()
    {
        isShowPopup = false;
        this.callBack = null;
        HidePopup();
    }



    public void ShowPopup()
    {
        GlobalData.instance.uiController.ShowFadeCanvasGroup(canvasGroupType, true);
    }
    public void HidePopup()
    {
        GlobalData.instance.uiController.ShowFadeCanvasGroup(canvasGroupType, false);
    }

}
