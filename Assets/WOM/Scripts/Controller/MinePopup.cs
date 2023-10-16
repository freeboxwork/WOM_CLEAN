using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.Collections;
using UnityEngine;
using System;

public class MinePopup : CastlePopupBase
{
    public TextMeshProUGUI productionCountText;//���귮
    public TextMeshProUGUI maxSupplyText;//�ִ����差
    public TextMeshProUGUI productionTimeText;//����ð�


    public TextMeshProUGUI nextProductionCountText;//���귮
    public TextMeshProUGUI nextMaxSupplyText;//�ִ����差
    public TextMeshProUGUI nextProductionTimeText;//����ð�

    public TextMeshProUGUI levelText;//����
    public TextMeshProUGUI priceText;//���׷��̵� ��ź ���
    public TextMeshProUGUI totlaMiningValue; //�� ä����
    public TextMeshProUGUI digUpTimeText; //�ǽð� ä���ð�
    public TextMeshProUGUI digUpStateText; //ä�� ��Ȳ

    public Slider digUpSlider;
    //public Image digUpFillImage;//ä�� ���� FillAmount
    public Button btnGetGold;
    public Button btnUpgrade;

    //���� Ŭ���� TextMeshProUGUI Ÿ���� �ɹ������� ���� text ���� ���� �����ϴ� ���� �Լ�


    protected override void Awake() {
        base.Awake();
    }

    void Start() 
    {
        SetButtonEvents();
    }

    public void SetGoodsButton(bool isGet)
    {
        btnGetGold.interactable = isGet;
    }

    void SetButtonEvents()
    {
        btnGetGold.onClick.AddListener(() =>
        {
            switch (popupType)
            {
                case EnumDefinition.CastlePopupType.mine:
                    GlobalData.instance.castleManager.WithdrawGold();
                    break;
                case EnumDefinition.CastlePopupType.factory:
                    GlobalData.instance.castleManager.WithdrawBone();
                    break;
            }
        });

        btnUpgrade.onClick.AddListener(ClockUpgradeButton);

        closeButton.onClick.AddListener(() =>
        {
            HidePopup();
        });
    }
    public override void ShowPopup()
    {
        base.ShowPopup();
    }
    public override void HidePopup()
    {
        base.HidePopup();
    }

    void ClockUpgradeButton()
    {
        GlobalData.instance.castleManager.UpGradeCastle(popupType);
    }
    //�ǽð� ä�� �ð�
    public void SetTextDigUpTimeValue(float max, float current)
    {
        digUpTimeText.text = string.Format("{0}/{1}", Math.Floor(current),Math.Floor(max));
        SetDigUpStateFillAmount(max, current);
    }
    //ä�� ��Ȳ(ä����...)
    public void SetTextDigUpFullText(string text)
    {
        digUpStateText.text = text;
    }
    //���귮
    public void SetTextProductionCount(string text)
    {
        productionCountText.text = text;
    }
    //�ִ� ���差
    public void SetTextMaxSupply(string text)
    {
        maxSupplyText.text =  text;
    }
    //���� �ð�
    public void SetTextProductionTime(string text)
    {
        productionTimeText.text = text;
    }
    //����
    public void SetTextLevel(string text)
    {
        levelText.text = text;
    }
    //���׷��̵� ��ź ���
    public void SetTextPrice(float text)
    {
        if(text < 0) 
        {
            priceText.text =  "MAX";
            return;
        }
        priceText.text =  UtilityMethod.ChangeSymbolNumber(text);
    }


    //ä�� ���� FillAmount
    void SetDigUpStateFillAmount(float max, float currnt)
    {
        digUpSlider.maxValue = (float)currnt/max;;
        //digUpFillImage.fillAmount = (float)currnt/max;
    }
    //�ǽð����� ����� �� ��
    public void SetTextTotalMiningValue(float text)
    {
        totlaMiningValue.text = UtilityMethod.ChangeSymbolNumber(text);
    }


    public void SetMaxUI()
    {
        btnUpgrade.interactable = false;
        SetTextPrice(-1);
    }
    
    //CastleBuildingData ��ü�� ���ڷ� �޾Ƽ� ������ �ɹ������� text ���� �����ϴ� �Լ�
    public void SetBuildingUI(CastleBuildingData data)
    {
        MineAndFactoryBuildingData nextLevelData;

        if(data.goodsType == EnumDefinition.GoodsType.gold)
        {
            nextLevelData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(data.level + 1);
        }
        else
        {
            nextLevelData = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(data.level + 1);
        }

        string _productionCountText = UtilityMethod.ChangeSymbolNumber(data.productionCount);
        string _maxSupplyText = UtilityMethod.ChangeSymbolNumber(data.maxSupplyAmount);
        string _productionTimeText = UtilityMethod.ChangeSymbolNumber(data.productionTime);
        string _levelText = $"Lv {data.level}";

        // ���� ���� ������ �����ϴ� ��� ���ڿ��� ������Ʈ
        if (nextLevelData != null)
        {
            
            if(nextLevelData.productionCount > data.productionCount)
            {
                nextProductionCountText.text = string.Format("<color=#00FF00>{0}</color>", UtilityMethod.ChangeSymbolNumber(nextLevelData.productionCount));
            }
            else
            {
                nextProductionCountText.text = string.Format("{0}", UtilityMethod.ChangeSymbolNumber(nextLevelData.productionCount));    
            }

            if(nextLevelData.maxSupplyAmount > data.maxSupplyAmount)
            {
                nextMaxSupplyText.text = string.Format("<color=#00FF00>{0}</color>", UtilityMethod.ChangeSymbolNumber(nextLevelData.maxSupplyAmount));
            }
            else
            {
                nextMaxSupplyText.text = string.Format("{0}", UtilityMethod.ChangeSymbolNumber(nextLevelData.maxSupplyAmount));    
            }

            if(nextLevelData.productionTime < data.productionTime)
            {
                nextProductionTimeText.text = string.Format("<color=#00FF00>{0}</color>", UtilityMethod.ChangeSymbolNumber(nextLevelData.productionTime));
            }
            else
            {
                nextProductionTimeText.text = string.Format("{0}", UtilityMethod.ChangeSymbolNumber(nextLevelData.productionTime));    
            }

            SetTextPrice(nextLevelData.price);
        }
        else
        {
            nextProductionCountText.text = "Max";
            nextMaxSupplyText.text = "Max";
            nextProductionTimeText.text = "Max";
            SetMaxUI();
        }

        // UI�� ���� ����
        SetTextProductionCount(_productionCountText);
        SetTextMaxSupply(_maxSupplyText);
        SetTextProductionTime(_productionTimeText);
        SetTextLevel(_levelText);

    }


}
