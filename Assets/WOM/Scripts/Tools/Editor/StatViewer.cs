using UnityEngine;
using UnityEditor;

public class StatViewer : EditorWindow
{

    float labelWidth = 200;


    [MenuItem("GM_TOOLS/STAT Viewer")]
    public static void ShowWindow()
    {
        var window = GetWindow<StatViewer>();
        window.Show();
    }

    Vector2 scrollView;
    bool foldInsectStat = false;
    // 보유 재화
    bool foldGoods = false;




    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("현재 스탯의 값의 확인 할 수 있습니다.");

        if (Application.isPlaying)
        {
            try
            {
                if (GlobalData.instance.statManager != null)
                {

                    scrollView = EditorGUILayout.BeginScrollView(scrollView);
                    GUILayout.BeginVertical("Box");
                    foldInsectStat = EditorGUILayout.BeginFoldoutHeaderGroup(foldInsectStat, "곤충 스탯");
                    if (foldInsectStat) GUI_InsectStat();
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("Box");
                    foldGoods = EditorGUILayout.BeginFoldoutHeaderGroup(foldGoods, "보유 재화");
                    if (foldGoods) GUI_PlayerGoods();
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    GUILayout.EndVertical();

                    EditorGUILayout.EndScrollView();

                }
            }
            catch
            {

            }

        }
        else
        {
            GUILayout.Box("재생 중일때만 확인 가능 합니다.");
        }
    }


    void GUI_InsectStat()
    {
        var insectBeeDamage = GlobalData.instance.statManager.GetInsectDamage(EnumDefinition.InsectType.bee);
        var insectBeeCriDamage = GlobalData.instance.statManager.GetInsectCriticalDamage(EnumDefinition.InsectType.bee);

        var insectBeetleDamage = GlobalData.instance.statManager.GetInsectDamage(EnumDefinition.InsectType.bee);
        var insectBeetleCriDamage = GlobalData.instance.statManager.GetInsectCriticalDamage(EnumDefinition.InsectType.bee);

        var insectMentisDamage = GlobalData.instance.statManager.GetInsectDamage(EnumDefinition.InsectType.bee);
        var insectMentisCriDamage = GlobalData.instance.statManager.GetInsectCriticalDamage(EnumDefinition.InsectType.bee);

        var insectBeeSpeed = GlobalData.instance.statManager.GetInsectMoveSpeed(EnumDefinition.InsectType.bee);
        var insectBeetleSpeed = GlobalData.instance.statManager.GetInsectMoveSpeed(EnumDefinition.InsectType.bee);
        var insectMentisSpeed = GlobalData.instance.statManager.GetInsectMoveSpeed(EnumDefinition.InsectType.bee);

        EditorCustomGUI.GUI_Label(labelWidth, "Bee 공격력", insectBeeDamage.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Bee 크리티컬 공격력", insectBeeCriDamage.ToString());

        EditorCustomGUI.GUI_Label(labelWidth, "Beetle 공격력", insectBeetleDamage.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Beetle 크리티컬 공격력", insectBeetleCriDamage.ToString());

        EditorCustomGUI.GUI_Label(labelWidth, "Mentis 공격력", insectMentisDamage.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Mentis 크리티컬 공격력", insectMentisCriDamage.ToString());

        EditorCustomGUI.GUI_Label(labelWidth, "Bee 이동 속도", insectBeeSpeed.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Beetle 이동 속도", insectBeetleSpeed.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Mentis 이동 속도", insectMentisSpeed.ToString());
    }

    void GUI_PlayerGoods()
    {
        var gold = GlobalData.instance.player.gold;
        var bone = GlobalData.instance.player.bone;
        var dice = GlobalData.instance.player.diceCount;
        var coal = GlobalData.instance.player.coal;
        var clearTicket = GlobalData.instance.player.clearTicket;
        var dungenKeyGold = GlobalData.instance.player.dungeonKeys[EnumDefinition.GoodsType.gold];
        var dungenKeyBone = GlobalData.instance.player.dungeonKeys[EnumDefinition.GoodsType.bone];
        var dungenKeyDice = GlobalData.instance.player.dungeonKeys[EnumDefinition.GoodsType.dice];
        var dungenKeyCoal = GlobalData.instance.player.dungeonKeys[EnumDefinition.GoodsType.coal];

        EditorCustomGUI.GUI_Label(labelWidth, "골드", gold.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "뼈", bone.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "주사위", dice.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "석탄", coal.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "클리어 티켓", clearTicket.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "골드 던전 열쇠", dungenKeyGold.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "뼈 던전 열쇠", dungenKeyBone.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "주사위 던전 열쇠", dungenKeyDice.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "석탄 던전 열쇠", dungenKeyCoal.ToString());
    }
}
