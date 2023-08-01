using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using static EnumDefinition;
using ProjectGraphics;
using static ProjectGraphics.CastleController;

public class CastleManager : MonoBehaviour
{
    public List<CastlePopupBase> castlePopupList = new List<CastlePopupBase>();

    public CastleBuildingData buildDataMine;
    public CastleBuildingData buildDataFactory;

    public CastleController castleController;

    public int mineLevel = 0;
    public int factoryLevel = 0;


    void Start()
    {
        SetBtnEvents();
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator Init()
    {
        // GET SAVE DATA
        SetCastleData();

        // 골드 채굴 시작
        StartCoroutine(MiningGold());
        // 뼈조각 채굴 시작 
        StartCoroutine(MiningBone());

        yield return null;
    }


    void SetCastleData()
    {
        //TODO: 저장된 데이터에서 불러와야 함.
        mineLevel = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.mineLevel;
        factoryLevel = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.factoryLevel;

        var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);
        var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);

        buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);
        buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);

        // 초기 UI 설정 ( POPUP )
        var minePopup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);
        minePopup.InitUIText(buildDataMine);

        var factoryPopup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);
        factoryPopup.InitUIText(buildDataFactory);

        // 초기 UI 설정 ( CASTLE )
        castleController.SetMineBuild(mineLevel);
        castleController.SetFactoryBuild(factoryLevel);

        // 건설하기 버튼 UI Disable
        if (mineLevel > 0)
        {
            UtilityMethod.GetCustomTypeBtnByID(64).gameObject.SetActive(false);
            UtilityMethod.GetCustomTypeBtnByID(51).interactable = true;
        }
        if (factoryLevel > 0)
        {
            UtilityMethod.GetCustomTypeBtnByID(65).gameObject.SetActive(false);
            UtilityMethod.GetCustomTypeBtnByID(52).interactable = true;
        }
    }


    void SetBtnEvents()
    {
        // 건설하기 버튼 ( 금광 , 가공소 )
        // 64, 65
        UtilityMethod.SetBtnEventCustomTypeByID(64, () =>
        {
            UpGradeCastle(CastlePopupType.mine);
            var isUpgrade = mineLevel > 0;

            //Debug.Log( isUpgrade + " mine level " + mineLevel );
            UtilityMethod.GetCustomTypeBtnByID(64).gameObject.SetActive(!isUpgrade);
            UtilityMethod.GetCustomTypeBtnByID(51).interactable = isUpgrade;
        });
        UtilityMethod.SetBtnEventCustomTypeByID(65, () =>
        {
            UpGradeCastle(CastlePopupType.factory);

            var isUpgrade = factoryLevel > 0;

            Debug.Log(isUpgrade + " factory level " + factoryLevel);
            UtilityMethod.GetCustomTypeBtnByID(65).gameObject.SetActive(!isUpgrade);
            UtilityMethod.GetCustomTypeBtnByID(52).interactable = isUpgrade;
        });

        UtilityMethod.SetBtnEventCustomTypeByID(51, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.mine);
        });
        UtilityMethod.SetBtnEventCustomTypeByID(52, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.factory);
        });
        UtilityMethod.SetBtnEventCustomTypeByID(53, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.camp);
        });
        UtilityMethod.SetBtnEventCustomTypeByID(54, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.lab);
        });

        // 64 금광 건설하기 버튼

        // 103 금광 건설에 필요한 골드 텍스트

    }

    void SetUi()
    {

    }


    public CastlePopupBase GetCastlePopupByType(EnumDefinition.CastlePopupType popupType)
    {
        return castlePopupList.FirstOrDefault(x => x.popupType == popupType);
    }


    // TODO : 리펙토링........
    public void OpenCastlePopup(EnumDefinition.CastlePopupType popupType)
    {
        switch (popupType)
        {
            case CastlePopupType.mine:
                var minePopup = (MinePopup)GetCastlePopupByType(popupType);
                var mineNextLevelData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel + 1);
                minePopup.btnUpgrade.interactable = mineNextLevelData != null;

                break;
            case CastlePopupType.factory:
                var factoryPopup = (MinePopup)GetCastlePopupByType(popupType);
                var factoryNextLevelData = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel + 1);
                factoryPopup.btnUpgrade.interactable = factoryNextLevelData != null;
                break;
        }
        // 업그레이트 버튼 활성/비활성
        GetCastlePopupByType(popupType).gameObject.SetActive(true);
    }


    // TODO : 리펙토링........
    public void UpGradeCastle(CastlePopupType type)
    {
        switch (type)
        {
            case CastlePopupType.mine:
                UpgradeMine((isSuccess, upgradeData) =>
                {
                    if (isSuccess)
                    {
                        var popup = (MinePopup)GetCastlePopupByType(type);
                        var nextLevelData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel + 1);
                        CastleBuildingData nextBuildData = null;
                        if (nextLevelData != null)
                        {
                            nextBuildData = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(nextLevelData);
                        }
                        popup.SetUpGradeText(upgradeData, nextBuildData);
                        castleController.SetBuildUpgrade(BuildingType.MINE, mineLevel);
                        if (nextBuildData == null)
                            popup.btnUpgrade.interactable = false;

                        Debug.Log("Upgrade Success " + type);
                    }
                    else
                    {
                        // 석탄 부족 POPUP
                        GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 16);
                        Debug.Log("Upgrade Fail");
                    }

                });
                break;
            case CastlePopupType.factory:
                UpgradeFactory((isSuccess, upgradeData) =>
                {
                    if (isSuccess)
                    {

                        var popup = (MinePopup)GetCastlePopupByType(type);
                        var nextLevelData = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel + 1);
                        CastleBuildingData nextBuildData = null;
                        if (nextLevelData != null)
                        {
                            nextBuildData = new CastleBuildingData().Create().SetGoodsType(GoodsType.coal).Clone(nextLevelData);
                        }
                        popup.SetUpGradeText(upgradeData, nextBuildData);
                        castleController.SetBuildUpgrade(BuildingType.FACTORY, factoryLevel);

                        if (nextBuildData == null)
                            popup.btnUpgrade.interactable = false;

                        // 성공 로그
                        Debug.Log("Upgrade Success " + type);
                    }
                    else
                    {
                        // 석탄 부족 POPUP
                        GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 16);
                        Debug.Log("Upgrade Fail");
                    }
                });
                break;
            case CastlePopupType.camp:
                break;
            case CastlePopupType.lab:
                break;
            default:
                break;
        }
    }


    void UpgradeMine()
    {

        // 가격만큼 resource 차감 후 레벨 업그레이드 진행
        GlobalData.instance.player.PayCoal(buildDataMine.price);
        mineLevel++;

        // 다음 레벨의 광산 정보 가져오기
        var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);

        // Clone 메소드를 이용하여 BuildDataMine 객체의 데이터 갱신
        buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);

        // Set Popup    UI
        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);
        var nextLevelData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel + 1);
        CastleBuildingData nextBuildData = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(nextLevelData);
        popup.SetUpGradeText(buildDataMine, nextBuildData);
        castleController.SetBuildUpgrade(BuildingType.MINE, mineLevel);
    }

    void UpgradeFactory()
    {
        // 가격만큼 resource 차감 후 레벨 업그레이드 진행
        GlobalData.instance.player.PayCoal(buildDataFactory.price);
        factoryLevel++;

        // 다음 레벨의 광산 정보 가져오기
        var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);

        // Clone 메소드를 이용하여 BuildDataMine 객체의 데이터 갱신
        buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataMine);

        // Set Popup    UI
        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);
        var nextLevelData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(factoryLevel + 1);
        CastleBuildingData nextBuildData = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(nextLevelData);
        popup.SetUpGradeText(buildDataMine, nextBuildData);
        castleController.SetBuildUpgrade(BuildingType.FACTORY, factoryLevel);
    }




    bool IsValidCastleUpgradePay(int price)
    {
        var value = GlobalData.instance.player.coal >= price;
        if (value)
        {
            return true;
        }
        else
        {
            // show popup 석탄 부족.
            GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 16);
            Debug.Log("Upgrade Fail");
            return false;
        }

    }

    // max level 체크
    bool IsValidCastleUpgradeLevel(int level, int price)
    {

        return true;
    }

    /*
     * UpgradeMine - 광산 업그레이드 메소드
     * 
     * @param completeCallback: UnityAction<bool, CastleBuildingData> 타입의 콜백 함수. 업그레이드 성공 여부와 다음 레벨 정보를 전달합니다.
     */
    public void UpgradeMine(UnityAction<bool, CastleBuildingData> completeCallback)
    {
        // 플레이어가 가진 coal(resource)이 광산의 가격보다 많을 때 업그레이드 진행
        if (GlobalData.instance.player.coal >= buildDataMine.price)
        {
            // 가격만큼 resource 차감 후 레벨 업그레이드 진행
            GlobalData.instance.player.PayCoal(buildDataMine.price);
            mineLevel++;

            // set save data
            GlobalData.instance.saveDataManager.SaveDataCastMineleLevel(mineLevel);

            // 다음 레벨의 광산 정보 가져오기
            var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);

            // Clone 메소드를 이용하여 BuildDataMine 객체의 데이터 갱신
            buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);

            // 업그레이드 성공 처리를 위해 completeCallback 호출
            completeCallback(true, buildDataMine);
        }
        else
        {
            // Coal(resource) 부족으로 업그레이드 실패 시 completeCallback 호출
            completeCallback(false, null);
        }
    }
    public void UpgradeFactory(UnityAction<bool, CastleBuildingData> completeCallback)
    {
        if (GlobalData.instance.player.coal >= buildDataFactory.price)
        {
            GlobalData.instance.player.PayCoal(buildDataFactory.price);
            factoryLevel++;
            // set save data
            GlobalData.instance.saveDataManager.SaveDataCastleFactoryLevel(factoryLevel);
            var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);
            buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);
            completeCallback(true, buildDataFactory);
        }
        else
        {
            completeCallback(false, null);
        }
    }



    // 골드 채굴
    // CastleBuildingData 클래스를 기준으로 코루틴을 사용하여 productionTime 한번씩 productionCount을 totlaValue에 더해주고 maxSupplyAmount을 넘어가면 더이상 totlaValue에 더하지 않는다
    // 이 함수는 CastleBuildingData를 인자로 받아 골드 채굴을 하는 IEnumerator입니다.
    IEnumerator MiningGold()
    {
        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);

        while (true)
        {
            // 해당 시간만큼 대기 후 다시 while문을 반복합니다.
            yield return new WaitForSeconds(buildDataMine.productionTime);

            // 이 조건문은 player의 coal이 충분한지 검사합니다.
            if (GlobalData.instance.player.coal >= buildDataMine.price && buildDataMine.level > 0)  // level이 0이면 채굴 불가
            {
                // 아래 두 줄은 player의 coal을 사용하여 채굴하고, productionCount만큼 totlaMiningValue를 업데이트합니다. 단, maxSupplyAmount를 넘지 않도록 합니다.
                GlobalData.instance.player.PayCoal(buildDataMine.price);
                buildDataMine.TotlaMiningValue = Mathf.Min(buildDataMine.totlaMiningValue + buildDataMine.productionCount, buildDataMine.maxSupplyAmount);

                // MinePopup UI를 설정하고 현재의 totalMiningValue를 팝업에 표시합니다. 
                popup.SetTextTotalMiningValue(buildDataMine.totlaMiningValue.ToString());
                Debug.Log("채굴된 골드: " + buildDataMine.totlaMiningValue + " 남은 시간: " + buildDataMine.productionTime);
            }


            //yield return new WaitForSeconds(3f);
        }
    }


    /// <summary> 골드 인출 </summary>
    // 이 함수는 골드 인출 버튼을 눌렀을 때 호출됩니다.
    public void WithdrawGold()
    {
        // BuildDataMine이 가지고 있는 총 채굴량을 withdrawnGold 변수에 저장합니다.
        int withdrawnGold = buildDataMine.totlaMiningValue;

        if (withdrawnGold > 0)
        {
            // Debug.Log를 사용하여 인출된 골드 양을 디버그 창에 출력합니다.
            Debug.Log("인출된 골드: " + withdrawnGold);

            // GlobalData의 instance에서 player를 가져와, AddGold() 함수를 사용하여 player의 소지금에 withdrawnGold만큼 추가합니다.
            GlobalData.instance.player.AddGold(withdrawnGold);

            // 모든 금 채굴량을 인출했으므로 BuildDataMine 객체의 totlaMiningValue를 0으로 설정합니다. 
            buildDataMine.TotlaMiningValue = 0;

            SetPriceText(CastlePopupType.mine);
        }
        else
        {
            Debug.Log("인출할 골드가 없습니다.");
        }
    }

    void SetPriceText(CastlePopupType popupType)
    {
        // var popup = (MinePopup)GetCastlePopupByType(popupType);
        // popup.SetTextTotalMiningValue("0");
    }

    // 뼈조각 채굴
    IEnumerator MiningBone()
    {
        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);
        while (true)
        {
            yield return new WaitForSeconds(buildDataFactory.productionTime);

            if (GlobalData.instance.player.coal >= buildDataFactory.price && buildDataFactory.level > 0)
            {
                GlobalData.instance.player.PayCoal(buildDataFactory.price);
                buildDataFactory.TotlaMiningValue = Mathf.Min(buildDataFactory.totlaMiningValue + buildDataFactory.productionCount, buildDataFactory.maxSupplyAmount);
                // set ui  

                popup.SetTextTotalMiningValue(buildDataFactory.totlaMiningValue.ToString());

                Debug.Log("채굴된 뼈조각: " + buildDataFactory.totlaMiningValue + " 남은 시간: " + buildDataFactory.productionTime);
            }
            //yield return new WaitForSeconds(3f);
        }
    }

    /// <summary> 뼈조각 인출 </summary>
    public void WithdrawBone()
    {
        int withdrawnBone = buildDataFactory.totlaMiningValue;

        if (withdrawnBone > 0)
        {
            Debug.Log("인출된 뼈조각: " + withdrawnBone);
            GlobalData.instance.player.AddBone(withdrawnBone);
            buildDataFactory.TotlaMiningValue = 0;
            SetPriceText(CastlePopupType.factory);
        }
        else
        {
            Debug.Log("인출할 뼈조각이 없습니다.");
        }
    }

}

[System.Serializable]
public class CastleBuildingData
{
    // 레벨
    public int level;
    // 골드 생산량
    public int productionCount;
    // 골드 최대 저장량 
    public int maxSupplyAmount;
    // 생산 시간
    public int productionTime;
    // 석탄 필요량
    public int price;
    public string currencyType;
    // 생산되는 재화 타입    
    EnumDefinition.GoodsType goodsType;
    // 총 생산량
    public int totlaMiningValue;
    public int TotlaMiningValue
    {

        get => totlaMiningValue;
        set
        {
            totlaMiningValue = value;
            var type = goodsType == EnumDefinition.GoodsType.gold ? CastlePopupType.mine : CastlePopupType.factory;
            var popup = (MinePopup)GlobalData.instance.castleManager.GetCastlePopupByType(type);
            popup.SetTextTotalMiningValue(totlaMiningValue.ToString());
        }

    }




    public CastleBuildingData data;

    public CastleBuildingData Create()
    {
        data = new CastleBuildingData();
        return this;
    }
    public CastleBuildingData SetGoodsType(EnumDefinition.GoodsType goodsType)
    {
        data.goodsType = goodsType;
        return this;
    }

    public CastleBuildingData Clone(MineAndFactoryBuildingData mineAndFactoryBuildingData)
    {
        data.level = mineAndFactoryBuildingData.level;
        data.productionCount = mineAndFactoryBuildingData.productionCount;
        data.maxSupplyAmount = mineAndFactoryBuildingData.maxSupplyAmount;
        data.productionTime = mineAndFactoryBuildingData.productionTime;
        data.price = mineAndFactoryBuildingData.price;
        return data;
    }
}