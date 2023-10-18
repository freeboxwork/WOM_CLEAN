using UnityEngine;
using UnityEditor;

using static EnumDefinition;

public class GM_TesterTools : EditorWindow
{
    Transform t;
    Vector2 scrollView;
    const string dataFileName = "saveData.json";

    bool goodsFold = false;
    bool otherFold = false;
    bool saveDataFold = false;

    bool customLevelingFold = false;

    bool tutorialFold = false;
    //bool skillFold = false;


    float addGold;
    float addBone;
    float addGem;
    float addCoal;
    float addDice;

    float addClertTicekt;
    float addUnionTicket;
    float addDnaTicket;
    float addTimeScaleValue;

    float insectAutoEnableTime = 0.1f;

    [MenuItem("GM_TOOLS/TesterTools")]
    public static void ShowWindwo()
    {
        var window = GetWindow<GM_TesterTools>();
        window.Show();
    }

    private void OnEnable()
    {
        var tg = GameObject.Find("TesterTransform");
        if (tg == null)
        {
            tg = new GameObject("TesterTransform");
        }
        t = tg.transform;

        tutorialManager = FindObjectOfType<TutorialManager>();
    }

    [System.Obsolete]
    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("게임 테스트를 위한 툴 모음");
        scrollView = EditorGUILayout.BeginScrollView(scrollView);

        addTimeScaleValue = EditorGUILayout.FloatField("타임 스케일", addTimeScaleValue);
        if (GUILayout.Button("타임 스케일 적용"))
        {
            Time.timeScale = addTimeScaleValue;
        }

        GUILayout.BeginVertical("Box");
        goodsFold = EditorGUILayout.BeginFoldoutHeaderGroup(goodsFold, "GOODS AND ITEM");
        if (goodsFold)
        {
            if (Application.isPlaying)
            {
                GUI_AddGoodsCustom("골드", RewardType.gold, ref addGold);
                GUI_AddGoodsCustom("뼈조각", RewardType.bone, ref addBone);
                GUI_AddGoodsCustom("보석", RewardType.gem, ref addGem);
                GUI_AddGoodsCustom("석탄", RewardType.coal, ref addCoal);
                GUI_AddGoodsCustom("주사위", RewardType.dice, ref addDice);
                GUI_AddGoodsCustom("클리어티켓", RewardType.clearTicket, ref addClertTicekt);
                GUI_AddGoodsCustom("유니온티켓", RewardType.unionTicket, ref addUnionTicket);
                GUI_AddGoodsCustom("유전자티켓", RewardType.dnaTicket, ref addDnaTicket);


                EditorCustomGUI.GUI_Button("골드 10000 추가", () => { GlobalData.instance.player.AddGold(1000); });
                EditorCustomGUI.GUI_Button("뼈조각 10000 추가", () => { GlobalData.instance.player.AddBone(1000); });
                EditorCustomGUI.GUI_Button("보석 10000 추가", () => { GlobalData.instance.player.AddGem(1000); });
                EditorCustomGUI.GUI_Button("주사위 10000 추가", () => { GlobalData.instance.player.AddDice(1000); });

                EditorCustomGUI.GUI_Button("던전 몬스터 키 - 골드 100 추가", () => { GlobalData.instance.player.AddDungeonKey(GoodsType.gold, 100); });
                EditorCustomGUI.GUI_Button("던전 몬스터 키 - 뼈조각 100 추가", () => { GlobalData.instance.player.AddDungeonKey(GoodsType.bone, 100); });
                EditorCustomGUI.GUI_Button("던전 몬스터 키 - 주사위  100 추가", () => { GlobalData.instance.player.AddDungeonKey(GoodsType.dice, 100); });
                EditorCustomGUI.GUI_Button("던전 몬스터 키 - 석탄  100 추가", () => { GlobalData.instance.player.AddDungeonKey(GoodsType.coal, 100); });
            }
            else
            {
                GUILayout.Box("플레이 모드에서만 사용 가능합니다.");
            }

        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        GUILayout.EndVertical();


        GUILayout.BeginVertical("Box");
        saveDataFold = EditorGUILayout.BeginFoldoutHeaderGroup(saveDataFold, "SAVE DATA");
        if (saveDataFold)
        {
            EditorCustomGUI.GUI_Button("저장된 모든 PlayerPrefs 지우기", () => { PlayerPrefs.DeleteAll(); });
            EditorCustomGUI.GUI_Button("저장된 모든 SAVE DATA 지우기", () => { DeleteSaveDataFile(); });
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        GUILayout.EndVertical();


        GUILayout.BeginVertical("Box");
        otherFold = EditorGUILayout.BeginFoldoutHeaderGroup(otherFold, "OTHER");
        if (otherFold)
        {

            if (Application.isPlaying)
            {
                GUI_AutoEnableInsect();
            }
            else
            {
                GUILayout.Box("자동으로 곤충 활성화는 플레이 모드에서만 사용 가능합니다.");
            }

            EditorCustomGUI.GUI_Button("몬스터 즉시 사냥", () => { KillMonster(); });
            EditorCustomGUI.GUI_Button("퀘스트 일일 타이머 리셋", () => { PlayerPrefs.DeleteKey("current_time"); PlayerPrefs.DeleteKey("midnight_time"); });
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        GUILayout.EndVertical();



        GUILayout.BeginVertical("Box");
        customLevelingFold = EditorGUILayout.BeginFoldoutHeaderGroup(customLevelingFold, "CUSTOM LEVELING");
        GUILayout.BeginVertical("Box");
        if (customLevelingFold)
        {
            GUI_CustomLeveling();
        }
        GUILayout.EndVertical();
        EditorGUILayout.EndFoldoutHeaderGroup();
        GUILayout.EndVertical();

        // tutorial fold
        GUILayout.BeginVertical("Box");
        tutorialFold = EditorGUILayout.BeginFoldoutHeaderGroup(tutorialFold, "TUTORIAL");
        if (tutorialFold)
        {
            GUI_Tutorial();
        }
        EditorGUILayout.EndFoldoutHeaderGroup();
        GUILayout.EndVertical();

        // // kill fold
        // GUILayout.BeginVertical("Box");
        // skillFold = EditorGUILayout.BeginFoldoutHeaderGroup(skillFold, "SKILL");
        // if (skillFold)
        // {
        //     GUI_Skill();
        // }
        // EditorGUILayout.EndFoldoutHeaderGroup();
        // GUILayout.EndVertical();

        EditorGUILayout.EndScrollView();
    }

    // void GUI_Skill()
    // {
    //     foreach (EnumDefinition.SkillType type in System.Enum.GetValues(typeof(EnumDefinition.SkillType)))
    //     {
    //         GUILayout.BeginHorizontal();
    //         EditorCustomGUI.GUI_Label(levelWidth, type.ToString(), "");
    //         if (GUILayout.Button(" 스킬 쿨타임 스킵"))
    //         {
    //             GlobalData.instance.skillManager.SkillCoolTimeSkipByType(type);
    //         }
    //         GUILayout.EndHorizontal();
    //     }
    // }

    TutorialManager tutorialManager;


    void GUI_Tutorial()
    {
        // try
        // {
        //     if (tutorialManager != null)
        //         tutorialManager.disableTutorial = EditorGUILayout.Toggle("튜토리얼 비활성화", tutorialManager.disableTutorial);
        //     else
        //         tutorialManager = FindObjectOfType<TutorialManager>();
        // }
        // catch
        // {

        // }

    }

    #region  CustomLeveling
    bool customLevelTraining = false;
    bool customLevelCamp = false;
    bool customLevelDNA = false;
    int campLevel;
    int trainingDamageLevel = 0;
    int trainingCriticalChanceLevel = 0;
    int trainingCriticalDamage = 0;
    int talentDamage = 0;
    int talentCriticalChance = 0;
    int talentCriticalDamage = 0;
    int talentMoveSpeed = 0;
    int talentSpawnSpeed = 0;
    int talentGoldBonus = 0;
    float levelWidth = 160f;

    int insectDamage = 0;
    int insectCriticalChance = 0;
    int insectCriticalDamage = 0;
    int unionDamage = 0;
    int glodBonus = 0;
    int insectMoveSpeed = 0;
    int unionMoveSpeed = 0;
    int unionSpawnTime = 0;
    int goldPig = 0;
    int skillDuration = 0;
    int skillCoolTime = 0;
    int bossDamage = 0;
    int monsterHpLess = 0;
    int boneBonus = 0;
    int goldMonsterBonus = 0;
    int offlineBonus = 0;
    #endregion

    ref int GetCustomLevel(EnumDefinition.SaleStatType stat)
    {

        switch (stat)
        {
            case EnumDefinition.SaleStatType.trainingDamage:
                return ref trainingDamageLevel;
            case EnumDefinition.SaleStatType.trainingCriticalChance:
                return ref trainingCriticalChanceLevel;
            case EnumDefinition.SaleStatType.trainingCriticalDamage:
                return ref trainingCriticalDamage;
            case EnumDefinition.SaleStatType.talentDamage:
                return ref talentDamage;
            case EnumDefinition.SaleStatType.talentCriticalChance:
                return ref talentCriticalChance;
            case EnumDefinition.SaleStatType.talentCriticalDamage:
                return ref talentCriticalDamage;
            case EnumDefinition.SaleStatType.talentMoveSpeed:
                return ref talentMoveSpeed;
            case EnumDefinition.SaleStatType.talentSpawnSpeed:
                return ref talentSpawnSpeed;
            case EnumDefinition.SaleStatType.talentGoldBonus:
                return ref talentGoldBonus;
            default:
                return ref trainingDamageLevel;
        }
    }

    ref int GetCustomLevelByDnaType(EnumDefinition.DNAType type)
    {
        switch (type)
        {
            case EnumDefinition.DNAType.insectDamage:
                return ref insectDamage;
            case EnumDefinition.DNAType.insectCriticalChance:
                return ref insectCriticalChance;
            case EnumDefinition.DNAType.insectCriticalDamage:
                return ref insectCriticalDamage;
            case EnumDefinition.DNAType.unionDamage:
                return ref unionDamage;
            case EnumDefinition.DNAType.glodBonus:
                return ref glodBonus;
            case EnumDefinition.DNAType.insectMoveSpeed:
                return ref insectMoveSpeed;
            case EnumDefinition.DNAType.unionMoveSpeed:
                return ref unionMoveSpeed;
            case EnumDefinition.DNAType.unionSpawnTime:
                return ref unionSpawnTime;
            case EnumDefinition.DNAType.goldPig:
                return ref goldPig;
            case EnumDefinition.DNAType.skillDuration:
                return ref skillDuration;
            case EnumDefinition.DNAType.skillCoolTime:
                return ref skillCoolTime;
            case EnumDefinition.DNAType.bossDamage:
                return ref bossDamage;
            case EnumDefinition.DNAType.monsterHpLess:
                return ref monsterHpLess;
            case EnumDefinition.DNAType.boneBonus:
                return ref boneBonus;
            case EnumDefinition.DNAType.goldMonsterBonus:
                return ref goldMonsterBonus;
            case EnumDefinition.DNAType.offlineBonus:
                return ref offlineBonus;
            default:
                return ref insectDamage;
        }
    }

    void GUI_CustomLeveling()
    {
        customLevelTraining = EditorGUILayout.Toggle("훈련 레벨 조정 ", customLevelTraining);
        if (customLevelTraining)
        {
            GUI_CustomLeveling_Training();
        }

        customLevelCamp = EditorGUILayout.Toggle("캠프 레벨 조정 ", customLevelCamp);
        if (customLevelCamp)
        {
            GUI_CustomLeveling_Camp();
        }

        customLevelDNA = EditorGUILayout.Toggle("DNA 레벨 조정 ", customLevelDNA);
        if (customLevelDNA)
        {
            GUI_CustomLevel_DNA();
        }

    }

    void GUI_CustomLevel_DNA()
    {
        foreach (EnumDefinition.DNAType type in System.Enum.GetValues(typeof(EnumDefinition.DNAType)))
        {
            GUILayout.BeginHorizontal();
            EditorCustomGUI.GUI_IntFiled(levelWidth, type.ToString(), ref GetCustomLevelByDnaType(type));
            if (GUILayout.Button("레벨 적용"))
            {
                GlobalData.instance.dnaManger.SetCustomLevel(type, GetCustomLevelByDnaType(type));
            }
            GUILayout.EndHorizontal();
        }
    }

    void GUI_CustomLeveling_Camp()
    {
        GUILayout.BeginHorizontal();
        EditorCustomGUI.GUI_IntFiled(levelWidth, "Camp Level", ref campLevel);
        if (GUILayout.Button("레벨 적용"))
        {
            GlobalData.instance.lotteryManager.CustomLevelUp(campLevel);
        }
        GUILayout.EndHorizontal();
    }

    void GUI_CustomLeveling_Training()
    {
        foreach (EnumDefinition.SaleStatType stat in System.Enum.GetValues(typeof(EnumDefinition.SaleStatType)))
        {
            GUILayout.BeginHorizontal();
            EditorCustomGUI.GUI_IntFiled(levelWidth, stat.ToString(), ref GetCustomLevel(stat));
            if (GUILayout.Button("레벨 적용"))
            {
                GlobalData.instance.traningManager.SetCustomLevel(stat, GetCustomLevel(stat));
            }
            GUILayout.EndHorizontal();
        }
    }


    [System.Obsolete]
    void GUI_AutoEnableInsect()
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label("곤충 자동생성 간격");
        insectAutoEnableTime = EditorGUILayout.FloatField(insectAutoEnableTime);
        GUILayout.EndHorizontal();

        EditorCustomGUI.GUI_Button("자동으로 곤충 활성화", () =>
        {
            GlobalData.instance.attackController.TestInsectAotoEnable(insectAutoEnableTime);
        });

        EditorCustomGUI.GUI_Button("자동 곤충 활성화 멈춤 ", () =>
        {
            GlobalData.instance.attackController.StopTestInsectAotoEnable();
        });

    }

    void GUI_AddGoodsCustom(string title, EnumDefinition.RewardType goodsType, ref float value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(title, GUILayout.Width(100));
        value = EditorGUILayout.FloatField(value);
        if (GUILayout.Button("추가"))
        {
            AddGoods(goodsType, value);
        }
        GUILayout.EndHorizontal();
    }

    // RewardByType

    public void AddGoods(EnumDefinition.RewardType goodsType, float value)
    {
        GlobalData.instance.rewardManager.RewardByType(goodsType, value);
    }

    void KillMonster()
    {
        var player = GlobalData.instance.player;
        player.currentMonster.hp = 0;
        EventManager.instance.RunEvent(CallBackEventType.TYPES.OnMonsterHit, EnumDefinition.InsectType.bee, 0, t);
    }

    void DeletePlayerPrefsByKey(string key)
    {
        PlayerPrefs.DeleteKey(key);
    }

    void DeleteSaveDataFile()
    {
        System.IO.File.Delete(GetSaveDataFilePaht());
    }

    string GetSaveDataFilePaht()
    {
        string path = "";
#if UNITY_EDITOR
        path = Application.dataPath + "/" + dataFileName;
#elif UNITY_ANDROID
            path = Application.persistentDataPath + "/"+ dataFileName;
#endif
        return path;
    }

}
