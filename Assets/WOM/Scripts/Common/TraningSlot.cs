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

    public GameObject infoCommon;
    public GameObject infoMax;

    public ProjectGraphics.ClickEffect clickEffect;

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
    public void SetDescription(string value)
    {
        txtInfo.text = value;
    }

    public void SetTxtCost(float value)
    {
        if (value > 0)
        {
            var txtSymbolValue = UtilityMethod.ChangeSymbolNumber(value);
            txtCost.text = txtSymbolValue;
        }
        else
            txtCost.text = "MAX";
    }

    public void BuyEvent()
    {
        GlobalData.instance.saleManager.AddData(new SaleStatMsgData(statType));
        effLevelUp.Play();
    }

    public void SetBtnEvent()
    {
        btnBuy.onClick.AddListener(() =>
        {

            effLevelUp.Play();

        });
    }

    public void BtnEnable(bool value)
    {
        btnBuy.interactable = value;
    }

    public void MaxStat()
    {
        infoCommon.SetActive(false);
        infoMax.SetActive(true);
        clickEffect.ResetClickEffect();
    }

    public bool GetBuyButtonInteracTable()
    {
        return btnBuy.interactable;
    }



}
