using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

#if UNITY_EDITOR
using UnityEditor;
#endif

public static class EditorCustomGUI 
{
#if UNITY_EDITOR


    //EditorUtility.DisplayDialog("Messa")

    public static void GUI_Display_Dialog(string title, string message, string ok , string cancel)
    {
        if(cancel.Length > 0)
            EditorUtility.DisplayDialog(title, message, ok, cancel);
        else
            EditorUtility.DisplayDialog(title, message, ok);
    }
    public static void GUI_Title(string title)
    {

        GUILayout.BeginHorizontal("HelpBox");
        EditorGUILayout.HelpBox(title, MessageType.Warning);
        GUILayout.EndHorizontal();
    }


    public static void GUI_TitleFold(ref bool foldValue,string foldTitle,string title)
    {
        foldValue = EditorGUILayout.Foldout(foldValue, foldTitle);
        if (foldValue)
        {
            GUILayout.BeginHorizontal("HelpBox");
            EditorGUILayout.HelpBox(title, MessageType.Warning);
            GUILayout.EndHorizontal();
        }
    }

    public static void GUI_Button(string title, UnityAction action)
    {
        GUILayout.BeginHorizontal("HelpBox");
        if (GUILayout.Button(title))
        {
            action.Invoke();
        }
        GUILayout.EndHorizontal();
    }

    public static void GUI_TextFiled(float labelWidth, string label, ref string value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        value = EditorGUILayout.TextField(value);
        GUILayout.EndHorizontal();
    }

    public static void GUI_Label(float labelWidth, string label, string value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        GUILayout.Label(value);
        GUILayout.EndHorizontal();
    }

    public static void GUI_Toggle(float labelWidth, string label,ref bool value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        value = EditorGUILayout.Toggle(value);
        GUILayout.EndHorizontal();
    }

    public static void GUI_ENUM_POPUP<T>(float labelWidth, string label, ref T value) where T :  System.Enum
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        value = (T)EditorGUILayout.EnumPopup(value);
        GUILayout.EndHorizontal();
    }


    public static void GUI_IntFiled(float labelWidth, string label, ref int value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        value = EditorGUILayout.IntField(value);
        GUILayout.EndHorizontal();
    }

    public static void GUI_FloatFiled(float labelWidth, string label, ref float value)
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(label, "HelpBox", GUILayout.Width(labelWidth));
        value = EditorGUILayout.FloatField(value);
        GUILayout.EndHorizontal();
    }

    public static void GUI_TextArea(string label, ref string value)
    {
        GUILayout.BeginVertical();
        GUILayout.Label(label, "HelpBox");
        value = EditorGUILayout.TextArea(value);
        GUILayout.EndHorizontal();
    }

    public static void GUI_LineSpace(float space)
    {
        GUILayout.Space(space);
        GUILayout.Box("", "HelpBox", GUILayout.Height(2));
        GUILayout.Space(space);
    }
   

    public static void GUI_ObjectFiled_UI<T>(float labelWidth,string name, ref T value) where T : UnityEngine.Object
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(name, "HelpBox", GUILayout.Width((labelWidth)));
        value = (T)EditorGUILayout.ObjectField(value, typeof(T));
        GUILayout.EndHorizontal();
    }
    public static void GUI_ObjectFiled_UI<T>(float labelWidth, string name,  T value) where T : UnityEngine.Object
    {
        GUILayout.BeginHorizontal();
        GUILayout.Label(name, "HelpBox", GUILayout.Width((labelWidth)));
        value = (T)EditorGUILayout.ObjectField(value, typeof(T));
        GUILayout.EndHorizontal();
    }

    // GUI GOAL PATTERN


#endif

}
