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


    //public int offLineSubGoldTime = 0;
    //public int offLineSubBoneTime = 0;
    private float digUpGoldTime;
    private float maxDigUpGoldTime;
    private float digUpBoneTime;
    private float maxDigUpBoneTime;
    void Start()
    {
        SetBtnEvents();
    }
    void SetBtnEvents()
    {
        // 금광 건설하기 버튼
        UtilityMethod.SetBtnEventCustomTypeByID(64, () =>
        {
            UpGradeCastle(CastlePopupType.mine);
        });
        //가공소 건설하기 버튼
        UtilityMethod.SetBtnEventCustomTypeByID(65, () =>
        {
            UpGradeCastle(CastlePopupType.factory);
        });
        //금광 팝업 열기
        UtilityMethod.SetBtnEventCustomTypeByID(51, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.mine);
        });

        //가공소 팝업 열기
        UtilityMethod.SetBtnEventCustomTypeByID(52, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.factory);
        });

        //캠프 팝업 열기
        UtilityMethod.SetBtnEventCustomTypeByID(53, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.camp);
        });

        //연구소 팝업 열기
        UtilityMethod.SetBtnEventCustomTypeByID(54, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.lab);
        });

        // 64 금광 건설하기 버튼

        // 103 금광 건설에 필요한 골드 텍스트
    }

    public IEnumerator Init()
    {
        // GET SAVE DATA
        SetCastleData();

        if (buildDataMine.level > 0)
        {
            digUpGoldTime = buildDataMine.productionTime;
            // 골드 채굴 시작
            StartCoroutine("MiningGold");
        }

        if (buildDataFactory.level > 0)
        {
            digUpBoneTime = buildDataMine.productionTime;
            // 뼈조각 채굴 시작
            StartCoroutine("MiningBone");
        }

        yield return null;
    }

    void SetCastleData()
    {
        //최초 세팅은 0레벨 시작
        mineLevel = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.mineLevel;
        factoryLevel = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.factoryLevel;

        //Level에 해당하는 데이터 가져오기
        var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);
        var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);

        buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);
        buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);

        buildDataMine.TotlaMiningValue = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.savedGold;
        buildDataFactory.TotlaMiningValue = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.savedBone;
        //TODO : 일단 오프라인 시간하는 부분 비활성화 / 현재까지 모아논 골드는 저장되지 않고 단순히 접속시간에 따른 생산량만 계산하기때문에
        #region  오프라인 시간에 따른 금광 가공소 세팅
        // // offline time 보상
        // var offlineTime = GlobalData.instance.playerDataManager.GetOfflineTimeValue();
        // var second = (int)offlineTime.TotalSeconds;

        // //빌딩 레벨이 0레벨 이상일 때만 오프라인 시간 계산
        // if (buildDataMine.level > 0)
        // {
        //     //        Debug.Log("오프라인 타임! : " + second + " 초 ");
        //     if (buildDataMine.productionTime > 0)
        //     {
        //         //광산 -> 골드
        //         int mineCount = (second / buildDataMine.productionTime);
        //         offLineSubGoldTime = second - buildDataMine.productionTime;

        //         if (mineCount <= 0)
        //         {
        //             offLineSubGoldTime = buildDataMine.productionTime - second;
        //             offLineSubGoldTime = buildDataMine.productionTime - offLineSubGoldTime;
        //         }

        //         var mineAddValue = buildDataMine.productionCount * mineCount;
        //         var mineResulValue = (long)Mathf.Min(buildDataMine.TotlaMiningValue + mineAddValue, buildDataMine.maxSupplyAmount);
        //         buildDataMine.TotlaMiningValue = mineResulValue;
        //         //            Debug.Log("캐슬 -> 광산 오프라인 획득 금액 : " + mineAddValue + " 실제 적용된 값 : " + mineResulValue + " count " + mineCount);
        //     }
        // }
        // if (buildDataFactory.level > 0)
        // {
        //     if (buildDataFactory.productionTime > 0)
        //     {
        //         //가공소 -> 뼈조각
        //         int factoryCount = (second / buildDataFactory.productionTime);

        //         offLineSubBoneTime = second - buildDataFactory.productionTime;

        //         if (factoryCount <= 0)
        //         {
        //             offLineSubBoneTime = buildDataFactory.productionTime - second;
        //             offLineSubBoneTime = buildDataFactory.productionTime - offLineSubBoneTime;
        //         }

        //         var factoryAddValue = buildDataFactory.productionCount * factoryCount;
        //         var factoryResulValue = (long)Mathf.Min(buildDataFactory.TotlaMiningValue + factoryAddValue, buildDataFactory.maxSupplyAmount);
        //         buildDataFactory.TotlaMiningValue = factoryResulValue;
        //         //            Debug.Log("캐슬 -> 가공소 오프라인 획득 금액 : " + factoryAddValue + " 실제 적용된 값 : " + factoryResulValue + " count " + factoryCount);
        //     }

        // }
        #endregion

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
        if (mineLevel > 0) SetUnLockButton(64, 51, true);
        if (factoryLevel > 0) SetUnLockButton(65, 52, true);

    }
    // 골드 채굴
    // CastleBuildingData 클래스를 기준으로 코루틴을 사용하여 productionTime 한번씩 productionCount을 totlaValue에 더해주고 maxSupplyAmount을 넘어가면 더이상 totlaValue에 더하지 않는다
    // 이 함수는 CastleBuildingData를 인자로 받아 골드 채굴을 하는 IEnumerator입니다.
    IEnumerator MiningGold()
    {

        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);
        digUpGoldTime = buildDataMine.productionTime;
        popup.SetTextDigUpFullText("채굴중");

        while (true)
        {
            yield return null;

            if (digUpGoldTime <= 0)
            {
                //최대 저장량에 도달하지 않았다면
                if (buildDataMine.TotlaMiningValue < buildDataMine.maxSupplyAmount)
                {
                    //다시 타이머 초기화
                    digUpGoldTime = buildDataMine.productionTime;
                    buildDataMine.TotlaMiningValue = System.Math.Min(buildDataMine.TotlaMiningValue + buildDataMine.productionCount, buildDataMine.maxSupplyAmount);
                    GlobalData.instance.saveDataManager.SaveDataCastleSaveGold(buildDataMine.TotlaMiningValue);
                    popup.SetTextDigUpFullText("채굴중");

                }
                else
                {
                    //최대 저장량에 도달했다면
                    digUpGoldTime = 0;
                    popup.SetTextDigUpFullText("저장소가 꽉 찾습니다");
                }

            }

            digUpGoldTime -= Time.deltaTime;
            Mining(popup, digUpGoldTime, buildDataMine.productionTime, buildDataMine);

        }

    }
    // 뼈조각 채굴
    IEnumerator MiningBone()
    {
        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);
        digUpBoneTime = buildDataFactory.productionTime;
        popup.SetTextDigUpFullText("채굴중");

        while (true)
        {

            yield return null;
            if (digUpBoneTime <= 0)
            {
                //최대 저장량에 도달하지 않았다면
                if (buildDataFactory.TotlaMiningValue < buildDataFactory.maxSupplyAmount)
                {
                    //다시 타이머 초기화
                    digUpBoneTime = buildDataFactory.productionTime;
                    buildDataFactory.TotlaMiningValue = System.Math.Min(buildDataFactory.TotlaMiningValue + buildDataFactory.productionCount, buildDataFactory.maxSupplyAmount);
                    GlobalData.instance.saveDataManager.SaveDataCastleSaveBone(buildDataFactory.TotlaMiningValue);
                    popup.SetTextDigUpFullText("채굴중");

                }
                else
                {
                    //최대 저장량에 도달했다면
                    digUpBoneTime = 0;
                    popup.SetTextDigUpFullText("저장소가 꽉 찾습니다");

                }
            }

            digUpBoneTime -= Time.deltaTime;
            Mining(popup, digUpBoneTime, buildDataFactory.productionTime, buildDataFactory);

        }
    }


    void Mining(MinePopup minePopup, float digUpTime, float maxDigUpTime, CastleBuildingData buildData)
    {
        minePopup.SetTextDigUpTimeValue(maxDigUpTime, digUpTime);
        minePopup.SetTextTotalMiningValue(buildData.TotlaMiningValue.ToString());
    }
    // 건설하기 버튼 UI Enable/Disable
    void SetUnLockButton(int n1, int n2, bool active)
    {
        UtilityMethod.GetCustomTypeBtnByID(n1).gameObject.SetActive(!active);

        UtilityMethod.GetCustomTypeBtnByID(n2).interactable = active;
    }

    public CastlePopupBase GetCastlePopupByType(EnumDefinition.CastlePopupType popupType)
    {
        return castlePopupList.FirstOrDefault(x => x.popupType == popupType);
    }

    /// <summary> 골드 인출 </summary>
    // 이 함수는 골드 인출 버튼을 눌렀을 때 호출됩니다.
    public void WithdrawGold()
    {
        // BuildDataMine이 가지고 있는 총 채굴량을 withdrawnGold 변수에 저장합니다.
        long withdrawnGold = buildDataMine.TotlaMiningValue;

        var minePopup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);

        if (withdrawnGold > 0)
        {
            // Debug.Log를 사용하여 인출된 골드 양을 디버그 창에 출력합니다.
            //Debug.Log("인출된 골드: " + withdrawnGold);

            // GlobalData의 instance에서 player를 가져와, AddGold() 함수를 사용하여 player의 소지금에 withdrawnGold만큼 추가합니다.
            GlobalData.instance.player.AddGold(withdrawnGold);

            // 모든 금 채굴량을 인출했으므로 BuildDataMine 객체의 totlaMiningValue를 0으로 설정합니다. 
            buildDataMine.TotlaMiningValue = 0;
            GlobalData.instance.saveDataManager.SaveDataCastleSaveGold(buildDataMine.TotlaMiningValue);
        }
        else
        {
            Debug.Log("인출할 골드가 없습니다.");
        }
    }

    /// <summary> 뼈조각 인출 </summary>
    public void WithdrawBone()
    {
        long withdrawnBone = buildDataFactory.TotlaMiningValue;
        var minePopup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);

        if (withdrawnBone > 0)
        {
            Debug.Log("인출된 뼈조각: " + withdrawnBone);
            GlobalData.instance.player.AddBone(withdrawnBone);
            buildDataFactory.TotlaMiningValue = 0;
            GlobalData.instance.saveDataManager.SaveDataCastleSaveBone(buildDataFactory.TotlaMiningValue);
        }
        else
        {
            Debug.Log("인출할 뼈조각이 없습니다.");
        }
    }
    // TODO : 리펙토링........
    public void OpenCastlePopup(EnumDefinition.CastlePopupType popupType)
    {
        switch (popupType)
        {
            case CastlePopupType.mine:
                var minePopup = (MinePopup)GetCastlePopupByType(popupType);
                minePopup.SetShowCanvasGroup(true);
                var mineNextLevelData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel + 1);
                if (mineNextLevelData == null)
                    minePopup.SetMaxUI();
                // else
                //     minePopup.btnUpgrade.interactable = true;

                break;
            case CastlePopupType.factory:
                var factoryPopup = (MinePopup)GetCastlePopupByType(popupType);
                factoryPopup.SetShowCanvasGroup(true);
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
                //업그레이드 비용 체크 및 레벨 증가
                CheckEnoughCostMine((isSuccess, upgradeBuildingData) =>
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
                        //현재 레벨에 해당하는 UI Text 세팅
                        popup.SetTextLevelData(upgradeBuildingData, nextBuildData);
                        //건물 업그레이드 연출
                        castleController.SetBuildUpgrade(BuildingType.MINE, mineLevel);
                        //Max레벨일 경우 UI 세팅
                        if (nextBuildData == null)
                        {
                            popup.SetMaxUI();
                        }

                        // 다음 레벨의 광산 정보 가져오기
                        var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);
                        // Clone 메소드를 이용하여 BuildDataMine 객체의 데이터 갱신
                        buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);

                        //Debug.Log( isUpgrade + " mine level " + 
                        SetUnLockButton(64, 51, true);
                        // 골드 채굴 시작
                        StopCoroutine("MiningGold");
                        StartCoroutine("MiningGold");
                        //Debug.Log("Upgrade Success " + type);
                    }
                    else
                    {
                        // 석탄 부족 POPUP
                        GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 16);
                        //Debug.Log("Upgrade Fail");
                    }

                });
                break;
            case CastlePopupType.factory:
                CheckEnoughCostFactory((isSuccess, upgradeBuildingData) =>
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
                        //현재 레벨에 해당하는 UI Text 세팅
                        popup.SetTextLevelData(upgradeBuildingData, nextBuildData);
                        //건물 업그레이드 연출

                        castleController.SetBuildUpgrade(BuildingType.FACTORY, factoryLevel);
                        if (nextBuildData == null)
                        {
                            popup.SetMaxUI();
                        }
                        var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);
                        buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);

                        //Debug.Log(isUpgrade + " factory level " + factoryLevel);
                        SetUnLockButton(65, 52, true);
                        // 골드 채굴 시작
                        StopCoroutine("MiningBone");
                        StartCoroutine("MiningBone");

                        // 성공 로그
                        //Debug.Log("Upgrade Success " + type);
                    }
                    else
                    {
                        // 석탄 부족 POPUP
                        GlobalData.instance.globalPopupController.EnableGlobalPopupByMessageId("Message", 16);
                        //Debug.Log("Upgrade Fail");
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
    public void CheckEnoughCostMine(UnityAction<bool, CastleBuildingData> completeCallback)
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
    public void CheckEnoughCostFactory(UnityAction<bool, CastleBuildingData> completeCallback)
    {
        if (GlobalData.instance.player.coal >= buildDataFactory.price)
        {
            GlobalData.instance.player.PayCoal(buildDataFactory.price);
            factoryLevel++;
            // set save data
            GlobalData.instance.saveDataManager.SaveDataCastleFactoryLevel(factoryLevel);
            // 다음 레벨의 광산 정보 가져오기
            var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);
            // Clone 메소드를 이용하여 BuildDataMine 객체의 데이터 갱신
            buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);
            // 업그레이드 성공 처리를 위해 completeCallback 호출
            completeCallback(true, buildDataFactory);
        }
        else
        {
            // Coal(resource) 부족으로 업그레이드 실패 시 completeCallback 호출
            completeCallback(false, null);
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
    long totlaMiningValue;
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