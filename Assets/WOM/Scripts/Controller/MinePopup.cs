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


    public TextMeshProUGUI nextProductionCountText;//생산량
    public TextMeshProUGUI nextMaxSupplyText;//최대저장량
    public TextMeshProUGUI nextProductionTimeText;//생산시간

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
    public void SetBuildingUI(CastleBuildingData data)
    {
        var nextLevelData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(data.level + 1);

        string _productionCountText = UtilityMethod.ChangeSymbolNumber(data.productionCount);
        string _maxSupplyText = UtilityMethod.ChangeSymbolNumber(data.maxSupplyAmount);
        string _productionTimeText = UtilityMethod.ChangeSymbolNumber(data.productionTime);
        string _levelText = $"Lv {data.level}";

        // 다음 레벨 정보가 존재하는 경우 문자열값 업데이트
        if (nextLevelData != null)
        {

            nextProductionCountText.text = string.Format("<color=#00FF00> ->  {0}</color>", UtilityMethod.ChangeSymbolNumber(nextLevelData.productionCount));
            nextMaxSupplyText.text = string.Format("<color=#00FF00> ->  {0}</color>", UtilityMethod.ChangeSymbolNumber(nextLevelData.maxSupplyAmount));
            nextProductionTimeText.text = string.Format("<color=#00FF00> ->  {0}</color>", UtilityMethod.ChangeSymbolNumber(nextLevelData.productionTime));
        }
        else
        {
            nextProductionCountText.text = "최대 레벨입니다";
            nextMaxSupplyText.text = "최대 레벨입니다";
            nextProductionTimeText.text = "최대 레벨입니다";
        }

        // UI에 값을 설정
        SetTextProductionCount(_productionCountText);
        SetTextMaxSupply(_maxSupplyText);
        SetTextProductionTime(_productionTimeText);
        SetTextLevel(_levelText);
        SetTextPrice(data.price);

    }


}
