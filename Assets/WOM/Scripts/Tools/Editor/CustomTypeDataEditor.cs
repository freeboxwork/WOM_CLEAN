using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(CustomTypeData))]
public class CustomTypeDataEditor : Editor
{

    CustomTypeData _target;


    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        _target = (CustomTypeData)target;

        GUILayout.Space(10);

        if (GUILayout.Button("Get My Components"))
        {
            _target.GetMyComponents();
        }

    }

 
}
