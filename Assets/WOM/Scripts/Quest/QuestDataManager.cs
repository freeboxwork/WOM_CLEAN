using System.Collections.Generic;
using UnityEngine;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuestDataManager : MonoBehaviour
{
    public List<GuidePannelData> guidePannelDatas = new List<GuidePannelData>();
    public List<GuideButtonData> guideButtonDatas = new List<GuideButtonData>();

    void Start()
    {

    }

    public GuidePannelData GetGuidePannelData(int id)
    {
        return guidePannelDatas.Where(a => a.id == id).FirstOrDefault();
    }

    public GuideButtonData GetGuideButtonData(int id)
    {
        return guideButtonDatas.Where(a => a.id == id).FirstOrDefault();
    }

}


#if UNITY_EDITOR
[CustomEditor(typeof(QuestDataManager))]
public class QuestDataManagerEditor : Editor
{
    QuestDataManager _target;
    public override void OnInspectorGUI()
    {
        _target = (QuestDataManager)target;
        base.OnInspectorGUI();

        if (GUILayout.Button("Get Guide Pannel Data"))
        {
            GetGuidePannelData(_target);
        }

        if (GUILayout.Button("Get Guide Button Data"))
        {
            GetGuideButtonData(_target);
        }
    }

    void GetGuidePannelData(QuestDataManager manager)
    {
        manager.guidePannelDatas.Clear();
        manager.guidePannelDatas = Resources.FindObjectsOfTypeAll<GuidePannelData>().Where(a => !AssetDatabase.Contains(a.gameObject)).ToList();
    }

    void GetGuideButtonData(QuestDataManager manager)
    {
        manager.guideButtonDatas.Clear();
        manager.guideButtonDatas = Resources.FindObjectsOfTypeAll<GuideButtonData>().Where(a => !AssetDatabase.Contains(a.gameObject)).ToList();
    }
}

#endif

