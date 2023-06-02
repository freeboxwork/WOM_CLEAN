using UnityEditor;
using UnityEngine;

[CustomEditor(typeof(DungeonMonster))]
public class DungeonMonsterEditor : Editor
{

    public override void OnInspectorGUI()
    {
      

        DungeonMonster dungenMonster = (DungeonMonster)target;
        if (dungenMonster.curMonsterData != null)
        {
            DrawUILine(Color.black);


            // Display and edit member variables

            dungenMonster.curMonsterData = (DungenMonsterFileData)EditorGUILayout.ObjectField("Monster Data",dungenMonster.curMonsterData, typeof(DungenMonsterFileData));
            dungenMonster.curMonsterData.goodsType = (EnumDefinition.GoodsType)EditorGUILayout.EnumPopup("Goods Type", dungenMonster.curMonsterData.goodsType);
            dungenMonster.curMonsterData.monsterType = (EnumDefinition.MonsterType)EditorGUILayout.EnumPopup("Monster Type", dungenMonster.curMonsterData.monsterType);
            dungenMonster.curMonsterData.monsterFaceId = EditorGUILayout.IntField("Monster Face ID", dungenMonster.curMonsterData.monsterFaceId);
            dungenMonster.curMonsterData.battleTime = EditorGUILayout.FloatField("Battle Time", dungenMonster.curMonsterData.battleTime);
            dungenMonster.curMonsterData.maxKeyCount = EditorGUILayout.IntField("Max Key Count", dungenMonster.curMonsterData.maxKeyCount);
            dungenMonster.curMonsterData.maxAdCount = EditorGUILayout.IntField("Max Ad Count", dungenMonster.curMonsterData.maxAdCount);
            dungenMonster.curMonsterData.bgID = EditorGUILayout.IntField("BG ID", dungenMonster.curMonsterData.bgID);



            // Save changes to the ScriptableObject
            if (GUI.changed)
            {
                EditorUtility.SetDirty(dungenMonster.curMonsterData);
            }

            DrawUILine(Color.black);

            base.OnInspectorGUI();
        }
         

        
    }
    private void DrawUILine(Color color, int thickness = 2, int padding = 10)
    {
        Rect r = EditorGUILayout.GetControlRect(GUILayout.Height(padding + thickness));
        r.height = thickness;
        r.y += padding / 2;
        r.x -= 2; // Optional offset to match default EditorGUI fields
        r.width += 6; // Optional offset to match default EditorGUI fields
        EditorGUI.DrawRect(r, color);
    }
}
