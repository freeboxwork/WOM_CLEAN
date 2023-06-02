using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TraningSlot : MonoBehaviour
{
    //level + sheet TrainingElementData : trainingName
    public TextMeshProUGUI txtInfo;
    
    // Sheet 에서 value + unitName 
    public TextMeshProUGUI txtPower;
    
    // 심볼 단위 구성
    public TextMeshProUGUI txtCost;
         
    public Button btnBuy;

    public EnumDefinition.SaleStatType statType;
    public EnumDefinition.GoodsType goodsType;

    public TraningInGameData traningInGameData;

    public ParticleSystem effLevelUp;

    public void Start()
    {
        SetBtnEvent();
    }

    public void SetTxtInfo(string value)
    {
        txtInfo.text = value;   
    }

    public void SetTxtPower(string value)
    {
        txtPower.text = value;

        //Debug.Log(value);
    }

    public void SetTxtCost(string value)
    {
        if (value != "Max")
        {
            var txtSymbolValue = UtilityMethod.ChangeSymbolNumber(float.Parse(value));
            txtCost.text = txtSymbolValue;
        }
        else
            txtCost.text = value;
    }

    public void SetBtnEvent()
    {
        btnBuy.onClick.AddListener(() => {

            effLevelUp.Play();

        });
    }

    public void BtnEnable(bool value)
    {
        btnBuy.interactable = value;
    }







}
