using UnityEngine.UI;
using TMPro;
using System.Diagnostics;
using System.Collections;
using UnityEngine;
using System;

public class MinePopup : CastlePopupBase
{
    public TextMeshProUGUI productionCountText;//생산량
    public TextMeshProUGUI maxSupplyText;//최대저장량
    public TextMeshProUGUI productionTimeText;//생산시간
    public TextMeshProUGUI levelText;//레벨
    public TextMeshProUGUI priceText;//업그레이드 석탄 비용
    public TextMeshProUGUI totlaMiningValue; //총 채굴량
    public TextMeshProUGUI digUpTimeText; //실시간 채굴시간
    public TextMeshProUGUI digUpStateText; //채굴 현황
    
    public Image digUpFillImage;//채굴 상태 FillAmount
    public Button btnGetGold;
    public Button btnUpgrade;

    public CanvasGroup canvasGroup;

    //현재 클래스 TextMeshProUGUI 타입의 맴버변수의 개별 text 값을 각각 설정하는 개별 함수
    

    //실시간 채굴 시간
    public void SetTextDigUpTimeValue(float max, float current)
    {
        digUpTimeText.text = string.Format("{0}/{1}", Math.Floor(current),Math.Floor(max));
        SetDigUpStateFillAmount(max, current);
    }
    //채굴 현황(채굴중...)
    public void SetTextDigUpFullText(string text)
    {
        digUpStateText.text = text;
    }
    //생산량
    public void SetTextProductionCount(string text)
    {
        productionCountText.text = text;
    }
    //최대 저장량
    public void SetTextMaxSupply(string text)
    {
        maxSupplyText.text =  text;
    }
    //생산 시간
    public void SetTextProductionTime(string text)
    {
        productionTimeText.text = text;
    }
    //레벨
    public void SetTextLevel(string text)
    {
        levelText.text = text;
    }
    //업그레이드 석탄 비용
    public void SetTextPrice(float text)
    {
        if(text < 0) 
        {
            priceText.text =  "MAX";
            return;
        }
        priceText.text =  UtilityMethod.ChangeSymbolNumber(text);
    }
    //채굴 상태 FillAmount
    void SetDigUpStateFillAmount(float max, float currnt)
    {
        digUpFillImage.fillAmount = (float)currnt/max;
    }
    //실시간으로 생산된 총 량
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
    //CastleBuildingData 객체를 인자로 받아서 각각의 맴버변수의 text 값을 설정하는 함수
    /*
    public void SetUpGradeText(CastleBuildingData data, CastleBuildingData nextLevelData =null)
    {

        // 생산량 = 현재 레벨의 생산량 - 다음 레벨의 생산량
        var productionCount = data.productionCount - nextLevelData.productionCount;
        // 최대 저장량 = 현재 레벨의 저장량 - 다음 레벨의 저장량
        var maxSupply = data.maxSupplyAmount - nextLevelData.maxSupplyAmount;
        // 생산시간 = 다음 레벨의 생산시간 - 현재 레벨의 생산시간
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

    //CastleBuildingData 객체를 인자로 받아서 각각의 맴버변수의 text 값을 설정하는 함수
    public void SetTextLevelData(CastleBuildingData data, CastleBuildingData nextLevelData = null)
    {
        // 생산량, 최대 저장량, 생산 시간 계산
        // var productionCount = (nextLevelData != null) ? data.productionCount - nextLevelData.productionCount : 0;
        // var maxSupply = (nextLevelData != null) ? data.maxSupplyAmount - nextLevelData.maxSupplyAmount : 0;
        // var productionTime = (nextLevelData != null) ? nextLevelData.productionTime - data.productionTime : 0;

        // 생산량, 최대 저장량, 생산 시간 계산
        var productionCount = data.productionCount;
        var maxSupply = data.maxSupplyAmount;
        var productionTime = data.productionTime;

        // 문자열 초기값 설정
        // string _productionCountText = $"{data.productionCount}";
        // string _maxSupplyText = $"{data.maxSupplyAmount}";
        // string _productionTimeText = $"{data.productionTime}";

        string _productionCountText = UtilityMethod.ChangeSymbolNumber(data.productionCount);
        string _maxSupplyText = UtilityMethod.ChangeSymbolNumber(data.maxSupplyAmount);
        string _productionTimeText = UtilityMethod.ChangeSymbolNumber(data.productionTime);

        string _levelText = $"Lv {data.level}";



        // 다음 레벨 정보가 존재하는 경우 문자열값 업데이트
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

        // UI에 값을 설정
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




    //CastleBuildingData 객체를 인자로 받아서 각각의 맴버변수의 text 값을 설정하는 함수
    public void InitUIText(CastleBuildingData data)
    {
        // 생산량, 최대 저장량, 생산 시간 계산
        var productionCount = data.productionCount;
        var maxSupply = data.maxSupplyAmount;
        var productionTime = data.productionTime;

        // 문자열 초기값 설정
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

        // UI에 값을 설정
        SetTextProductionCount(_productionCountText);
        SetTextMaxSupply(_maxSupplyText);
        SetTextProductionTime(_productionTimeText);
        SetTextLevel(_levelText);
        SetTextPrice(data.price);

        //TOD0: 저장된 데이터에서 불러 와야 함
        // data.TotlaMiningValue = 0;

        //data 의 모든 변수값 출력
        // Debug.Log($"data.productionCount : {data.productionCount}");
        // Debug.Log($"data.maxSupplyAmount : {data.maxSupplyAmount}");
        // Debug.Log($"data.productionTime : {data.productionTime}");
        // Debug.Log($"data.level : {data.level}");
        // Debug.Log($"data.price : {data.price}");


    }


}
