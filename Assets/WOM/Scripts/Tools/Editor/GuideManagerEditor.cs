using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(GuideManager))]
public class GuideManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GuideManager _target = (GuideManager)target;
        GUILayout.Space(10);
        if (GUILayout.Button("Get All Custom Type Objects In Scene\n순서 마춰서 정렬해야함"))
        {
            GetAllTypeObjects(_target);
        }
    }

    void GetAllTypeObjects(GuideManager target)
    {
        target.guideButtons.Clear();

        List<GuideButton> objectsInScene = new List<GuideButton>();

        foreach (GuideButton go in Resources.FindObjectsOfTypeAll(typeof(GuideButton)) as GuideButton[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
            {
                go.GetButton();
                objectsInScene.Add(go);
            }
        }

        target.guideButtons = objectsInScene.ToList();
    }

}