using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.AnimatedValues;
using System;
using System.Linq;
using static EnumDefinition;

public class CustomTypeDataViewer : EditorWindow
{
    [MenuItem("GM_TOOLS/CustomTypeDataViewer")]
    public static void ShowWindow()
    {
        var window = GetWindow<CustomTypeDataViewer>();
        window.Show();
    }

    List<bool> enableTypes;
    List<Vector2> scrollViews;
    
    AnimBool tAnimBool;

    CustomTypeDataManager manager;
    
    private void OnEnable()
    {
        SetEnableTypes();
        tAnimBool = new AnimBool(false);
        tAnimBool.valueChanged.AddListener(Repaint);

        if(manager == null)
            manager = FindObjectOfType<CustomTypeDataManager>();    
    }

    void SetEnableTypes()
    {
        enableTypes = new List<bool>();
        scrollViews = new List<Vector2>();

        foreach(CustomDataType type in Enum.GetValues(typeof(CustomDataType)))
        {
            enableTypes.Add(false);
            scrollViews.Add(new Vector2());
        }
    }
    Vector2 scrollViwe;

    bool fold = false;
    private void OnGUI()
    {

        EditorCustomGUI.GUI_Title("씬에 배치된 모든 CUSTOM TYPE DATA Object를 볼 수 있습니다.");

        for (int i = 0; i < enableTypes.Count; i++)
        {
            GUILayout.BeginVertical("Box");

            var typeName = (CustomDataType)i;
            enableTypes[i] = EditorGUILayout.BeginFoldoutHeaderGroup(enableTypes[i], typeName.ToString().ToUpper());
            if (enableTypes[i])
            {
                scrollViews[i] = EditorGUILayout.BeginScrollView(scrollViews[i], "HelpBox");
                List<CustomTypeData> data = manager.GetCustomTypeDatas((CustomDataType)i).OrderBy(o => o.index).ToList();
                for (int j = 0; j < data.Count; j++)
                {
                    EditorCustomGUI.GUI_ObjectFiled_UI(120f, " INDEX : " + data[j].index, data[j]);
                }
                EditorGUILayout.EndScrollView();
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            GUILayout.EndHorizontal();
        }

    }

    //void FoldIn(AnimBool animValue)
    //{ 
    //    var value = EditorGUILayout.BeginFadeGroup(animValue.faded);
    //    if (value)
    //    {
            
    //        GUILayout.Button("TEST");
    //        GUILayout.Button("TEST");
    //        GUILayout.Button("TEST");
    //        GUILayout.Button("TEST");
    //        GUILayout.Button("TEST");
            
    //        GUILayout.EndScrollView();
            
    //    }
    //}


}
