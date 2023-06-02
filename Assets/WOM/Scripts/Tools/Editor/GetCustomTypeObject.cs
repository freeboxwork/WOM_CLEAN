using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


public class GetCustomTypeObject : EditorWindow
{
    public EnumDefinition.CustomDataType dataType;
    public int customDataIndex;
    float labelWidth = 120;
         

    [MenuItem("GM_TOOLS/GetCustomTypeObject")]
    public static void ShowWindow()
    {
        var window = GetWindow<GetCustomTypeObject>();
        window.Show();
    }


    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("Scene 에 배치된 Custom Type Data를 찾아 줍니다.");

        GUILayout.BeginVertical("Box");
        EditorCustomGUI.GUI_ENUM_POPUP(labelWidth, "CustomDataType", ref dataType);
        EditorCustomGUI.GUI_IntFiled(labelWidth, "DATA INDEX", ref customDataIndex);
        EditorCustomGUI.GUI_Button("Get Custom Type Data", () => { GetCusTypeData(); });
        GUILayout.EndHorizontal();

    }

    void GetCusTypeData()
    {
        var datas = GetDatas();
        var isContains = datas.Any(a => a.customType == dataType && a.index == customDataIndex);
        if (isContains)
        {
            var select = datas.FirstOrDefault(a => a.customType == dataType && a.index == customDataIndex);
            Selection.activeGameObject = select.gameObject;
            EditorGUIUtility.PingObject(select.gameObject);
        }
        else
        {
            EditorCustomGUI.GUI_Display_Dialog("Message", "선택한 Custom Data Type 가 Scene 에 배치되어 있지 않습니다.", "확인", "");
        }
    }


    List<CustomTypeData> GetDatas()
    {
        List<CustomTypeData> objectsInScene = new List<CustomTypeData>();

        foreach (CustomTypeData go in Resources.FindObjectsOfTypeAll(typeof(CustomTypeData)) as CustomTypeData[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }
        return objectsInScene;
    }




}
