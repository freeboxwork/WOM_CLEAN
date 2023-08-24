using UnityEngine;
using UnityEditor;
using System.IO;
using ProjectGraphics;
using System.Threading.Tasks;

public class MonsterSkinDataMaker : EditorWindow
{

    TextAsset monsterSkinDataJson;
    MonsterSprites monSkinDatas;
    float labelWidth = 80f;

    int curSkinID = 0;

    PartsChangeTest partsChanger;
    private float minValue = 0f;
    private float maxValue = 32f;
    private float minLimit = 0f;
    private float maxLimit = 32;

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
            SaveDataFile();
        });

        if (monSkinDatas != null && monSkinDatas.data.Count > 0)
        {
            GUI_SkinDataView();
        }

    }

    async void SaveDataFile()
    {
        var json = JsonUtility.ToJson(monSkinDatas);
        var filePath = Application.dataPath + "/CaptureImage/SkinDataBoss.json";

        await Task.Yield();

        File.WriteAllText(Application.dataPath + "/CaptureImage/SkinDataBoss.json", json);
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
        GUILayout.BeginVertical("box");
        EditorGUILayout.MinMaxSlider("RandomRange", ref minValue, ref maxValue, minLimit, maxLimit);
        EditorCustomGUI.GUI_Button("Random Parts", () =>
        {
            RandomParts();
            SetData();
        });
        EditorGUILayout.LabelField("Min Value: " + (int)minValue);
        EditorGUILayout.LabelField("Max Value: " + (int)maxValue);

        GUILayout.EndVertical();
    }

    void RandomParts()
    {
        var data = monSkinDatas.data[curSkinID];
        data.tail = GetRandomPartsNumber();
        data.hand = GetRandomPartsNumber();
        data.finger = GetRandomPartsNumber();
        data.foreArm = GetRandomPartsNumber();
        data.upperArm = GetRandomPartsNumber();
        data.head = GetRandomPartsNumber();
        data.body = GetRandomPartsNumber();
        data.leg_0 = GetRandomPartsNumber();
        data.leg_1 = GetRandomPartsNumber();
        data.leg_2 = GetRandomPartsNumber();
    }

    int GetRandomPartsNumber()
    {
        return Random.Range((int)minValue, (int)maxValue);
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
        if (partsChanger == null) partsChanger = FindObjectOfType<PartsChangeTest>();
        partsChanger.SkinChange(monSkinDatas.data[curSkinID]);
    }

    T GetData<T>(string text)
    {

        return JsonUtility.FromJson<T>(text);
    }

}
