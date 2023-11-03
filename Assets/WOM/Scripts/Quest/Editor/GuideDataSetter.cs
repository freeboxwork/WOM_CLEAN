using UnityEditor;
using UnityEngine;
using System.Collections.Generic;
using System.Linq;


public class GuideDataSetter : EditorWindow
{

    Vector2 scrollPos;
    GuideObjectDataList dataList;
    public TextAsset textAsset;
    public List<Transform> allSceneObjects = new List<Transform>();

    [MenuItem("GM_TOOLS/GuideDataSetter")]
    public static void ShowEditorWindow()
    {
        GetWindow<GuideDataSetter>();
    }

    void OnGUI()
    {
        textAsset = (TextAsset)EditorGUILayout.ObjectField("TextAsset", textAsset, typeof(TextAsset), true);
        if (textAsset != null)
        {
            if (GUILayout.Button("Load Data"))
            {
                LoadData();
            }
        }
        if (dataList != null && dataList.data.Count > 0)
        {
            ShowDataList();
            GUILayout.BeginHorizontal();
            if (GUILayout.Button("Set Guide Pannel Data"))
            {
                SetGuidePannelData();
            }
            if (GUILayout.Button("Set Guide Button Data"))
            {
                SetGuideButtonData();
            }
            GUILayout.EndHorizontal();
        }


    }

    void ShowDataList()
    {
        scrollPos = EditorGUILayout.BeginScrollView(scrollPos);
        for (int i = 0; i < dataList.data.Count; i++)
        {

            EditorGUILayout.BeginHorizontal("Helpbox");
            EditorGUILayout.LabelField(dataList.data[i].id.ToString(), "Box", GUILayout.Width(30));
            GUI.color = IsSceneContainsObjectByName(dataList.data[i].objectName) ? Color.green : Color.red;
            EditorGUILayout.LabelField(dataList.data[i].objectName);
            GUI.color = Color.white;
            if (GUILayout.Button("Sel"))
            {
                var obj = GetObjectByName(dataList.data[i].objectName);
                Selection.activeObject = obj;
                EditorGUIUtility.PingObject(obj);
            }
            EditorGUILayout.EndHorizontal();
        }
        EditorGUILayout.EndScrollView();


    }

    bool IsSceneContainsObjectByName(string name)
    {
        return allSceneObjects.Any(a => a.name == name);
    }
    Transform GetObjectByName(string name)
    {
        return allSceneObjects.FirstOrDefault(a => a.name == name);
    }

    void LoadData()
    {
        allSceneObjects.Clear();
        allSceneObjects = Resources.FindObjectsOfTypeAll<Transform>().Where(a => !AssetDatabase.Contains(a.gameObject)).ToList();

        dataList = JsonUtility.FromJson<GuideObjectDataList>(textAsset.text);
    }

    void SetGuidePannelData()
    {
        foreach (var data in dataList.data)
        {
            var obj = GetObjectByName(data.objectName);
            if (obj != null)
            {
                var guidePannelData = obj.GetComponent<GuidePannelData>();
                if (guidePannelData == null)
                {
                    guidePannelData = obj.gameObject.AddComponent<GuidePannelData>();
                }
                guidePannelData.id = data.id;
                guidePannelData.objectName = data.objectName;
            }
        }
    }

    void SetGuideButtonData()
    {
        foreach (var data in dataList.data)
        {
            var obj = GetObjectByName(data.objectName);
            if (obj != null)
            {
                var guideButtonData = obj.GetComponent<GuideButtonData>();
                if (guideButtonData == null)
                {
                    guideButtonData = obj.gameObject.AddComponent<GuideButtonData>();
                }
                guideButtonData.id = data.id;
                guideButtonData.objectName = data.objectName;
            }
        }
    }

}

public class GuideObjectDataList
{
    public List<GuideObjectData> data;
}

[System.Serializable]
public class GuideObjectData
{
    public int id;
    public string objectName;
}
