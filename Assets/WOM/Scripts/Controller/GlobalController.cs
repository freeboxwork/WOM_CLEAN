using System.Collections;
using UnityEngine;
using BackEnd;
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

    //public TutorialManager tutorialManager;

    public FirebaseManager firebaseManager;

    public BackEndDataManager backEndDataManager;
    public GuideManager guideManager;

    void Awake()
    {
        var bk = Backend.Initialize(true);
        if (bk.IsSuccess())
        {
            Debug.Log("Backend Initialize Success");
        }
        else
        {
            Debug.Log("Backend Initialize Fail");
        }
    }
    void Start()
    {
        if (dataManager == null) dataManager = FindObjectOfType<DataManager>();
        StartCoroutine(Init());
    }

    IEnumerator Init()
    {
        // black screen on
        GlobalData.instance.effectManager.EnableTransitionBlackScreen();
        // 공격 불가능 상태로 전환
        attackController.SetAttackableState(false);
        // 버튼 불가능 상태로 전환
        UtilityMethod.EnableUIEventSystem(false);
        // set data
        yield return StartCoroutine(dataManager.SetDatas());

        // save data load
        yield return StartCoroutine(saveDataManager.Init());

        // get player data ( 게임 종료전 저장 되어있는 데이터 로드 )
        yield return StartCoroutine(playerDataManager.InitPlayerData());

        if (playerDataManager.saveData.stageIdx >= StaticDefine.MAX_STAGE) playerDataManager.saveData.stageIdx = StaticDefine.MAX_STAGE;

        yield return StartCoroutine(firebaseManager.Init());
        // 스테이지 세팅
        yield return StartCoroutine(stageManager.Init(playerDataManager.saveData.stageIdx));

        // 곤충 세팅
        yield return StartCoroutine(insectManager.Init(playerDataManager));

        // 이펙트 세팅
        yield return StartCoroutine(effectManager.Init());

        // Player data 세팅
        yield return StartCoroutine(player.Init(playerDataManager.saveData));
        // 캐슬 초기화
        yield return StartCoroutine(GlobalData.instance.castleManager.Init());
        // 뽑기 매니저 초기화 ( 첫 뽑기 데이터는 모두 0번 )
        yield return StartCoroutine(lotteryManager.Init());
        // UI Controller 세팅
        yield return StartCoroutine(uiController.Init());
        // 몬스터 세팅
        yield return StartCoroutine(monsterManager.Init(stageManager.stageData.stageId));

        // 타겟 몬스터 지정 -> 첫 시작은 노멀 몬스터
        player.SetCurrentMonster(monsterManager.monsterNormal);
        player.SetCurrentMonsterHP(monsterManager.monsterNormal.hp);
        // 등장 몬스터 활성화 -> 첫 시작은 노멀 몬스터
        // TODO : 하나만 켜야 하는지 확인 필요 현재 모두 나타나도록 수정
        //monsterManager.EnableMonster(EnumDefinition.MonsterType.normal);

        // Battle UI 초기화
        SetBattleUI_Init();

        // 훈련 데이터및 UI 세팅
        yield return StartCoroutine(traningManager.Init());

        // 스탯 매니저 초기 세팅
        yield return StartCoroutine(statManager.Init());

        // 스킬 데이터및 UI 세팅
        yield return StartCoroutine(skillManager.Init());

        // 유전자 데이터및 UI 세팅
        yield return StartCoroutine(dnaManager.Init());

        // 진화매니저 초기 세팅
        yield return StartCoroutine(evolutionManager.Init());

        // 진화 주사위 뽑기 세팅
        yield return StartCoroutine(evolutionDiceLotteryManager.Init());

        // 유니온 스폰 매니저 세팅
        yield return StartCoroutine(unionSpwanManager.Init());

        // 곤충 스폰 매니저 세팅
        yield return StartCoroutine(insectSpwanManager.Init());

        // 던전 매니저 세팅
        yield return StartCoroutine(GlobalData.instance.dungeonManager.Init());

        // 유니온 데이터및 UI 세팅
        yield return StartCoroutine(unionManager.Init());

        // 사운드 매니저 초기화
        yield return StartCoroutine(soundManager.Init());

        // 광고 매니저 초기화
        yield return StartCoroutine(adManager.Init());

        // 랩 초기화
        yield return StartCoroutine(labBuildingManager.Init());
        //가이드 초기화
        yield return StartCoroutine(guideManager.Init());   
        
        // 투토리얼 초기화
        //yield return StartCoroutine(tutorialManager.Init());

        // 투토리얼 실행 상황에 따라 상점버튼 활성 / 비활성화
        //UtilityMethod.GetCustomTypeBtnByID(6).gameObject.SetActive(tutorialManager.isEndTutorial);

        // 골드 피그 등장( 지정된 시간 지난뒤 등장 )
        yield return StartCoroutine(goldPigController.Init());
        //인앱 결제 초기화
        yield return StartCoroutine(inAppPurchaseManager.Init());
        // 퀘스트 매니저 세팅
        yield return StartCoroutine(questManager.Init());
        // 상점 데이터및 UI 세팅 자정 체크 때문에 무조건 퀘스트 매니저보다 나중에 Init해야함
        yield return StartCoroutine(shopManager.Init());
        // 오프라인 보상 팝업 등장
        yield return StartCoroutine(offlineRewardPopupContoller.Init());
        //만약 튜토리얼 SetId가 8번(유니온뽑기) 보다 크다면 캐슬버튼 비활성화
        //var active = GlobalData.instance.tutorialManager.GetTutorialSetId() > 4 ? true : false;
        //uiController.castleButtonObj.SetActive(true);

        //캐슬
        UtilityMethod.GetCustomTypeGMById(16).SetActive(true);
        //소환 버튼
        UtilityMethod.GetCustomTypeGMById(9).SetActive(true);

        // 트랜지션 아웃 ( black screen )
        yield return StartCoroutine(GlobalData.instance.effectManager.TransitionOut());

        // 백엔드 데이터 매니저 초기화
        StartCoroutine(backEndDataManager.Init());

        // 한 프레임 대기
        yield return new WaitForEndOfFrame();
        // Monster In Animation
        yield return StartCoroutine(player.currentMonster.inOutAnimator.AnimPositionIn());
        // 곤충 스폰 활성화 -> tutorial pattenr 10 에서 활성화
        // if (tutorialManager.isEndTutorial)
        //     insectSpwanManager.AllTimerStart();
        // else if (tutorialManager.GetTutorialSetById(3).isSetComplete)
        // {
        //     // 투토리얼 5번 세트 완료시 곤충 스폰 활성화
        //     insectSpwanManager.AllTimerStart();
        // }
        insectSpwanManager.AllTimerStart();

        // 공격 가능 상태로 전환
        attackController.SetAttackableState(true);

        // if (tutorialManager.isTutorial && tutorialManager.newUserEventPopupObj.activeSelf == false)
        // {
        //     tutorialManager.TutorialStart();
        // }
        // 버튼 가능 상태로 전환
        UtilityMethod.EnableUIEventSystem(true);
        //tutorialManager.TutorialStart();
        //firebaseManager.LogEvent("UserData","Stage",stageManager.stageData.stageId);

        // firebaseManager.LogEvent("Stage",
        // new Parameter(FirebaseAnalytics.ParameterValue, stageManager.stageData.stageId));
        // firebaseManager.LogEvent("EvolutionLevel",
        // new Parameter(FirebaseAnalytics.ParameterValue, saveDataManager.saveDataTotal.saveDataEvolution.level_evolution));
        // firebaseManager.LogEvent("TutorialStep",
        // new Parameter(FirebaseAnalytics.ParameterValue, saveDataManager.saveDataTotal.saveDataTutorial.tutorialSetId));

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
