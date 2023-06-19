using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class NewUserSlot : MonoBehaviour
{

    public TextMeshProUGUI txtDay;
    public TextMeshProUGUI[] txtRewardValues;
    public Image[] imgRewardIcons;
    public Image imgBlockImage;
    public Button btnReward;



    
    private void Start()
    {
        SetBtnEvents();
    }

    void SetBtnEvents()
    {
        btnReward.onClick.AddListener(() =>
        {

        });
    }

    public void SetRewardIcon(Sprite sprite) // 보상 아이콘의 스프라이트를 설정합니다.
    {
        //imgRewardIcon.sprite = sprite;
    }

    public void SetTxtRewardValue(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        //txtRewardValue.text = value;
    }

    public void SetTxtDayCount(string value) // 보상 가치의 텍스트를 설정합니다.
    {
        //txtDayCount.text = $"{value} Day";
    }

    public void SetBlockImage(bool isActive)
    {
        imgBlockImage.gameObject.SetActive(isActive);
    }


    // btn_reward 의interactable 를 설정합니다.
    public void SetBtnRewardInteractable(bool isActive)
    {
        btnReward.interactable = isActive;
    }






}
