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

    int addGold;
    int addBone;
    int addGem;
    int addCoal;
    int addDice;

    int addClertTicekt;
    int addUnionTicket;
    int addDnaTicket;

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
    }

    [System.Obsolete]
    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("게임 테스트를 위한 툴 모음");
        scrollView = EditorGUILayout.BeginScrollView(scrollView);


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



        EditorGUILayout.EndScrollView();
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

    void GUI_AddGoodsCustom(string title, EnumDefinition.RewardType goodsType, ref int value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(title, GUILayout.Width(100));
        value = EditorGUILayout.IntField(value);
        if (GUILayout.Button("추가"))
        {
            AddGoods(goodsType, value);
        }
        GUILayout.EndHorizontal();
    }

    // RewardByType

    public void AddGoods(EnumDefinition.RewardType goodsType, int value)
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
