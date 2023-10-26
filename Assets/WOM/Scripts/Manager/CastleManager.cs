using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using UnityEngine.Events;
using static EnumDefinition;
using ProjectGraphics;
using static ProjectGraphics.CastleController;
using UnityEngine.Experimental.AI;
using TMPro;

public class CastleManager : MonoBehaviour
{
    public List<CastlePopupBase> castlePopupList = new List<CastlePopupBase>();

    public CastleBuildingData buildDataMine;
    public CastleBuildingData buildDataFactory;

    public CastleController castleController;

    public int mineLevel = 0;
    public int factoryLevel = 0;

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
            OpenCastlePopup(EnumDefinition.CastlePopupType.dungeon);
        });

        //연구소 팝업 열기
        UtilityMethod.SetBtnEventCustomTypeByID(54, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.lab);
        });


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

        // 초기 UI 설정 ( POPUP )
        var minePopup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);
        buildDataMine.goodsType = GoodsType.gold;
        minePopup.SetBuildingUI(buildDataMine);

        var factoryPopup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);
        buildDataFactory.goodsType = GoodsType.bone;
        factoryPopup.SetBuildingUI(buildDataFactory);

        // 초기 UI 설정 ( CASTLE )
        castleController.SetMineBuild(mineLevel);
        castleController.SetFactoryBuild(factoryLevel);

        var dungeonPopup = (DungeonPopup)GetCastlePopupByType(CastlePopupType.dungeon);

        dungeonPopup.Init();

    }
    // 골드 채굴
    // CastleBuildingData 클래스를 기준으로 코루틴을 사용하여 productionTime 한번씩 productionCount을 totlaValue에 더해주고 maxSupplyAmount을 넘어가면 더이상 totlaValue에 더하지 않는다
    // 이 함수는 CastleBuildingData를 인자로 받아 골드 채굴을 하는 IEnumerator입니다.
    IEnumerator MiningGold()
    {

        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);
        //digUpGoldTime = buildDataMine.productionTime;
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
        //digUpBoneTime = buildDataFactory.productionTime;
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
        minePopup.SetTextTotalMiningValue(buildData.TotlaMiningValue);
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
        float withdrawnGold = buildDataMine.TotlaMiningValue;

        var minePopup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);

        if (withdrawnGold > 0)
        {
            GlobalData.instance.soundManager.PlaySfxUI(SFX_TYPE.Reward);
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
        float withdrawnBone = buildDataFactory.TotlaMiningValue;
        var factoryPopup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);

        if (withdrawnBone > 0)
        {
            GlobalData.instance.soundManager.PlaySfxUI(SFX_TYPE.Reward);

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
        //버튼 스프라이트만 변경되는 부분
        //GlobalData.instance.uiController.ButtonInteractableCheck(EnumDefinition.RewardType.gem);
        // 업그레이트 버튼 활성/비활성
        
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


        GetCastlePopupByType(popupType).ShowPopup();

        //GetCastlePopupByType(popupType).gameObject.SetActive(true);

    }
    // TODO : 리펙토링........
    public void UpGradeCastle(CastlePopupType type)
    {
        Debug.Log(type);

        switch (type)
        {
            case CastlePopupType.mine:
                //업그레이드 비용 체크 및 레벨 증가
                CheckEnoughCostMine((isSuccess, upgradeBuildingData) =>
                {
                    if (isSuccess)
                    {
                        var popup = (MinePopup)GetCastlePopupByType(type);
                        //건물 업그레이드 연출
                        castleController.SetBuildUpgrade(BuildingType.MINE, mineLevel);
                        //현재 레벨에 해당하는 UI Text 세팅
                        popup.SetBuildingUI(upgradeBuildingData);

                        //광산 데이터 갱신
                        var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);
                        // Clone 메소드를 이용하여 BuildDataMine 객체의 데이터 갱신
                        buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);
                        //현재까지 저장된 골드를 갱신한 데이터에 저장
                        var tempSaveGold = buildDataMine.TotlaMiningValue;
                        buildDataMine.TotlaMiningValue = tempSaveGold;
                        
                        digUpGoldTime = 0;
                        //Debug.Log( isUpgrade + " mine level " + 

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
                        //건물 업그레이드 연출
                        castleController.SetBuildUpgrade(BuildingType.FACTORY, factoryLevel);
                        //현재 레벨에 해당하는 UI Text 세팅
                        popup.SetBuildingUI(upgradeBuildingData);
                        //광산 데이터 갱신
                        var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);
                        // Clone 메소드를 이용하여 BuildData객체의 데이터 갱신
                        buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);
                        //현재까지 저장된 뼛조각를 갱신한 데이터에 저장
                        var tempSaveBone = buildDataMine.TotlaMiningValue;
                        buildDataFactory.TotlaMiningValue = tempSaveBone;
                        digUpBoneTime = 0;
                        //Debug.Log(isUpgrade + " factory level " + factoryLevel);

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
            case CastlePopupType.dungeon:
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

        var targetBuildingData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel + 1);

        Debug.Log(targetBuildingData.price);

        // 플레이어가 가진 coal(resource)이 광산의 가격보다 많을 때 업그레이드 진행
        if (GlobalData.instance.player.coal >= targetBuildingData.price)
        {
            GlobalData.instance.soundManager.PlaySfxInGame(SFX_TYPE.Upgrade);
            // 가격만큼 resource 차감 후 레벨 업그레이드 진행
            GlobalData.instance.player.PayCoal(targetBuildingData.price);

            mineLevel++;
            // set save data
            GlobalData.instance.saveDataManager.SaveDataCastMineleLevel(mineLevel);
            // 다음 레벨의 광산 정보 가져오기
            var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);
            // Clone 메소드를 이용하여 BuildDataMine 객체의 데이터 갱신
            buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);
            // 업그레이드 성공 처리를 위해 completeCallback 호출
            completeCallback(true, buildDataMine);

            Debug.Log("업그레이드 성공");
        }
        else
        {
            Debug.Log("업그레이드 실패");

            // Coal(resource) 부족으로 업그레이드 실패 시 completeCallback 호출
            completeCallback(false, null);
        }
    }
    public void CheckEnoughCostFactory(UnityAction<bool, CastleBuildingData> completeCallback)
    {
        var targetBuildingData = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel + 1);

        Debug.Log(targetBuildingData.price);

        if (GlobalData.instance.player.coal >= targetBuildingData.price)
        {
            GlobalData.instance.soundManager.PlaySfxInGame(SFX_TYPE.Upgrade);

            GlobalData.instance.player.PayCoal(targetBuildingData.price);
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
    public float productionCount;
    // 골드 최대 저장량 
    public float maxSupplyAmount;
    // 생산 시간
    public int productionTime;
    // 석탄 필요량
    public float price;
    public string currencyType;
    // 생산되는 재화 타입    
    public EnumDefinition.GoodsType goodsType;
    // 총 생산량
    float totlaMiningValue;
    public float TotlaMiningValue
    {

        get => totlaMiningValue;
        set
        {
            totlaMiningValue = value;
            var type = goodsType == EnumDefinition.GoodsType.gold ? CastlePopupType.mine : CastlePopupType.factory;
            var popup = (MinePopup)GlobalData.instance.castleManager.GetCastlePopupByType(type);
            popup.SetTextTotalMiningValue(totlaMiningValue);

            if(totlaMiningValue > 0)
            {
                popup.SetGoodsButton(true);

            }
            else
            {
                popup.SetGoodsButton(false);
            }

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