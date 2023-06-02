using UnityEngine;
using UnityEditor;
using static EnumDefinition;

public class GM_TesterTools : EditorWindow
{
    Transform t;
    Vector2 scrollView;

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

    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("게임 테스트를 위한 툴 모음");

        scrollView = EditorGUILayout.BeginScrollView(scrollView);
        EditorCustomGUI.GUI_Button("골드 10000 추가", () => { GlobalData.instance.player.AddGold(1000); });
        EditorCustomGUI.GUI_Button("뼈조각 10000 추가", () => { GlobalData.instance.player.AddBone(1000); });
        EditorCustomGUI.GUI_Button("보석 10000 추가", () => { GlobalData.instance.player.AddGem(1000); });
        EditorCustomGUI.GUI_Button("주사위 10000 추가", () => { GlobalData.instance.player.AddDice(1000); });

        EditorCustomGUI.GUI_Button("던전 몬스터 키 - 골드 100 추가", () => { GlobalData.instance.player.AddDungeonKey(GoodsType.gold, 100); });
        EditorCustomGUI.GUI_Button("던전 몬스터 키 - 뼈조각 100 추가", () => { GlobalData.instance.player.AddDungeonKey(GoodsType.bone, 100); });
        EditorCustomGUI.GUI_Button("던전 몬스터 키 - 주사위  100 추가", () => { GlobalData.instance.player.AddDungeonKey(GoodsType.dice, 100); });
        EditorCustomGUI.GUI_Button("던전 몬스터 키 - 석탄  100 추가", () => { GlobalData.instance.player.AddDungeonKey(GoodsType.coal, 100); });

        EditorCustomGUI.GUI_Button("몬스터 즉시 사냥", () => { KillMonster(); });

        EditorCustomGUI.GUI_Button("퀘스트 일일 타이머 리셋", () => { PlayerPrefs.DeleteKey("current_time"); PlayerPrefs.DeleteKey("midnight_time"); });

        EditorCustomGUI.GUI_Button("저장된 모든 PlayerPrefs 지우기", () => { PlayerPrefs.DeleteAll(); });



        EditorGUILayout.EndScrollView();
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

}
