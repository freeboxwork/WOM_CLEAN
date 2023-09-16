using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;


public class PopUpGiveUpDungeon : MonoBehaviour
{
    //받게 될 보상량
    public TextMeshProUGUI txtRewardAmount;
    
    //수락 버튼
     public Button btnAccept;
    //거절 버튼
    public Button btnRefuse;
    //재화 아이콘
    public Image imageCurrencyIcon;
    public SerializableDictionary<EnumDefinition.GoodsType, Sprite> goodsToIconMap;

    public GameObject content;

    Action<EnumDefinition.GoodsType, long> callBack;
    EnumDefinition.GoodsType goodsType;
    long reward;

    public bool isShowPopup = false;
    private void Awake()
    {
        btnAccept.onClick.AddListener(ButtonAccept);
        btnRefuse.onClick.AddListener(ButtonRefuse);
    }

    public void ShowGiveUpDungeonPopup(EnumDefinition.GoodsType goodsType, long reward, Action<EnumDefinition.GoodsType, long> callBack)
    {
        isShowPopup = true;
        this.callBack = callBack;
        this.goodsType = goodsType;
        this.reward = reward;
        content.SetActive(true);
        txtRewardAmount.text = UtilityMethod.ChangeSymbolNumber(this.reward.ToString());
        imageCurrencyIcon.sprite = goodsToIconMap[this.goodsType];
    }


    void ButtonAccept()
    {
        isShowPopup = false;
        this.callBack.Invoke(this.goodsType, this.reward);
        this.callBack = null;
        content.SetActive(false);
    }
    public void ButtonRefuse()
    {
        isShowPopup = false;
        this.callBack = null;
        content.SetActive(false);
    }

}
