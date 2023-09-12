using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using static EnumDefinition;
using ProjectGraphics;
using static ProjectGraphics.CastleController;
using UnityEngine.Experimental.AI;

public class CastleManager : MonoBehaviour
{
    public List<CastlePopupBase> castlePopupList = new List<CastlePopupBase>();

    public CastleBuildingData buildDataMine;
    public CastleBuildingData buildDataFactory;

    public CastleController castleController;

    public int mineLevel = 0;
    public int factoryLevel = 0;


    public int offLineSubGoldTime = 0;
    public int offLineSubBoneTime = 0;
    private float digUpGoldTime;
    private float maxDigUpGoldTime;
    private float digUpBoneTime;
    private float maxDigUpBoneTime;
    void Start()
    {
        SetBtnEvents();
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
    
    public void SetCampBuildingLevel(int gradeLevel)
    {

    }
    public void SetLabBuildingLevel(int totalLevel)
    {

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

        // offline time 보상
        var offlineTime = GlobalData.instance.playerDataManager.GetOfflineTimeValue();
        var second = (int)offlineTime.TotalSeconds;
//        Debug.Log("오프라인 타임! : " + second + " 초 ");
        if (buildDataMine.productionTime > 0)
        {
            //광산 -> 골드
            int mineCount = (second / buildDataMine.productionTime);
            offLineSubGoldTime = second - buildDataMine.productionTime;

            if(mineCount <= 0)
            {
                offLineSubGoldTime = buildDataMine.productionTime - second;
                offLineSubGoldTime = buildDataMine.productionTime - offLineSubGoldTime;
            }

            var mineAddValue = buildDataMine.productionCount * mineCount;
            var mineResulValue = (long)Mathf.Min(buildDataMine.totlaMiningValue + mineAddValue, buildDataMine.maxSupplyAmount);
            buildDataMine.TotlaMiningValue = mineResulValue;
//            Debug.Log("캐슬 -> 광산 오프라인 획득 금액 : " + mineAddValue + " 실제 적용된 값 : " + mineResulValue + " count " + mineCount);
        }

        if (buildDataFactory.productionTime > 0)
        {
            //가공소 -> 뼈조각
            int factoryCount = (second / buildDataFactory.productionTime);

            offLineSubBoneTime = second - buildDataFactory.productionTime;

            if(factoryCount <= 0)
            {
                offLineSubBoneTime = buildDataFactory.productionTime - second;
                offLineSubBoneTime = buildDataFactory.productionTime - offLineSubBoneTime;
            }

            var factoryAddValue = buildDataFactory.productionCount * factoryCount;
            var factoryResulValue = (long)Mathf.Min(buildDataFactory.totlaMiningValue + factoryAddValue, buildDataFactory.maxSupplyAmount);
            buildDataFactory.TotlaMiningValue = factoryResulValue;
//            Debug.Log("캐슬 -> 가공소 오프라인 획득 금액 : " + factoryAddValue + " 실제 적용된 값 : " + factoryResulValue + " count " + factoryCount);
        }



        // 초기 UI 설정 ( POPUP )
        var minePopup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);
        buildDataMine.goodsType = GoodsType.gold;
        minePopup.InitUIText(buildDataMine);

        var factoryPopup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);
        buildDataFactory.goodsType = GoodsType.bone;
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
  
            // 다음 레벨의 광산 정보 가져오기
            var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);

            // Clone 메소드를 이용하여 BuildDataMine 객체의 데이터 갱신
            buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);
            
            //Debug.Log( isUpgrade + " mine level " + 
            UtilityMethod.GetCustomTypeBtnByID(64).gameObject.SetActive(!isUpgrade);
            UtilityMethod.GetCustomTypeBtnByID(51).interactable = isUpgrade;

        });
        UtilityMethod.SetBtnEventCustomTypeByID(65, () =>
        {
            UpGradeCastle(CastlePopupType.factory);

            var isUpgrade = factoryLevel > 0;

            var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);
            buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);

            //Debug.Log(isUpgrade + " factory level " + factoryLevel);
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
                if (mineNextLevelData == null)
                    minePopup.SetMaxUI();
                // else
                //     minePopup.btnUpgrade.interactable = true;

                break;
            case CastlePopupType.factory:
                var factoryPopup = (MinePopup)GetCastlePopupByType(popupType);
                var factoryNextLevelData = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel + 1);
                if (factoryNextLevelData == null)
                    factoryPopup.SetMaxUI();
                //factoryPopup.btnUpgrade.interactable = factoryNextLevelData != null;
                break;
        }
        // 업그레이트 버튼 활성/비활성
        GetCastlePopupByType(popupType).gameObject.SetActive(true);
        GlobalData.instance.uiController.ButtonInteractableCheck(EnumDefinition.RewardType.gem);

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
                            popup.SetMaxUI();

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
                            popup.SetMaxUI();

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



    public float GetDigUpTime()
    {
        return digUpGoldTime;
    }

    // 골드 채굴
    // CastleBuildingData 클래스를 기준으로 코루틴을 사용하여 productionTime 한번씩 productionCount을 totlaValue에 더해주고 maxSupplyAmount을 넘어가면 더이상 totlaValue에 더하지 않는다
    // 이 함수는 CastleBuildingData를 인자로 받아 골드 채굴을 하는 IEnumerator입니다.
    IEnumerator MiningGold()
    {

        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);

        if (buildDataMine.level > 0)
        {
            digUpGoldTime = offLineSubGoldTime;
        }

        while (true)
        {
            yield return null;
            popup.SetTextDigUpFullText("채굴 준비 중..");

            //0레벨일때는 채굴 불가
            if(buildDataMine.level > 0)
            {
                //채굴 생산시간 가져오기
                maxDigUpGoldTime = buildDataMine.productionTime;

                if(digUpGoldTime < maxDigUpGoldTime)
                {
                    popup.SetTextDigUpFullText("채굴중");

                    digUpGoldTime += Time.deltaTime;

                    popup.SetTextDigUpTimeValue(maxDigUpGoldTime, digUpGoldTime);

                    if (digUpGoldTime >= maxDigUpGoldTime)
                    {

                        buildDataMine.TotlaMiningValue = System.Math.Min(buildDataMine.totlaMiningValue + buildDataMine.productionCount, buildDataMine.maxSupplyAmount);
                        // MinePopup UI를 설정하고 현재의 totalMiningValue를 팝업에 표시합니다. 
                        popup.SetTextTotalMiningValue(buildDataMine.totlaMiningValue.ToString());

                        //광산이 꽉 차지 않았을때만 생산
                        if (buildDataMine.totlaMiningValue < buildDataMine.maxSupplyAmount)
                        {
                            digUpGoldTime = 0;
                        }

                    }
                }
                else
                {
                    popup.SetTextDigUpFullText("금광이 꽉 찾습니다");
                }
                
            }
            
            popup.SetTextDigUpFullText("");

        }

    }

    /// <summary> 골드 인출 </summary>
    // 이 함수는 골드 인출 버튼을 눌렀을 때 호출됩니다.
    public void WithdrawGold()
    {
        // BuildDataMine이 가지고 있는 총 채굴량을 withdrawnGold 변수에 저장합니다.
        long withdrawnGold = buildDataMine.totlaMiningValue;

        if (withdrawnGold > 0)
        {
            // Debug.Log를 사용하여 인출된 골드 양을 디버그 창에 출력합니다.
            Debug.Log("인출된 골드: " + withdrawnGold);

            // GlobalData의 instance에서 player를 가져와, AddGold() 함수를 사용하여 player의 소지금에 withdrawnGold만큼 추가합니다.
            GlobalData.instance.player.AddGold(withdrawnGold);

            // 모든 금 채굴량을 인출했으므로 BuildDataMine 객체의 totlaMiningValue를 0으로 설정합니다. 
            buildDataMine.TotlaMiningValue = 0;

            SetPriceText(CastlePopupType.mine);

            digUpGoldTime = 0;

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

        if (buildDataFactory.level > 0)
        {
            digUpBoneTime = offLineSubBoneTime;
        }

        popup.SetTextDigUpFullText("채굴 준비 중..");

        while (true)
        {

            yield return null;

            //0레벨일때는 채굴 불가
            if (buildDataFactory.level > 0)
            {
                //채굴 생산시간 가져오기
                maxDigUpBoneTime = buildDataFactory.productionTime;

                if (digUpBoneTime < maxDigUpBoneTime)
                {
                    popup.SetTextDigUpFullText("채굴중");

                    digUpBoneTime += Time.deltaTime;

                    popup.SetTextDigUpTimeValue(maxDigUpBoneTime, digUpBoneTime);

                    if (digUpBoneTime >= maxDigUpBoneTime)
                    {

                        buildDataFactory.TotlaMiningValue = System.Math.Min(buildDataFactory.totlaMiningValue + buildDataFactory.productionCount, buildDataFactory.maxSupplyAmount);
                        // MinePopup UI를 설정하고 현재의 totalMiningValue를 팝업에 표시합니다. 
                        popup.SetTextTotalMiningValue(buildDataFactory.totlaMiningValue.ToString());
                        //Debug.Log("채굴된 뼈조각: " + buildDataFactory.totlaMiningValue + " 남은 시간: " + buildDataFactory.productionTime);

                        //광산이 꽉 차지 않았을때만 생산
                        if (buildDataFactory.totlaMiningValue < buildDataFactory.maxSupplyAmount)
                        {
                            digUpBoneTime = 0;
                        }

                    }
                }
                else
                {
                    popup.SetTextDigUpFullText("공장이 꽉 찾습니다");
                }

            }
            popup.SetTextDigUpFullText("");

        }
    }

    /// <summary> 뼈조각 인출 </summary>
    public void WithdrawBone()
    {
        long withdrawnBone = buildDataFactory.totlaMiningValue;

        if (withdrawnBone > 0)
        {
            Debug.Log("인출된 뼈조각: " + withdrawnBone);
            GlobalData.instance.player.AddBone(withdrawnBone);
            buildDataFactory.TotlaMiningValue = 0;
            SetPriceText(CastlePopupType.factory);
            digUpBoneTime = 0;
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
    public long productionCount;
    // 골드 최대 저장량 
    public long maxSupplyAmount;
    // 생산 시간
    public int productionTime;
    // 석탄 필요량
    public long price;
    public string currencyType;
    // 생산되는 재화 타입    
    public EnumDefinition.GoodsType goodsType;
    // 총 생산량
    public long totlaMiningValue;
    public long TotlaMiningValue
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