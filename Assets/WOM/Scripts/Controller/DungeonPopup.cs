using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using TMPro;
using Coffee.UIExtensions;
using AssetKits.ParticleImage;

public class DungeonPopup : MonoBehaviour
{
    public TextMeshProUGUI txtRewardAmount;
    public Button btnApply;
    public Image imageCurrencyIcon;
    public ParticleImage particleImage;

    public SerializableDictionary<EnumDefinition.GoodsType, Sprite> goodsToIconMap;
    public SerializableDictionary<EnumDefinition.GoodsType, Texture> goodsToTextureMap;
    public event Action OnFinishParticle;
    public event Action OnButtonClick;


    private void Start()
    {
        
    }

    private void Awake()
    {
        btnApply.onClick.AddListener(ButtonClickEvent);
        particleImage.onStop.AddListener(EndParticle);

    }

    public void SetDungeonPopup(EnumDefinition.GoodsType goodsType,int reward)
    {
        txtRewardAmount.text = string.Format("{0}", reward);//재화 보상량
        imageCurrencyIcon.sprite = goodsToIconMap[goodsType];//보상 재화의 아이콘 종류
        particleImage.texture = goodsToTextureMap[goodsType];
    }

    //확인 버튼 클릭
    public void ButtonClickEvent()
    {
        //파티클이 아직 재생중이라면 버튼 클릭이 되지 않음
        if (particleImage.isPlaying) return;
        OnButtonClick.Invoke();
        gameObject.SetActive(false);
    }
    
    //파티클이 종류된 후 재화 UI Text & Data 업데이트
    public void EndParticle()
    {
        OnFinishParticle.Invoke();
    }


    public void SetBtnApplyEvent(UnityAction action)
    {
        btnApply.onClick.RemoveAllListeners();
        btnApply.onClick.AddListener(() => {
            action.Invoke();
            gameObject.SetActive(false);
        });
    }

}
