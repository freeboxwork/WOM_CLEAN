using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.Linq;


[CustomEditor(typeof(CustomTypeDataManager))]
public class CustomTypeDataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();
        
        CustomTypeDataManager _target = (CustomTypeDataManager)target;
        GUILayout.Space(10);
        if (GUILayout.Button("Get All Custom Type Objects In Scene"))
        {
            GetAllCustomTypeObjects(_target);
        }
    }

    void GetAllCustomTypeObjects(CustomTypeDataManager target)
    {
        target.customTypeDatas.Clear();
        target.customTypeDatas_animCont.Clear();
        target.customTypeDatas_uiButton.Clear();
        target.customTypeDatas_uiText.Clear();
        target.customTypeDatas_uiImage.Clear();
        target.customTypeDatas_transform.Clear();
        target.customTypeDatas_gameObject.Clear();

        List<CustomTypeData> objectsInScene = new List<CustomTypeData>();

        foreach (CustomTypeData go in Resources.FindObjectsOfTypeAll(typeof(CustomTypeData)) as CustomTypeData[])
        {
            if (!EditorUtility.IsPersistent(go.transform.root.gameObject) && !(go.hideFlags == HideFlags.NotEditable || go.hideFlags == HideFlags.HideAndDontSave))
                objectsInScene.Add(go);
        }

        target.customTypeDatas = objectsInScene.ToList();
        target.customTypeDatas_animCont   = target.customTypeDatas.Where(w => w.customType == EnumDefinition.CustomDataType.animCont).ToList();
        target.customTypeDatas_uiButton   = target.customTypeDatas.Where(w => w.customType == EnumDefinition.CustomDataType.button).ToList();
        target.customTypeDatas_uiText     = target.customTypeDatas.Where(w => w.customType == EnumDefinition.CustomDataType.text).ToList();
        target.customTypeDatas_uiImage    = target.customTypeDatas.Where(w => w.customType == EnumDefinition.CustomDataType.image).ToList();
        target.customTypeDatas_transform = target.customTypeDatas.Where(w => w.customType == EnumDefinition.CustomDataType.tr).ToList();
        target.customTypeDatas_gameObject = target.customTypeDatas.Where(w => w.customType == EnumDefinition.CustomDataType.gm).ToList();
    }



}
