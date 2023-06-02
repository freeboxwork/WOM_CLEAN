using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FactoryPopup : CastlePopupBase
{
   
    public TextMeshProUGUI productionCountText;//생산량
    public TextMeshProUGUI maxSupplyText;//최대저장량
    public TextMeshProUGUI productionTimeText;//생산시간
    public TextMeshProUGUI levelText;//레벨
    public TextMeshProUGUI priceText;//업그레이드 석탄 비용
    public TextMeshProUGUI totlaMiningValue; //총 채굴량

    public Button btnGetGold;
    public Button btnUpgrade;

    //현재 클래스 TextMeshProUGUI 타입의 맴버변수의 개별 text 값을 각각 설정하는 개별 함수
    public void SetTextTotalMiningValue(string text)
    {
        totlaMiningValue.text = text;
    }
    public void SetTextProductionCount(string text)
    {
        productionCountText.text = text;
    }
    public void SetTextMaxSupply(string text)
    {
        maxSupplyText.text = text;
    }
    public void SetTextProductionTime(string text)
    {
        productionTimeText.text = text;
    }
    public void SetTextLevel(string text)
    {
        levelText.text = text;
    }
    public void SetTextPrice(string text)
    {
        priceText.text = text;
    }

   //CastleBuildingData 객체를 인자로 받아서 각각의 맴버변수의 text 값을 설정하는 함수
        public void SetUpGradeText(CastleBuildingData data, CastleBuildingData nextLevelData = null)
    {
        // 생산량, 최대 저장량, 생산 시간 계산
        var productionCount = (nextLevelData != null) ? data.productionCount - nextLevelData.productionCount : 0;
        var maxSupply = (nextLevelData != null) ? data.maxSupplyAmount - nextLevelData.maxSupplyAmount : 0;
        var productionTime = (nextLevelData != null) ? nextLevelData.productionTime - data.productionTime : 0;

        // 문자열 초기값 설정
        string _productionCountText = $"{data.productionCount}";
        string _maxSupplyText = $"{data.maxSupplyAmount}";
        string _productionTimeText = $"{data.productionTime}";
        string _levelText = $"{data.level}";
        string _priceText = data.price.ToString();

        // 다음 레벨 정보가 존재하는 경우 문자열값 업데이트
        if (nextLevelData != null)
        {
            _productionCountText += $" / {productionCount}";
            _maxSupplyText += $" / {maxSupply}";
            _productionTimeText += $" / {productionTime}";
            _levelText += $" > {nextLevelData.level}";
            _priceText = nextLevelData.price.ToString();
        }

        // UI에 값을 설정
        SetTextProductionCount(_productionCountText);
        SetTextMaxSupply(_maxSupplyText);
        SetTextProductionTime(_productionTimeText);
        SetTextLevel(_levelText);
        SetTextPrice(_priceText);
    }

}
