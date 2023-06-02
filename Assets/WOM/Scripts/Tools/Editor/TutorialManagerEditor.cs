using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;

[CustomEditor(typeof(TutorialManager))]
public class TutorialManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        TutorialManager _target = (TutorialManager)target;
        GUILayout.Space(10);
        if (GUILayout.Button("Get All Custom Type Objects In Scene"))
        {
            GetAllTypeObjects(_target);
        }
    }

    void GetAllTypeObjects(TutorialManager target)
    {
        target.tutorialButtons.Clear();

        List<TutorialButton> objectsInScene = new List<TutorialButton>();

        foreach (TutorialButton go in Resources.FindObjectsOfTypeAll(typeof(TutorialButton)) as TutorialButton[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
            {
                go.GetButton();
                objectsInScene.Add(go);
            }
        }

        target.tutorialButtons = objectsInScene.ToList();
    }

}
