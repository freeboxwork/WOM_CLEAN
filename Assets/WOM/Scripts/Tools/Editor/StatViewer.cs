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
    // ���� ��ȭ
    bool foldGoods = false;




    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("���� ������ ���� Ȯ�� �� �� �ֽ��ϴ�.");

        if (Application.isPlaying)
        {
            try
            {
                if (GlobalData.instance.statManager != null)
                {

                    scrollView = EditorGUILayout.BeginScrollView(scrollView);
                    GUILayout.BeginVertical("Box");
                    foldInsectStat = EditorGUILayout.BeginFoldoutHeaderGroup(foldInsectStat, "���� ����");
                    if (foldInsectStat) GUI_InsectStat();
                    EditorGUILayout.EndFoldoutHeaderGroup();
                    GUILayout.EndVertical();

                    GUILayout.BeginVertical("Box");
                    foldGoods = EditorGUILayout.BeginFoldoutHeaderGroup(foldGoods, "���� ��ȭ");
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
            GUILayout.Box("��� ���϶��� Ȯ�� ���� �մϴ�.");
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

        EditorCustomGUI.GUI_Label(labelWidth, "Bee ���ݷ�", insectBeeDamage.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Bee ũ��Ƽ�� ���ݷ�", insectBeeCriDamage.ToString());

        EditorCustomGUI.GUI_Label(labelWidth, "Beetle ���ݷ�", insectBeetleDamage.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Beetle ũ��Ƽ�� ���ݷ�", insectBeetleCriDamage.ToString());

        EditorCustomGUI.GUI_Label(labelWidth, "Mentis ���ݷ�", insectMentisDamage.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Mentis ũ��Ƽ�� ���ݷ�", insectMentisCriDamage.ToString());

        EditorCustomGUI.GUI_Label(labelWidth, "Bee �̵� �ӵ�", insectBeeSpeed.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Beetle �̵� �ӵ�", insectBeetleSpeed.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Mentis �̵� �ӵ�", insectMentisSpeed.ToString());
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

        EditorCustomGUI.GUI_Label(labelWidth, "���", gold.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "��", bone.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "�ֻ���", dice.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "��ź", coal.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "Ŭ���� Ƽ��", clearTicket.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "��� ���� ����", dungenKeyGold.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "�� ���� ����", dungenKeyBone.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "�ֻ��� ���� ����", dungenKeyDice.ToString());
        EditorCustomGUI.GUI_Label(labelWidth, "��ź ���� ����", dungenKeyCoal.ToString());
    }
}
