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

        //�ݱ� �˾� ����
        UtilityMethod.SetBtnEventCustomTypeByID(51, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.mine);
        });

        //������ �˾� ����
        UtilityMethod.SetBtnEventCustomTypeByID(52, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.factory);
        });

        //ķ�� �˾� ����
        UtilityMethod.SetBtnEventCustomTypeByID(53, () =>
        {
            OpenCastlePopup(EnumDefinition.CastlePopupType.dungeon);
        });

        //������ �˾� ����
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
            // ��� ä�� ����
            StartCoroutine("MiningGold");
        }

        if (buildDataFactory.level > 0)
        {
            digUpBoneTime = buildDataMine.productionTime;
            // ������ ä�� ����
            StartCoroutine("MiningBone");
        }

        yield return null;
    }

    void SetCastleData()
    {
        //���� ������ 0���� ����
        mineLevel = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.mineLevel;
        factoryLevel = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.factoryLevel;

        //Level�� �ش��ϴ� ������ ��������
        var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);
        var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);

        buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);
        buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);

        buildDataMine.TotlaMiningValue = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.savedGold;
        buildDataFactory.TotlaMiningValue = GlobalData.instance.saveDataManager.saveDataTotal.saveDataCastle.savedBone;

        // �ʱ� UI ���� ( POPUP )
        var minePopup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);
        buildDataMine.goodsType = GoodsType.gold;
        minePopup.SetBuildingUI(buildDataMine);

        var factoryPopup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);
        buildDataFactory.goodsType = GoodsType.bone;
        factoryPopup.SetBuildingUI(buildDataFactory);

        // �ʱ� UI ���� ( CASTLE )
        castleController.SetMineBuild(mineLevel);
        castleController.SetFactoryBuild(factoryLevel);

        var dungeonPopup = (DungeonPopup)GetCastlePopupByType(CastlePopupType.dungeon);

        dungeonPopup.Init();

    }
    // ��� ä��
    // CastleBuildingData Ŭ������ �������� �ڷ�ƾ�� ����Ͽ� productionTime �ѹ��� productionCount�� totlaValue�� �����ְ� maxSupplyAmount�� �Ѿ�� ���̻� totlaValue�� ������ �ʴ´�
    // �� �Լ��� CastleBuildingData�� ���ڷ� �޾� ��� ä���� �ϴ� IEnumerator�Դϴ�.
    IEnumerator MiningGold()
    {

        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);
        //digUpGoldTime = buildDataMine.productionTime;
        popup.SetTextDigUpFullText("ä����");

        while (true)
        {
            yield return null;

            if (digUpGoldTime <= 0)
            {
                //�ִ� ���差�� �������� �ʾҴٸ�
                if (buildDataMine.TotlaMiningValue < buildDataMine.maxSupplyAmount)
                {
                    //�ٽ� Ÿ�̸� �ʱ�ȭ
                    digUpGoldTime = buildDataMine.productionTime;
                    buildDataMine.TotlaMiningValue = System.Math.Min(buildDataMine.TotlaMiningValue + buildDataMine.productionCount, buildDataMine.maxSupplyAmount);
                    GlobalData.instance.saveDataManager.SaveDataCastleSaveGold(buildDataMine.TotlaMiningValue);
                    popup.SetTextDigUpFullText("ä����");

                }
                else
                {
                    //�ִ� ���差�� �����ߴٸ�
                    digUpGoldTime = 0;
                    popup.SetTextDigUpFullText("����Ұ� �� ã���ϴ�");
                }

            }

            digUpGoldTime -= Time.deltaTime;
            Mining(popup, digUpGoldTime, buildDataMine.productionTime, buildDataMine);

        }

    }
    // ������ ä��
    IEnumerator MiningBone()
    {
        var popup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);
        //digUpBoneTime = buildDataFactory.productionTime;
        popup.SetTextDigUpFullText("ä����");

        while (true)
        {

            yield return null;
            if (digUpBoneTime <= 0)
            {
                //�ִ� ���差�� �������� �ʾҴٸ�
                if (buildDataFactory.TotlaMiningValue < buildDataFactory.maxSupplyAmount)
                {
                    //�ٽ� Ÿ�̸� �ʱ�ȭ
                    digUpBoneTime = buildDataFactory.productionTime;
                    buildDataFactory.TotlaMiningValue = System.Math.Min(buildDataFactory.TotlaMiningValue + buildDataFactory.productionCount, buildDataFactory.maxSupplyAmount);
                    GlobalData.instance.saveDataManager.SaveDataCastleSaveBone(buildDataFactory.TotlaMiningValue);
                    popup.SetTextDigUpFullText("ä����");

                }
                else
                {
                    //�ִ� ���差�� �����ߴٸ�
                    digUpBoneTime = 0;
                    popup.SetTextDigUpFullText("����Ұ� �� ã���ϴ�");

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

    /// <summary> ��� ���� </summary>
    // �� �Լ��� ��� ���� ��ư�� ������ �� ȣ��˴ϴ�.
    public void WithdrawGold()
    {
        // BuildDataMine�� ������ �ִ� �� ä������ withdrawnGold ������ �����մϴ�.
        float withdrawnGold = buildDataMine.TotlaMiningValue;

        var minePopup = (MinePopup)GetCastlePopupByType(CastlePopupType.mine);

        if (withdrawnGold > 0)
        {
            GlobalData.instance.soundManager.PlaySfxUI(SFX_TYPE.Reward);
            // Debug.Log�� ����Ͽ� ����� ��� ���� ����� â�� ����մϴ�.
            //Debug.Log("����� ���: " + withdrawnGold);

            // GlobalData�� instance���� player�� ������, AddGold() �Լ��� ����Ͽ� player�� �����ݿ� withdrawnGold��ŭ �߰��մϴ�.
            GlobalData.instance.player.AddGold(withdrawnGold);

            // ��� �� ä������ ���������Ƿ� BuildDataMine ��ü�� totlaMiningValue�� 0���� �����մϴ�. 
            buildDataMine.TotlaMiningValue = 0;
            GlobalData.instance.saveDataManager.SaveDataCastleSaveGold(buildDataMine.TotlaMiningValue);
        }
        else
        {
            Debug.Log("������ ��尡 �����ϴ�.");
        }
    }

    /// <summary> ������ ���� </summary>
    public void WithdrawBone()
    {
        float withdrawnBone = buildDataFactory.TotlaMiningValue;
        var factoryPopup = (MinePopup)GetCastlePopupByType(CastlePopupType.factory);

        if (withdrawnBone > 0)
        {
            GlobalData.instance.soundManager.PlaySfxUI(SFX_TYPE.Reward);

            Debug.Log("����� ������: " + withdrawnBone);
            GlobalData.instance.player.AddBone(withdrawnBone);
            buildDataFactory.TotlaMiningValue = 0;
            GlobalData.instance.saveDataManager.SaveDataCastleSaveBone(buildDataFactory.TotlaMiningValue);

        }
        else
        {
            Debug.Log("������ �������� �����ϴ�.");
        }
    }
    // TODO : �����丵........
    public void OpenCastlePopup(EnumDefinition.CastlePopupType popupType)
    {
        //��ư ��������Ʈ�� ����Ǵ� �κ�
        //GlobalData.instance.uiController.ButtonInteractableCheck(EnumDefinition.RewardType.gem);
        // ���׷���Ʈ ��ư Ȱ��/��Ȱ��
        
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
    // TODO : �����丵........
    public void UpGradeCastle(CastlePopupType type)
    {
        Debug.Log(type);

        switch (type)
        {
            case CastlePopupType.mine:
                //���׷��̵� ��� üũ �� ���� ����
                CheckEnoughCostMine((isSuccess, upgradeBuildingData) =>
                {
                    if (isSuccess)
                    {
                        var popup = (MinePopup)GetCastlePopupByType(type);
                        //�ǹ� ���׷��̵� ����
                        castleController.SetBuildUpgrade(BuildingType.MINE, mineLevel);
                        //���� ������ �ش��ϴ� UI Text ����
                        popup.SetBuildingUI(upgradeBuildingData);

                        //���� ������ ����
                        var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);
                        // Clone �޼ҵ带 �̿��Ͽ� BuildDataMine ��ü�� ������ ����
                        buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);
                        //������� ����� ��带 ������ �����Ϳ� ����
                        var tempSaveGold = buildDataMine.TotlaMiningValue;
                        buildDataMine.TotlaMiningValue = tempSaveGold;
                        
                        digUpGoldTime = 0;
                        //Debug.Log( isUpgrade + " mine level " + 

                        // ��� ä�� ����
                        StopCoroutine("MiningGold");
                        StartCoroutine("MiningGold");
                        //Debug.Log("Upgrade Success " + type);
                    }
                    else
                    {
                        // ��ź ���� POPUP
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
                        //�ǹ� ���׷��̵� ����
                        castleController.SetBuildUpgrade(BuildingType.FACTORY, factoryLevel);
                        //���� ������ �ش��ϴ� UI Text ����
                        popup.SetBuildingUI(upgradeBuildingData);
                        //���� ������ ����
                        var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);
                        // Clone �޼ҵ带 �̿��Ͽ� BuildData��ü�� ������ ����
                        buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);
                        //������� ����� �������� ������ �����Ϳ� ����
                        var tempSaveBone = buildDataMine.TotlaMiningValue;
                        buildDataFactory.TotlaMiningValue = tempSaveBone;
                        digUpBoneTime = 0;
                        //Debug.Log(isUpgrade + " factory level " + factoryLevel);

                        // ��� ä�� ����
                        StopCoroutine("MiningBone");
                        StartCoroutine("MiningBone");

                        // ���� �α�
                        //Debug.Log("Upgrade Success " + type);
                    }
                    else
                    {
                        // ��ź ���� POPUP
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
     * UpgradeMine - ���� ���׷��̵� �޼ҵ�
     * 
     * @param completeCallback: UnityAction<bool, CastleBuildingData> Ÿ���� �ݹ� �Լ�. ���׷��̵� ���� ���ο� ���� ���� ������ �����մϴ�.
     */
    public void CheckEnoughCostMine(UnityAction<bool, CastleBuildingData> completeCallback)
    {

        var targetBuildingData = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel + 1);

        Debug.Log(targetBuildingData.price);

        // �÷��̾ ���� coal(resource)�� ������ ���ݺ��� ���� �� ���׷��̵� ����
        if (GlobalData.instance.player.coal >= targetBuildingData.price)
        {
            GlobalData.instance.soundManager.PlaySfxInGame(SFX_TYPE.Upgrade);
            // ���ݸ�ŭ resource ���� �� ���� ���׷��̵� ����
            GlobalData.instance.player.PayCoal(targetBuildingData.price);

            mineLevel++;
            // set save data
            GlobalData.instance.saveDataManager.SaveDataCastMineleLevel(mineLevel);
            // ���� ������ ���� ���� ��������
            var refBuildDataMine = GlobalData.instance.dataManager.GetBuildDataMineByLevel(mineLevel);
            // Clone �޼ҵ带 �̿��Ͽ� BuildDataMine ��ü�� ������ ����
            buildDataMine = new CastleBuildingData().Create().SetGoodsType(GoodsType.gold).Clone(refBuildDataMine);
            // ���׷��̵� ���� ó���� ���� completeCallback ȣ��
            completeCallback(true, buildDataMine);

            Debug.Log("���׷��̵� ����");
        }
        else
        {
            Debug.Log("���׷��̵� ����");

            // Coal(resource) �������� ���׷��̵� ���� �� completeCallback ȣ��
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
            // ���� ������ ���� ���� ��������
            var refBuildDataFactory = GlobalData.instance.dataManager.GetBuildDataFactoryByLevel(factoryLevel);
            // Clone �޼ҵ带 �̿��Ͽ� BuildDataMine ��ü�� ������ ����
            buildDataFactory = new CastleBuildingData().Create().SetGoodsType(GoodsType.bone).Clone(refBuildDataFactory);
            // ���׷��̵� ���� ó���� ���� completeCallback ȣ��
            completeCallback(true, buildDataFactory);
        }
        else
        {
            // Coal(resource) �������� ���׷��̵� ���� �� completeCallback ȣ��
            completeCallback(false, null);
        }
    }


}





[System.Serializable]
public class CastleBuildingData
{
    // ����
    public int level;
    // ��� ���귮
    public float productionCount;
    // ��� �ִ� ���差 
    public float maxSupplyAmount;
    // ���� �ð�
    public int productionTime;
    // ��ź �ʿ䷮
    public float price;
    public string currencyType;
    // ����Ǵ� ��ȭ Ÿ��    
    public EnumDefinition.GoodsType goodsType;
    // �� ���귮
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