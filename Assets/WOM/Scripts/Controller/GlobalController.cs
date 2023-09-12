using System.Collections;
using UnityEngine;

public class GlobalController : MonoBehaviour
{

    public DataManager dataManager;
    public PlayerDataManager playerDataManager;
    public InsectManager insectManager;
    public StageManager stageManager;
    public MonsterManager monsterManager;
    public Player player;
    public AttackController attackController;
    public UiController uiController;
    public EffectManager effectManager;
    public LotteryManager lotteryManager;
    public EvolutionDiceLotteryManager evolutionDiceLotteryManager;
    public SkillManager skillManager;
    public UnionManager unionManager;
    public DNAManager dnaManager;
    public TraningManager traningManager;
    public EvolutionManager evolutionManager;
    public ShopManager shopManager;
    public UnionSpwanManager unionSpwanManager;
    public InsectSpwanManager insectSpwanManager;
    public StatManager statManager;
    public SaveDataManager saveDataManager;
    public QuestManager questManager;
    public SoundManager soundManager;
    public GoldPigController goldPigController;
    public LabBuildingManager labBuildingManager;
    public AD_Manager adManager;
    public OfflineRewardPopupContoller offlineRewardPopupContoller;

    public InAppPurchaseManager inAppPurchaseManager;

    public TutorialManager tutorialManager;




    void Start()
    {
        if (dataManager == null) dataManager = FindObjectOfType<DataManager>();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        // black screen on
        GlobalData.instance.effectManager.EnableTransitionBlackScreen();
        // ���� �Ұ��� ���·� ��ȯ
        attackController.SetAttackableState(false);
        // ��ư �Ұ��� ���·� ��ȯ
        UtilityMethod.EnableUIEventSystem(false);
        // set data
        yield return StartCoroutine(dataManager.SetDatas());

        // save data load
        yield return StartCoroutine(saveDataManager.Init());

        // get player data ( ���� ������ ���� �Ǿ��ִ� ������ �ε� )
        yield return StartCoroutine(playerDataManager.InitPlayerData());

        if(playerDataManager.saveData.stageIdx >= StaticDefine.MAX_STAGE) playerDataManager.saveData.stageIdx = StaticDefine.MAX_STAGE;

        // �������� ����
        yield return StartCoroutine(stageManager.Init(playerDataManager.saveData.stageIdx));

        // ���� ����
        yield return StartCoroutine(insectManager.Init(playerDataManager));

        // ����Ʈ ����
        yield return StartCoroutine(effectManager.Init());

        // Player data ����
        yield return StartCoroutine(player.Init(playerDataManager.saveData));

        // UI Controller ����
        yield return StartCoroutine(uiController.Init());

        // ���� ����
        yield return StartCoroutine(monsterManager.Init(stageManager.stageData.stageId));

        // Ÿ�� ���� ���� -> ù ������ ��� ����
        player.SetCurrentMonster(monsterManager.monsterNormal);
        player.SetCurrentMonsterHP(monsterManager.monsterNormal.hp);
        // ���� ���� Ȱ��ȭ -> ù ������ ��� ����
        // TODO : �ϳ��� �Ѿ� �ϴ��� Ȯ�� �ʿ� ���� ��� ��Ÿ������ ����
        //monsterManager.EnableMonster(EnumDefinition.MonsterType.normal);

        // Battle UI �ʱ�ȭ
        SetBattleUI_Init();

        // �Ʒ� �����͹� UI ����
        yield return StartCoroutine(traningManager.Init());

        // ���� �Ŵ��� �ʱ� ����
        yield return StartCoroutine(statManager.Init());

        // ��ų �����͹� UI ����
        yield return StartCoroutine(skillManager.Init());

        // ������ �����͹� UI ����
        yield return StartCoroutine(dnaManager.Init());

        // ��ȭ�Ŵ��� �ʱ� ����
        yield return StartCoroutine(evolutionManager.Init());

        // �̱� �Ŵ��� �ʱ�ȭ ( ù �̱� �����ʹ� ��� 0�� )
        yield return StartCoroutine(lotteryManager.Init());

        // ��ȭ �ֻ��� �̱� ����
        yield return StartCoroutine(evolutionDiceLotteryManager.Init());

        // ���Ͽ� ���� �Ŵ��� ����
        yield return StartCoroutine(unionSpwanManager.Init());

        // ���� ���� �Ŵ��� ����
        yield return StartCoroutine(insectSpwanManager.Init());

        // ���� �Ŵ��� ����
        yield return StartCoroutine(GlobalData.instance.dungeonManager.Init());

        // ���Ͽ� �����͹� UI ����
        yield return StartCoroutine(unionManager.Init());

        // ���� �Ŵ��� �ʱ�ȭ
        yield return StartCoroutine(soundManager.Init());

        // ���� �Ŵ��� �ʱ�ȭ
        yield return StartCoroutine(adManager.Init());

        // ĳ�� �ʱ�ȭ
        yield return StartCoroutine(GlobalData.instance.castleManager.Init());

        // �� �ʱ�ȭ
        yield return StartCoroutine(labBuildingManager.Init());

        // ���� �����͹� UI ����
        yield return StartCoroutine(shopManager.Init());

        // ���丮�� �ʱ�ȭ
        yield return StartCoroutine(tutorialManager.Init());

        // ���丮�� ���� ��Ȳ�� ���� ������ư Ȱ�� / ��Ȱ��ȭ
        var isShopBtnActive = tutorialManager.isTutorial == false;
        UtilityMethod.GetCustomTypeBtnByID(6).gameObject.SetActive(isShopBtnActive);

        //���� Ʃ�丮�� SetId�� 8��(���Ͽ»̱�) ���� ũ�ٸ� ĳ����ư ��Ȱ��ȭ
        var active = GlobalData.instance.tutorialManager.GetTutorialSetId() > 8 ? true : false;
            uiController.castleButtonObj.SetActive(active);

        // ��� �Ǳ� ����( ������ �ð� ������ ���� )
        yield return StartCoroutine(goldPigController.Init());

        // Ʈ������ �ƿ� ( black screen )
        yield return StartCoroutine(GlobalData.instance.effectManager.TransitionOut());

        // ����Ʈ �Ŵ��� ����
        yield return StartCoroutine(questManager.Init());

        // �� ������ ���
        yield return new WaitForEndOfFrame();

        // �������� ���� �˾� ����
        yield return StartCoroutine(offlineRewardPopupContoller.Init());

        // Monster In Animation
        yield return StartCoroutine(player.currentMonster.inOutAnimator.AnimPositionIn());

        yield return StartCoroutine(inAppPurchaseManager.Init());

        // ���� ���� Ȱ��ȭ -> tutorial pattenr 10 ���� Ȱ��ȭ
        if (tutorialManager.isTutorial == false)
            insectSpwanManager.AllTimerStart();
        else if (tutorialManager.GetTutorialSetById(5).isSetComplete)
        {
            // ���丮�� 5�� ��Ʈ �Ϸ�� ���� ���� Ȱ��ȭ
            insectSpwanManager.AllTimerStart();
        }

        // ���� ���� ���·� ��ȯ
        attackController.SetAttackableState(true);

        // ��ư ���� ���·� ��ȯ
        UtilityMethod.EnableUIEventSystem(true);

        if (tutorialManager.isTutorial && tutorialManager.newUserEventPopupObj.activeSelf == false)
        {
            tutorialManager.TutorialStart();
        }

    }

    void SetBattleUI_Init()
    {
        // set boss monster hp
        var hp = player.currentMonster.hp;
        var phaseCount = player.currentStageData.phaseCount;
        uiController.SetTxtMonsterHp(hp);
        uiController.SetSliderMonsterHp(hp);
        uiController.SetTxtPhaseCount(phaseCount);
        uiController.SetSliderPhaseValue(phaseCount);
    }


}
