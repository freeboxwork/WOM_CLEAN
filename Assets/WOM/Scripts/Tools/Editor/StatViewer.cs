using System.Collections;
using System.Collections.Generic;
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


    

    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("현재 스탯의 값의 확인 할 수 있습니다.");

        if (Application.isPlaying && GlobalData.instance.statManager!= null)
        {
            var insectBeeDamage = GlobalData.instance.statManager.GetInsectDamage(EnumDefinition.InsectType.bee);
            var insectBeetleDamage = GlobalData.instance.statManager.GetInsectDamage(EnumDefinition.InsectType.bee);
            var insectMentisDamage = GlobalData.instance.statManager.GetInsectDamage(EnumDefinition.InsectType.bee);

            var insectBeeSpeed = GlobalData.instance.statManager.GetInsectMoveSpeed(EnumDefinition.InsectType.bee);
            var insectBeetleSpeed = GlobalData.instance.statManager.GetInsectMoveSpeed(EnumDefinition.InsectType.bee);
            var insectMentisSpeed = GlobalData.instance.statManager.GetInsectMoveSpeed(EnumDefinition.InsectType.bee);

            EditorCustomGUI.GUI_Label(labelWidth, "Bee 공격력", insectBeeDamage.ToString());
            EditorCustomGUI.GUI_Label(labelWidth, "Beetle 공격력", insectBeetleDamage.ToString());
            EditorCustomGUI.GUI_Label(labelWidth, "Mentis 공격력", insectMentisDamage.ToString());

            EditorCustomGUI.GUI_Label(labelWidth, "Bee 이동 속도", insectBeeSpeed.ToString());
            EditorCustomGUI.GUI_Label(labelWidth, "Beetle 이동 속도", insectBeetleSpeed.ToString());
            EditorCustomGUI.GUI_Label(labelWidth, "Mentis 이동 속도", insectMentisSpeed.ToString());



        }
        else
        {
            GUILayout.Box("재생 중일때만 확인 가능 합니다.");
        }
        
    }

}
