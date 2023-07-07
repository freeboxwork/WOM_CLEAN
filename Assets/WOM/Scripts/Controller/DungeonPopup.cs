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
    //public ParticleImage particleImage;

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
        //particleImage.onStop.AddListener(EndParticle);

    }

    public void SetDungeonPopup(EnumDefinition.GoodsType goodsType,int reward)
    {
        txtRewardAmount.text = string.Format("{0}", reward);//��ȭ ����
        imageCurrencyIcon.sprite = goodsToIconMap[goodsType];//���� ��ȭ�� ������ ����
        //particleImage.texture = goodsToTextureMap[goodsType];
    }

    //Ȯ�� ��ư Ŭ��
    public void ButtonClickEvent()
    {
        //��ƼŬ�� ���� ������̶�� ��ư Ŭ���� ���� ����
        //if (particleImage.isPlaying) return;
        OnButtonClick.Invoke();
        gameObject.SetActive(false);
    }
    
    //��ƼŬ�� ������ �� ��ȭ UI Text & Data ������Ʈ
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
