using UnityEngine;
using UnityEditor;

public class DungeonMonsterDataEditorWindow : EditorWindow
{
    private DungenMonsterFileData dungeonMonsterData;
    private DefaultAsset folder;
    private string fileName = "DungeonMonsterData.asset";

    [MenuItem("GM_TOOLS/DungeonMonsterData Editor")]
    public static void ShowWindow()
    {
        GetWindow<DungeonMonsterDataEditorWindow>("DungeonMonsterData Editor");
    }

    private void OnGUI()
    {
        EditorCustomGUI.GUI_Title("던전 몬스터 데이터를 생성 하거나 편집 합니다.");

        folder = (DefaultAsset)EditorGUILayout.ObjectField("Folder:", folder, typeof(DefaultAsset), false);
        fileName = EditorGUILayout.TextField("File Name:", fileName);

        string assetPath = AssetDatabase.GetAssetPath(folder) + "/" + fileName;

        dungeonMonsterData = (DungenMonsterFileData)EditorGUILayout.ObjectField("DungeonMonsterData:", dungeonMonsterData, typeof(DungenMonsterFileData), false);

        if (dungeonMonsterData != null)
        {
            EditorGUI.BeginChangeCheck();

            dungeonMonsterData.goodsType = (EnumDefinition.GoodsType)EditorGUILayout.EnumPopup("Goods Type:", dungeonMonsterData.goodsType);
            dungeonMonsterData.monsterType = (EnumDefinition.MonsterType)EditorGUILayout.EnumPopup("Monster Type:", dungeonMonsterData.monsterType);
            dungeonMonsterData.monsterFaceId = EditorGUILayout.IntField("Monster Face ID:", dungeonMonsterData.monsterFaceId);
            dungeonMonsterData.battleTime = EditorGUILayout.FloatField("Battle Time:", dungeonMonsterData.battleTime);
            dungeonMonsterData.maxKeyCount = EditorGUILayout.IntField("Max Key Count:", dungeonMonsterData.maxKeyCount);
            dungeonMonsterData.maxAdCount = EditorGUILayout.IntField("Max Ad Count:", dungeonMonsterData.maxAdCount);
            dungeonMonsterData.bgID = EditorGUILayout.IntField("Background ID:", dungeonMonsterData.bgID);

            if (EditorGUI.EndChangeCheck())
            {
                EditorUtility.SetDirty(dungeonMonsterData);
            }
        }
        else
        {
            GUILayout.Label("No DungeonMonsterData loaded", EditorStyles.boldLabel);
        }

        GUILayout.Space(10);

        GUILayout.BeginHorizontal();
        if (GUILayout.Button("Load"))
        {
            dungeonMonsterData = LoadDungeonMonsterData(assetPath);
        }

        if (GUILayout.Button("Save"))
        {
            SaveDungeonMonsterData(assetPath);
        }
        GUILayout.EndHorizontal();
    }

    private DungenMonsterFileData LoadDungeonMonsterData(string assetPath)
    {
        return AssetDatabase.LoadAssetAtPath<DungenMonsterFileData>(assetPath);
    }

    private void SaveDungeonMonsterData(string assetPath)
    {
        if (dungeonMonsterData == null)
        {
            dungeonMonsterData = CreateInstance<DungenMonsterFileData>();
        }

        if (!AssetDatabase.Contains(dungeonMonsterData))
        {
            AssetDatabase.CreateAsset(dungeonMonsterData, assetPath);
        }

        AssetDatabase.SaveAssets();
    }
}