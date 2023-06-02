using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using UnityEditor;
using UnityEditor.UIElements;

public class CustomUI_Toolkit : EditorWindow
{
    [SerializeField]
    VisualTreeAsset _visualTreeAsset;

    ObjectField _objectField;
    Button _button;

    [MenuItem("Tools/UI_ToolKit_TEST")]
    public static void ShowWindow()
    {
        var window = GetWindow( typeof(CustomUI_Toolkit));
        window.Show();
    }


    private void CreateGUI()
    {
        _visualTreeAsset.CloneTree(rootVisualElement);
        _objectField = rootVisualElement.Q<ObjectField>("_obj");
        _button = rootVisualElement.Q<Button>("_btn");

        _button.clickable.clicked += Click;
    }

    void Click()
    {
        Debug.Log("btn click");
    }

}
