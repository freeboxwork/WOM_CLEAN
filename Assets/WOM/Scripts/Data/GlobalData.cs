using ProjectGraphics;
using UnityEngine;
using UnityEngine.EventSystems;

public class GlobalData : MonoBehaviour
{

    public static GlobalData instance;
    public DataManager dataManager;
    public InsectManager insectManager;
    public MonsterManager monsterManager;
    public AttackController attackController;
    public CustomTypeDataManager customTypeDataManager;
    public EventController eventController;
    public UiController uiController;
    public Player player;
    public EffectManager effectManager;
    public BossMonsterChallengeTimer bossChallengeTimer;
    public StageManager stageManager;
    public LotteryManager lotteryManager;
    public SaleManager saleManager;
    public EvolutionManager evolutionManager;
    public SkillManager skillManager;
    public UnionManager unionManager;
    public DNAManager dnaManger;
    public GradeAnimationController gradeAnimCont;
    public GlobalPopupController globalPopupController;
    public EvolutionDiceLotteryManager evolutionDiceLotteryManager;
    public UnionInfoPopupController unionInfoPopupController;
    public TraningManager traningManager;
    public ShopManager shopManager;
    public UnionSpwanManager unionSpwanManager;
    public InsectSpwanManager insectSpwanManager;
    public StatManager statManager;
    public SaveDataManager saveDataManager;
    public SettingPopupController settingPopupController;
    public SoundManager soundManager;
    public DungeonPopup dungeonPopup;
    public DungeonManager dungeonManager;
    public DungeonEnterPopup dungeonEnterPopup;
    public CastleManager castleManager;
    public EventSystem eventSystem;
    public QuestManager questManager;
    public RewardManager rewardManager;


    private void Awake()
    {
        SetInstance();
    }

    void Start()
    {

    }

    void SetInstance()
    {
        if (instance != null) Destroy(instance);
        instance = this;
    }



}
