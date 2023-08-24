using UnityEngine;
using UnityEditor;
using ProjectGraphics;

public class MonsterSkinDataMaker : EditorWindow
{

    TextAsset monsterSkinDataJson;
    MonsterSprites monSkinDatas;
    float labelWidth = 80f;

    int curSkinID = 0;

    PartsChangeTest partsChanger;


    private void OnEnable()
    {
        partsChanger = FindObjectOfType<PartsChangeTest>();
    }


    [MenuItem("GM_TOOLS/MonsterSkinDataMaker")]
    private static void ShowWindow()
    {
        var window = GetWindow<MonsterSkinDataMaker>();
        window.titleContent = new GUIContent("MonsterSkinDataMaker");
        window.Show();
    }


    private void OnGUI()
    {
        EditorCustomGUI.GUI_ObjectFiled_UI(labelWidth, "MonsterSkinDataJson", ref monsterSkinDataJson);
        EditorCustomGUI.GUI_Button("Load", () =>
        {
            monSkinDatas = GetData<MonsterSprites>(monsterSkinDataJson.text);
        });
        EditorCustomGUI.GUI_Button("Save", () =>
        {
            // var json = JsonUtility.ToJson(monSkinDatas);
            // Debug.Log(json);
        });

        if (monSkinDatas != null && monSkinDatas.data.Count > 0)
        {
            GUI_SkinDataView();
        }



    }


    void GUI_SkinDataView()
    {
        GUILayout.BeginVertical("box");

        // id change
        GUILayout.BeginHorizontal();
        GUILayout.Label("SkinID", GUILayout.Width(labelWidth));
        curSkinID = EditorGUILayout.IntField(curSkinID);
        if (GUILayout.Button("Perv"))
        {
            curSkinID--;
            SetData();
        }
        if (GUILayout.Button("Next"))
        {
            curSkinID++;
            SetData();
        }

        GUILayout.EndHorizontal();
        var data = monSkinDatas.data[curSkinID];
        GUI_PartsView("Tail", ref data.tail);
        GUI_PartsView("Hand", ref data.hand);
        GUI_PartsView("finger", ref data.finger);
        GUI_PartsView("forArm", ref data.foreArm);
        GUI_PartsView("upperArm", ref data.upperArm);
        GUI_PartsView("head", ref data.head);
        GUI_PartsView("body", ref data.body);
        GUI_PartsView("leg0", ref data.leg_0);
        GUI_PartsView("leg1", ref data.leg_1);
        GUI_PartsView("leg2", ref data.leg_2);
        GUILayout.EndVertical();
    }




    void GUI_PartsView(string name, ref int id)
    {
        GUILayout.BeginHorizontal("box");
        GUILayout.Label(name, GUILayout.Width(labelWidth));
        id = EditorGUILayout.IntField(id);
        if (GUILayout.Button("Perv"))
        {
            id--;
            SetData();
        }
        if (GUILayout.Button("Next"))
        {
            id++;
            SetData();
        }
        GUILayout.EndHorizontal();
    }

    void SetData()
    {
        partsChanger.SkinChange(monSkinDatas.data[curSkinID]);
    }

    T GetData<T>(string text)
    {

        return JsonUtility.FromJson<T>(text);
    }

}
