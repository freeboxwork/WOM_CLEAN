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
    public TextMeshProUGUI levelText;//����
    public TextMeshProUGUI priceText;//���׷��̵� ��ź ���
    public TextMeshProUGUI totlaMiningValue; //�� ä����
    public TextMeshProUGUI digUpTimeText; //�ǽð� ä���ð�
    public TextMeshProUGUI digUpStateText; //ä�� ��Ȳ
    
    public Image digUpFillImage;//ä�� ���� FillAmount
    public Button btnGetGold;
    public Button btnUpgrade;

    public CanvasGroup canvasGroup;

    //���� Ŭ���� TextMeshProUGUI Ÿ���� �ɹ������� ���� text ���� ���� �����ϴ� ���� �Լ�
    

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
        digUpFillImage.fillAmount = (float)currnt/max;
    }
    //�ǽð����� ����� �� ��
    public void SetTextTotalMiningValue(float text)
    {
        totlaMiningValue.text = UtilityMethod.ChangeSymbolNumber(text);
    }
    public void SetShowCanvasGroup(bool isShow)
    {
        canvasGroup.alpha = isShow ? 1 : 0;
        canvasGroup.blocksRaycasts = isShow;
    }


    private void Start()
    {
        SetButtonEvents();
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

        btnUpgrade.onClick.AddListener(() =>
        {
            GlobalData.instance.castleManager.UpGradeCastle(popupType);
        });
    }

    public void SetMaxUI()
    {
        btnUpgrade.interactable = false;
        SetTextPrice(-1);
    }
    //CastleBuildingData ��ü�� ���ڷ� �޾Ƽ� ������ �ɹ������� text ���� �����ϴ� �Լ�
    /*
    public void SetUpGradeText(CastleBuildingData data, CastleBuildingData nextLevelData =null)
    {

        // ���귮 = ���� ������ ���귮 - ���� ������ ���귮
        var productionCount = data.productionCount - nextLevelData.productionCount;
        // �ִ� ���差 = ���� ������ ���差 - ���� ������ ���差
        var maxSupply = data.maxSupplyAmount - nextLevelData.maxSupplyAmount;
        // ����ð� = ���� ������ ����ð� - ���� ������ ����ð�
        var productionTime = nextLevelData.productionTime - data.productionTime;

        string _productionCountText = string.Empty;
        string _maxSupplyText = string.Empty; ;
        string _productionTimeText = string.Empty; ;
        string _levelText = string.Empty; 
        string _priceText = string.Empty; 
      
        if(nextLevelData != null)
        {
           _productionCountText = $"{data.productionCount} / {productionCount}";
           _maxSupplyText = $"{data.maxSupplyAmount} / {maxSupply}";
           _productionTimeText = $"{data.productionTime} / {productionTime}";
           _levelText = $"{data.level} > {nextLevelData.level}";
           _priceText = nextLevelData.price.ToString();
        }
        else
        {
            _productionCountText = $"{data.productionCount}";
            _maxSupplyText = $"{data.maxSupplyAmount}";
            _productionTimeText = $"{data.productionTime}";
            _levelText = $"{data.level}";
            _priceText = data.price.ToString(); 
        }

        SetTextProductionCount(_productionCountText);
        SetTextMaxSupply(_maxSupplyText);
        SetTextProductionTime(_productionTimeText);
        SetTextLevel(_levelText);
        SetTextPrice(data.price.ToString());
       
    }
    */

    //CastleBuildingData ��ü�� ���ڷ� �޾Ƽ� ������ �ɹ������� text ���� �����ϴ� �Լ�
    public void SetTextLevelData(CastleBuildingData data, CastleBuildingData nextLevelData = null)
    {
        // ���귮, �ִ� ���差, ���� �ð� ���
        // var productionCount = (nextLevelData != null) ? data.productionCount - nextLevelData.productionCount : 0;
        // var maxSupply = (nextLevelData != null) ? data.maxSupplyAmount - nextLevelData.maxSupplyAmount : 0;
        // var productionTime = (nextLevelData != null) ? nextLevelData.productionTime - data.productionTime : 0;

        // ���귮, �ִ� ���差, ���� �ð� ���
        var productionCount = data.productionCount;
        var maxSupply = data.maxSupplyAmount;
        var productionTime = data.productionTime;

        // ���ڿ� �ʱⰪ ����
        // string _productionCountText = $"{data.productionCount}";
        // string _maxSupplyText = $"{data.maxSupplyAmount}";
        // string _productionTimeText = $"{data.productionTime}";

        string _productionCountText = UtilityMethod.ChangeSymbolNumber(data.productionCount);
        string _maxSupplyText = UtilityMethod.ChangeSymbolNumber(data.maxSupplyAmount);
        string _productionTimeText = UtilityMethod.ChangeSymbolNumber(data.productionTime);

        string _levelText = $"Lv {data.level}";



        // ���� ���� ������ �����ϴ� ��� ���ڿ��� ������Ʈ
        if (nextLevelData != null)
        {
            var nextProduct = nextLevelData.productionCount - productionCount;
            var nextSupply = nextLevelData.maxSupplyAmount - maxSupply;
            var nextTime = productionTime - nextLevelData.productionTime;

            // _productionCountText = string.Format("{0}", data.productionCount);
            // _maxSupplyText = string.Format("{0}", data.maxSupplyAmount);
            // _productionTimeText = string.Format("{0}", data.productionTime);

            _productionCountText = UtilityMethod.ChangeSymbolNumber(data.productionCount);
            _maxSupplyText = UtilityMethod.ChangeSymbolNumber(data.maxSupplyAmount);
            _productionTimeText = UtilityMethod.ChangeSymbolNumber(data.productionTime);



            if (!CheckEqualZeroByCalculation(nextProduct))
            {
                _productionCountText += string.Format("<color=#00FF00> + {0}</color>", UtilityMethod.ChangeSymbolNumber(nextProduct));
            }
            if (!CheckEqualZeroByCalculation(nextSupply))
            {
                _maxSupplyText += string.Format("<color=#00FF00> + {0}</color>", UtilityMethod.ChangeSymbolNumber(nextSupply));
            }
            if (!CheckEqualZeroByCalculation(nextTime))
            {
                _productionTimeText += string.Format("<color=#00FF00> - {0}</color>", UtilityMethod.ChangeSymbolNumber(nextTime));
            }


            //            _levelText += $" > Lv {nextLevelData.level}";
            //            _priceText = nextLevelData.price.ToString();
        }

        // UI�� ���� ����
        SetTextProductionCount(_productionCountText);
        SetTextMaxSupply(_maxSupplyText);
        SetTextProductionTime(_productionTimeText);
        SetTextLevel(_levelText);
        SetTextPrice(data.price);

    }

    bool CheckEqualZeroByCalculation(float calc)
    {
        if (calc > 0) return false;
        return true;
    }




    //CastleBuildingData ��ü�� ���ڷ� �޾Ƽ� ������ �ɹ������� text ���� �����ϴ� �Լ�
    public void InitUIText(CastleBuildingData data)
    {
        // ���귮, �ִ� ���差, ���� �ð� ���
        var productionCount = data.productionCount;
        var maxSupply = data.maxSupplyAmount;
        var productionTime = data.productionTime;

        // ���ڿ� �ʱⰪ ����
        // string _productionCountText = $"{data.productionCount}";
        // string _maxSupplyText = $"{data.maxSupplyAmount}";
        // string _productionTimeText = $"{data.productionTime}";
        string _productionCountText = UtilityMethod.ChangeSymbolNumber(data.productionCount);
        //UnityEngine.Debug.Log(data.maxSupplyAmount);
        string _maxSupplyText = UtilityMethod.ChangeSymbolNumber(data.maxSupplyAmount);
        string _productionTimeText = UtilityMethod.ChangeSymbolNumber(data.productionTime);
        string _levelText = $"Lv {data.level}";


        var nextLevelData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(data.level + 1);
        CastleBuildingData nextBuildData = null;
        if (nextLevelData != null)
        {
            nextBuildData = new CastleBuildingData().Create().SetGoodsType(data.goodsType).Clone(nextLevelData);

            var nextProduct = nextLevelData.productionCount - productionCount;
            var nextSupply = nextLevelData.maxSupplyAmount - maxSupply;
            var nextTime = productionTime - nextLevelData.productionTime;

            // _productionCountText = string.Format("{0}", data.productionCount);
            // _maxSupplyText = string.Format("{0}", data.maxSupplyAmount);
            // _productionTimeText = string.Format("{0}", data.productionTime);

            _productionCountText = UtilityMethod.ChangeSymbolNumber(data.productionCount);
            _maxSupplyText = UtilityMethod.ChangeSymbolNumber(data.maxSupplyAmount);
            _productionTimeText = UtilityMethod.ChangeSymbolNumber(data.productionTime);



            if (!CheckEqualZeroByCalculation(nextProduct))
            {
                _productionCountText += string.Format("<color=#00FF00> + {0}</color>", UtilityMethod.ChangeSymbolNumber(nextProduct));
            }
            if (!CheckEqualZeroByCalculation(nextSupply))
            {
                _maxSupplyText += string.Format("<color=#00FF00> + {0}</color>", UtilityMethod.ChangeSymbolNumber(nextSupply));
            }
            if (!CheckEqualZeroByCalculation(nextTime))
            {
                _productionTimeText += string.Format("<color=#00FF00> - {0}</color>", UtilityMethod.ChangeSymbolNumber(nextTime));
            }

        }

        // UI�� ���� ����
        SetTextProductionCount(_productionCountText);
        SetTextMaxSupply(_maxSupplyText);
        SetTextProductionTime(_productionTimeText);
        SetTextLevel(_levelText);
        SetTextPrice(data.price);

        //TOD0: ����� �����Ϳ��� �ҷ� �;� ��
        // data.TotlaMiningValue = 0;

        //data �� ��� ������ ���
        // Debug.Log($"data.productionCount : {data.productionCount}");
        // Debug.Log($"data.maxSupplyAmount : {data.maxSupplyAmount}");
        // Debug.Log($"data.productionTime : {data.productionTime}");
        // Debug.Log($"data.level : {data.level}");
        // Debug.Log($"data.price : {data.price}");


    }


}
